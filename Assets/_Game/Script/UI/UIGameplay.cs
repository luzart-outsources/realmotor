using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIGameplay : UIBase
{
    public UIController UIController;
    public Button btnPause;
    public LeaderBoardUIInGame leaderBoard;
    public TMP_Text txtTime;
    public TMP_Text txtLap;
    public TMP_Text txtVelocity;
    public CountdownUI countdown;
    private DB_Level db_Level;
    private float minClockwise = 0.1f;
    private float maxClockwise = 0.72f;
    public Image imClockFilled;
    public ParticleSystem fxLine;
    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnPause, ClickPause, true);
    }

    private void ClickPause()
    {
        var ui = UIManager.Instance.ShowUI<UIResume>(UIName.Resume);
    }
    public override void Show(Action onHideDone)
    {
        base.Show(onHideDone);
        SetStatusUIController(false);
    }
    public void SetStatusUIController(bool status)
    {
        UIController.gameObject.SetActive(status);
    }
    public void InitData(DB_Level db)
    {
        db_Level = db;
    }
    public void InitList(List<DB_LeaderBoardInGame> list)
    {
        leaderBoard.InitList(list);
    }
    public void UpdateLeaderBoard(List<DB_LeaderBoardInGame> list)
    {
        leaderBoard.UpdateList(list);
    }
    public void UpdateUI()
    {
       int round =  GameManager.Instance.gameCoordinator.myMotorbike.round;
       txtLap.text = $"{round}/{db_Level.lapRequire}";
       int speed = (int)GameManager.Instance.gameCoordinator.myMotorbike.Speed;
       int maxSpeed = (int)GameManager.Instance.gameCoordinator.myMotorbike.inforMotorbike.maxSpeed;
       txtVelocity.text = $"{speed}";
       float factor = (float)speed / (float)maxSpeed;
       float valueClock = (maxClockwise - minClockwise) * factor;
       imClockFilled.fillAmount = minClockwise + valueClock; 
       txtTime.text = GameUtil.FloatTimeSecondToUnixTime(GameManager.Instance.gameCoordinator.timePlay, false, "", "", "", "");

    }
    public void StartCountDown()
    {
        countdown.InitCountDown(3, 0, ()=> StartGame()) ;
    }
    public void StartGame()
    {
        countdown.gameObject.SetActive(false);
        SetStatusUIController(true);
    }
    public float emissionMax = 400;
    public float emissionMin = 100;
    public void SetFXLineSpeed(float velocity, float maxSpeed)
    {
        var emission = fxLine.emission;

        if(velocity > 50)
        {
            emission.rateOverTime = emissionMin+ (emissionMax - emissionMin)* (velocity/ maxSpeed);
        }
        else
        {
            emission.rateOverTime = 0;
        }
    }
}
