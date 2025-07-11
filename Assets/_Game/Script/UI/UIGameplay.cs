namespace Luzart
{
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine.UI;
    using UnityEngine;
    using DG.Tweening;
    using UnityEngine.Device;
    
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
        private float maxClockwise = 0.8f;
        public Image imClockFilled;
        public ParticleSystem fxLine;
        public CanvasGroup canvasGroupStartGame;
        public CanvasGroup canvasGroupScreen;
        public GameObject obScreen;
    
        protected override void Setup()
        {
            base.Setup();
            GameUtil.ButtonOnClick(btnPause, ClickPause, true,KeyAds.BtnGameplayResume);
        }
    
        private void ClickPause()
        {
            var ui = UIManager.Instance.ShowUI<UIResume>(UIName.Resume);
        }
        public override void Show(Action onHideDone)
        {
            base.Show(onHideDone);
            SetStatusUIController(false);
            canvasGroupScreen.gameObject.SetActive(false);
            canvasGroupStartGame.gameObject.SetActive(false);
        }
        private Sequence sequenceCanvasGroup;
        public void FadeOutCanvasGroupStartGame(float time, Action onComplete = null)
        {
            sequenceCanvasGroup?.Kill();
            sequenceCanvasGroup = DOTween.Sequence();
            canvasGroupStartGame.alpha = 1;
            float timePlay = time;
            sequenceCanvasGroup.Append(
                DOVirtual.Float(1, 0, timePlay, (x) =>
            {
                canvasGroupStartGame.alpha = x;
            }));
            sequenceCanvasGroup.AppendCallback(
            () =>
                {
                    SetShowScreen(true);
                    canvasGroupStartGame.gameObject.SetActive(false);
                    StartGame();
                    onComplete?.Invoke();
                }
                );
        }
        private Tweener twBlackScreen;
        public Tweener SetBlackScreen(bool status)
        {
            if (status)
            {
                twBlackScreen = DOVirtual.Float(0, 1, 0.5f, (x) =>
                {
                    canvasGroupStartGame.alpha = x;
                });
            }
            else
            {
                twBlackScreen = DOVirtual.Float(1, 0, 0.5f, (x) =>
                {
                    canvasGroupStartGame.alpha = x;
                });
                twBlackScreen.OnComplete(() =>
                {
                    canvasGroupStartGame.gameObject.SetActive(false);
                });
            }
            return twBlackScreen;
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
        public void UpdateDistanceLeaderBoard(List<DB_LeaderBoardInGame> list)
        {
            leaderBoard.UpdateDistance(list);
        }
        public void UpdateUI()
        {
            var myMotorbike = GameManager.Instance.gameCoordinator.myMotorbike;
            int round = myMotorbike.round;
            txtLap.text = $"{round}/{db_Level.lapRequire}";
            txtTime.text = GameUtil.FloatTimeSecondToUnixTime(GameManager.Instance.gameCoordinator.timePlay, true, "", "", "", "");
    
            int speed = (int)myMotorbike.Speed;
            int maxSpeed = (int)myMotorbike.inforMotorbike.maxSpeed;
    
            txtVelocity.text = $"{speed}";
    
            float factor = (float)speed / (float)maxSpeed;
            float valueClock = (maxClockwise - minClockwise) * factor;
            imClockFilled.fillAmount = minClockwise + valueClock;
    
            SetFXLineSpeed(speed, maxSpeed);
        }
        public void StartCountDown(Action action)
        {
            countdown.InitCountDown(3, 0, ()=>
            {
                countdown.txt.text = "GO!";
                DOVirtual.DelayedCall(1f, () =>
                {
                    StartGame();
                    action?.Invoke();
                }).SetId(this);
            }) ;
        }
        public void StartGame()
        {
            countdown.gameObject.SetActive(false);
            SetStatusUIController(true);
        }
        public void EndGame()
        {
    
        }
        private Tweener twShowScreen;
        public Tweener SetShowScreen(bool status)
        {
            canvasGroupScreen.gameObject.SetActive(true);
            if (status)
            {
               twShowScreen = DOVirtual.Float(0, 1, 0.5f, (x) =>
               {
                    canvasGroupScreen.alpha = x;
               });
            }
            else
            {
                twShowScreen = DOVirtual.Float(1, 0, 0.5f, (x) =>
                {
                    canvasGroupScreen.alpha = x;
                });
                twShowScreen.OnComplete(() =>
                {
                    canvasGroupScreen.gameObject.SetActive(false);
                });
            }
            return twShowScreen;
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
        private void OnDisable()
        {
            this.DOKill();
        }
    }
}
