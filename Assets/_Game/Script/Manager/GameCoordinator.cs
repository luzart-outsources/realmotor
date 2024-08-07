using DG.Tweening;
using MoreMountains.HighroadEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class GameCoordinator : MonoBehaviour
{
    public Action ActionOnLoadDoneLevel = null;
    public Action<BaseMotorbike> ActionOnFinishLine = null;
    public Action<bool> ActionOnEndGame = null;

    public WavingPointGizmos wavingPointGizmos;
    public MiniMapEnvironment miniMapEnvironment;

    [SerializeField]
    private BaseMotorbike baseMotorBike;
    [SerializeField]
    private Transform parentEnvironment;
    [SerializeField]
    private Transform parentMotor;

    private EnvironmentMap environmentMap;
    private List<BaseMotorbike> listBot = new List<BaseMotorbike>();
    private List<DB_MotorbikeBot> listDBBot = new List<DB_MotorbikeBot>();
    public BaseMotorbike myMotorbike;
    private DB_Motorbike myDBMotorbike = null;


    public List<BaseMotorbike> listLeaderBoard = new List<BaseMotorbike>();
    private List<DB_LeaderBoardInGame> listDBLeaderBoardInGame = new List<DB_LeaderBoardInGame>();


    public int countLeaderBoard = 0;
    public DB_Level db_Level;


    public float timePlay = 0;
    private UIGameplay uiGameplay = null;

#if UNITY_EDITOR
    public DB_Level db_levelEditor;
#endif
    public void StartTestLevel()
    {
#if UNITY_EDITOR
        EnvironmentMap envi = FindAnyObjectByType<EnvironmentMap>();
        ResetData();
        db_Level = db_levelEditor;
        CameraManager.Instance.helicopterCamera.gameObject.SetActive(true);
        EnvironmentMap.actionMap = OnLoadMapDone;

        uiGameplay = UIManager.Instance.ShowUI<UIGameplay>(UIName.Gameplay);
        uiGameplay.InitData(db_Level);
        StartInGame();
        envi.InvokeRegisterMap();

#endif
    }

    public void StartGame(DB_Level _dbLevel)
    {
        ResetData();
        db_Level = _dbLevel;


        CameraManager.Instance.helicopterCamera.gameObject.SetActive(true);
        EnvironmentMap.actionMap = OnLoadMapDone;

        LoadMap();
        uiGameplay = UIManager.Instance.ShowUI<UIGameplay>(UIName.Gameplay);
        uiGameplay.InitData(db_Level);


    }
    private void OnLoadMapDone(EnvironmentMap map)
    {
        environmentMap = map;
        environmentMap.transform.localPosition = Vector3.zero;
        environmentMap.transform.rotation = Quaternion.identity;
        LoadPlayer();
        LoadBot();

        InitMap();
        InitPlayer();
        InitBot();
        InitLeaderBoard();
        InitAllOtherData();
    }
    public void ShowHideUIController(bool status)
    {
        uiGameplay.SetStatusUIController(status);
    }
    public void StartInGame()
    {
        AudioManager.Instance.PlayMusicBgInGame();
        uiGameplay.StartCountDown();
        DOVirtual.DelayedCall(3, StartRace);
    }
    public void StartRace()
    {
        foreach (var item in listLeaderBoard)
        {
            item.StartRace();
        }
        StartUpdateLeaderBoard();
    }
    private void ResetData()
    {
        timePlay = 0;
        listBot.Clear();
        listDBBot.Clear();
        myMotorbike = null;
        myDBMotorbike = null;
        listResult = new List<BaseMotorbike>();
    }
    private void InitAllOtherData()
    {
        UpdateDbLeaderBoardInGameUI();
        uiGameplay.InitList(listDBLeaderBoardInGame);

    }
    private void LoadMap()
    {
        ResourcesManager.Instance.LoadMapScene(db_Level.idEnvironment);
    }
    private void LoadPlayer()
    {
        DB_Character dbChar = DataManager.Instance.GameData.curCharacter;
        int idCurMotor = DataManager.Instance.GameData.idCurMotor;
        int[] levelUpgrades = ConfigStats.GetLevelsUpgrade(idCurMotor, DataManager.Instance.GameData.motorbikeDatas);

        myDBMotorbike = new DB_Motorbike();
        myDBMotorbike.idMotor = idCurMotor;
        myDBMotorbike.levelUpgrades = levelUpgrades;

        myMotorbike = Instantiate(baseMotorBike, parentMotor);
        Transform transPos = environmentMap.GetStartPoint(db_Level.indexStart);
        myMotorbike.transform.position = transPos.position;
        myMotorbike.transform.rotation = transPos.rotation;
        SetNameEditor(myMotorbike.gameObject, $"Player");
        myMotorbike.InitSpawn(dbChar, myDBMotorbike);
    }
    private void LoadBot()
    {
        var ids = db_Level.idBot;
        var indexS = db_Level.indexStartBot;

        for (int i = 0; i < ids.Length; i++)
        {
            BaseMotorbike baseBot = Instantiate(baseMotorBike, environmentMap.parentMotorbike);
            Transform transPos = environmentMap.GetStartPoint(indexS[i]);
            baseBot.transform.position = transPos.position;
            baseBot.transform.rotation = transPos.rotation;

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
        miniMapEnvironment = environmentMap.miniMapEnvironment;
    }
    private void InitPlayer()
    {
        InforMotorbike infor = ConfigStats.GetInforMotorbike(myDBMotorbike.idMotor, myDBMotorbike.levelUpgrades);
        //BaseController controller = myMotorbike.AddComponent<VehicleAI>();
        BaseController controller = uiGameplay.UIController;
        myMotorbike.Initialize(infor, controller, ETeam.Player);
        myMotorbike.InitStartRace();
        myMotorbike.strMyName = DataManager.Instance.GameData.name;

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
            bot.strMyName = db.name;
        }
    }
    public List<BaseMotorbike> listResult = new List<BaseMotorbike>();
    public void OnPassFinishLine(BaseMotorbike motorFinish)
    {
        if(motorFinish.round >= db_Level.lapRequire)
        {
            listResult.Add(motorFinish);
            motorFinish.OnFinishRace();
            if (motorFinish.eTeam == ETeam.Player)
            {
                StopUpdateLeaderBoard();
                UpdateCoordinator();
                UpdateLeaderBoard();
                countLeaderBoard = listResult.Count - 1; 
                bool isWin = listResult.Count <= 3;
                GameUtil.Instance.WaitAndDo(3f, () => ActionOnEndGame?.Invoke(isWin));

            }
        }

    }
    public void EndGame(bool isWin)
    {
        CameraManager.Instance.helicopterCamera.animator.enabled = false;
        CameraManager.Instance.helicopterCamera.transform.SetParent(CameraManager.Instance.transform);
        OnCompleteDataLeaderboard();
        StopUpdateLeaderBoard();
        DestroyAllBike();
        void DestroyAllBike()
        {
            Destroy(myMotorbike.gameObject);
            for (int i = 0; i < listBot.Count; i++)
            {
                Destroy(listBot[i].gameObject);
                listBot.RemoveAt(0);
            }
        }
    }
    public List<DataItemWinLeaderboardUI> listDataItemWinLeaderBoard = new List<DataItemWinLeaderboardUI>();
    private void OnCompleteDataLeaderboard()
    {
        listDataItemWinLeaderBoard = new List<DataItemWinLeaderboardUI>();
        List<BaseMotorbike> listRemain = listLeaderBoard.Except(listResult).ToList();
        int length = listResult.Count;
        int countIndex = 0;
        for (int i = 0; i < length; i++)
        {
            var item = listResult[i];
            DataItemWinLeaderboardUI data = new DataItemWinLeaderboardUI();
            data.index = (countIndex + 1).ToString();
            data.time = GameUtil.FloatTimeSecondToUnixTime(item.timePlay,true);
            DB_Motor motor = DataManager.Instance.motorSO.GetDBMotor(item.dbMotorbike.idMotor);
            data.nameModel = motor.nameMotor;

            data.timeAll = ((int)item.inforMotorbike.PR).ToString();
            if (item.eTeam == ETeam.Player)
            {
                data.name = DataManager.Instance.GameData.name;
            }
            else
            {
                data.name = item.strMyName;
            }
            listDataItemWinLeaderBoard.Add(data);
            countIndex++;
        }
        int lengthExp = listRemain.Count;
        for (int i = 0; i < lengthExp;i++)
        {
            var item = listRemain[i];
            DataItemWinLeaderboardUI data = new DataItemWinLeaderboardUI();
            data.index = (countIndex + 1).ToString();
            float timeEach = timePlay + DisFromTarget(item) / (item.inforMotorbike.maxSpeed + UnityEngine.Random.Range(-5,5));
            data.time = GameUtil.FloatTimeSecondToUnixTime(timeEach,true);
            DB_Motor motor = DataManager.Instance.motorSO.GetDBMotor(item.dbMotorbike.idMotor);
            data.nameModel = motor.nameMotor;

            data.timeAll = ((int)item.inforMotorbike.PR).ToString();
            if (item.eTeam == ETeam.Player)
            {
                data.name = DataManager.Instance.GameData.name;
            }
            else
            {
                data.name = item.strMyName;
            }
            listDataItemWinLeaderBoard.Add(data);
            countIndex++;
        }
    }
    private void InitLeaderBoard()
    {
        listLeaderBoard = new List<BaseMotorbike>(listBot)
        {
            myMotorbike
        };
    }
    private void StartUpdateLeaderBoard()
    {
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

        while (true)
        {
            yield return null;
            UpdateLeaderBoard();
            timePlay += Time.deltaTime;
            UpdateCoordinator();
        }
    }

    private void UpdateCoordinator()
    {
        if(uiGameplay == null)
        {
            return;
        }
        uiGameplay.UpdateUI();
        UpdateDbLeaderBoardInGameUI();
        uiGameplay.UpdateLeaderBoard(listDBLeaderBoardInGame);
        uiGameplay.SetFXLineSpeed(myMotorbike.Speed, myMotorbike.inforMotorbike.maxSpeed);
    }
    private void UpdateDbLeaderBoardInGameUI()
    {
        int length = listLeaderBoard.Count;
        listDBLeaderBoardInGame.Clear();
        for (int i = 0; i < length; i++)
        {
            DB_LeaderBoardInGame db = new DB_LeaderBoardInGame();
            db.index = i;
            db.name = listLeaderBoard[i].strMyName;
            db.eTeam = listLeaderBoard[i].eTeam;
            if (db.eTeam == ETeam.Player)
            {
                if (db.index == 0)
                {
                    db.distance = 0;
                }
                else
                {
                    db.distance = Mathf.Abs(DisFrom2Player(myMotorbike, listLeaderBoard[i - 1]));
                }
            }
#if ENABLE_TEST_LEADERBOARD
            db.round = listLeaderBoard[i].round;
            db.curIndex = listLeaderBoard[i].currentIndex;
            db.disFromIndex = listLeaderBoard[i].DistanceForWavingPoint();
#endif
            listDBLeaderBoardInGame.Add(db);
        }
    }
    private float DisFrom2Player(BaseMotorbike mine, BaseMotorbike enemy)
    {
        int cur = mine.currentIndex;
        int next = enemy.currentIndex;
        var disEnemy = enemy.GetDistanceFromTarget();
        var disMe = mine.GetDistanceFromTarget();

        int length = wavingPointGizmos.allWavePoint.Length;
        next = next + (enemy.round - mine.round)* length;
        if(cur == next)
        {
            return disMe - disEnemy;
        }
        cur++;
        next++;
        float disReal = 0;
        for (int i = cur  ;i < next;i++)
        {
            int indexMe = i % length;
            int indexReal = (i+1) % length;
            float dis = Vector3.Distance(wavingPointGizmos.allWavePoint[indexMe].transform.position, wavingPointGizmos.allWavePoint[indexReal].transform.position);
            disReal += dis;
        }
        disReal = disReal - disEnemy + disMe;

        return disReal;
    }
    private float DisFromTarget(BaseMotorbike mine)
    {
        int cur = mine.currentIndex;
        int next = wavingPointGizmos.GetLastIndex();
        var disMe = mine.GetDistanceFromTarget();
        int length = wavingPointGizmos.allWavePoint.Length;
        next = next + (db_Level.lapRequire - mine.round) * length;
        cur++;
        next++;
        float disReal = 0;
        for (int i = cur; i < next; i++)
        {
            int indexMe = i % length;
            int indexReal = (i + 1) % length;
            float dis = Vector3.Distance(wavingPointGizmos.allWavePoint[indexMe].transform.position, wavingPointGizmos.allWavePoint[indexReal].transform.position);
            disReal += dis;
        }
        disReal = disReal - disMe;

        return disReal;
    }

    private void UpdateLeaderBoard()
    {
        for (int i = 0; i < listLeaderBoard.Count; i++)
        {
            if(listLeaderBoard[i] == null)
            {
                return;
            }
        }
        listLeaderBoard.Sort((x, y) =>
        {
            int roundComparison = y.round.CompareTo(x.round);

            if (roundComparison == 0)
            {
                int currentComparison = y.currentIndex.CompareTo(x.currentIndex);
                if (currentComparison == 0)
                {
                    return x.DistanceForWavingPoint().CompareTo(y.DistanceForWavingPoint());
                }
                return currentComparison;
            }
            return roundComparison;
        });
    }




    private void SetNameEditor(GameObject ob, string name)
    {
#if UNITY_EDITOR
        ob.name = name;
#endif
    }
}
