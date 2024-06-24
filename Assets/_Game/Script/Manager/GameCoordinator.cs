using MoreMountains.HighroadEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameCoordinator : MonoBehaviour
{
    public Action ActionOnLoadDoneLevel = null;
    public Action<BaseMotorbike> ActionOnFinishLine = null;
    public Action<bool> ActionOnEndGame = null;

    public WavingPointGizmos wavingPointGizmos;

    [SerializeField]
    private BaseMotorbike baseMotorBike;
    [SerializeField]
    private Transform parentEnvironment;
    [SerializeField]
    private Transform parentMotor;

    private EnvironmentMap environmentMap;
    private List<BaseMotorbike> listBot = new List<BaseMotorbike>();
    private List<DB_MotorbikeBot> listDBBot = new List<DB_MotorbikeBot>();
    private BaseMotorbike myMotorbike;
    private DB_Motorbike myDBMotorbike = null;


    private List<BaseMotorbike> leaderBoard = new List<BaseMotorbike>();
    private DB_Level db_Level;

    public void StartGame(DB_Level _dbLevel)
    {
        db_Level = _dbLevel;
        ResetData();
        LoadMap();
        LoadPlayer();
        LoadBot();

        InitMap();
        InitPlayer();
        InitBot();
        StartUpdateLeaderBoard();
    }
    private void ResetData()
    {
        listBot.Clear();
        listDBBot.Clear();
        myMotorbike = null;
        myDBMotorbike = null;
    }
    private void LoadMap()
    {
        var enviMap = ResourcesManager.Instance.LoadMap(db_Level.idEnvironment);
        environmentMap = Instantiate(enviMap, parentEnvironment);
        environmentMap.transform.localPosition = Vector3.zero;
        environmentMap.transform.rotation = Quaternion.identity;
    }
    private void LoadPlayer()
    {
        DB_Character dbChar = DataManager.Instance.GameData.curCharacter;
        int idCurMotor = DataManager.Instance.GameData.idCurMotor;
        int levelUpgrade = ConfigStats.GetLevelUpgrade(idCurMotor, DataManager.Instance.GameData.motorbikeDatas);

        myDBMotorbike = new DB_Motorbike();
        myDBMotorbike.idMotor = idCurMotor;
        myDBMotorbike.levelUpgrade = levelUpgrade;

        myMotorbike = Instantiate(baseMotorBike, parentMotor);
        myMotorbike.transform.position = environmentMap.GetStartPoint(db_Level.indexStart).position;
        SetNameEditor(myMotorbike.gameObject, $"Player");
        myMotorbike.InitSpawn(dbChar, myDBMotorbike);
    }
    private void LoadBot()
    {
        var ids = db_Level.idBot;
        var indexS = db_Level.indexStartBot;

        for (int i = 0; i < ids.Length; i++)
        {
            BaseMotorbike baseBot = Instantiate(baseMotorBike, parentMotor);
            baseBot.transform.position = environmentMap.GetStartPoint(indexS[i]).position;

            DB_MotorbikeBot db = DataManager.Instance.motorbikeSO.GetMotorbikeBot(ids[i]);
            DB_Character character = DataManager.Instance.characterSO.GetDB_CharacterBot(db.idCharacterBot);

            baseBot.InitSpawn(character, db.db_Motorbike);
            SetNameEditor(baseBot.gameObject, $"BOT_{i}");

            listBot.Add(baseBot);
            listDBBot.Add(db);
        }
    }

    private void InitMap()
    {
        wavingPointGizmos = environmentMap.wavingPointGizmos;
    }
    private void InitPlayer()
    {
        InforMotorbike infor = ConfigStats.GetInforMotorbike(myDBMotorbike.idMotor, myDBMotorbike.levelUpgrade);
        BaseController controller = myMotorbike.AddComponent<BaseController>();
        myMotorbike.Initialize(infor, controller, ETeam.Player);
        myMotorbike.InitStartRace();

    }
    private void InitBot()
    {
        var ids = db_Level.idBot;
        int length = listBot.Count; 

        for (int i = 0; i < length; i++)
        {
            var db = listDBBot[i];
            var bot = listBot[i];
            InforMotorbike infor = ConfigStats.GetInforMotorbikeBot(db.db_Motorbike.idMotor, db.db_Motorbike.levelUpgrade);
            VehicleAI controller = bot.AddComponent<VehicleAI>();
            bot.Initialize(infor,controller, ETeam.AI);
            bot.InitStartRace();
        }
    }

    public void OnPassFinishLine(BaseMotorbike motorFinish)
    {
        if(motorFinish.round >= db_Level.lapRequire)
        {
            motorFinish.OnFinishRace();
        }
        if(motorFinish.eTeam == ETeam.Player)
        {
            EndGame();
            UpdateLeaderBoard();
        }
    }
    private void EndGame()
    {
        StopUpdateLeaderBoard();
    }
    private void StartUpdateLeaderBoard()
    {
        listLeaderBoard = new List<BaseMotorbike>(listBot);
        listLeaderBoard.Add(myMotorbike);
        StopUpdateLeaderBoard();
        corUpdateLeaderBoard = StartCoroutine(IEUpdateLeaderBoard());
    }
    private void StopUpdateLeaderBoard()
    {
        if (corUpdateLeaderBoard != null)
        {
            StopCoroutine(corUpdateLeaderBoard);
        }
    }
    private Coroutine corUpdateLeaderBoard = null;
    private IEnumerator IEUpdateLeaderBoard()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (true)
        {
            UpdateLeaderBoard();
            yield return wait;
        }
    }

    public List<BaseMotorbike> listLeaderBoard = new List<BaseMotorbike>();
    private void UpdateLeaderBoard()
    {
        listLeaderBoard.Sort((x, y) =>
        {
            int currentComparison = y.currentIndex.CompareTo(x.currentIndex);
            if (currentComparison == 0)
            {
                return y.GetDistanceFromTarget().CompareTo(x.GetDistanceFromTarget());
            }
            return currentComparison;
        });
    }




    private void SetNameEditor(GameObject ob, string name)
    {
#if UNITY_EDITOR
        ob.name = name;
#endif
    }
}
