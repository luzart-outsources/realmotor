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
    }
    private void ResetData()
    {
        listBot.Clear();
        myMotorbike = null;
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
        }
    }

}
