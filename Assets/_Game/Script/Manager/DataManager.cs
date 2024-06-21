using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [Space, Header("BaseGameLuzart")]
    public GameData _gameData;
    public GameData GameData => _gameData;

    [Space, Header("SO")]
    public LevelSO levelSO;
    public MotorbikeSO motorbikeSO;
    public MotorSO motorSO;
    public CharacterSO characterSO;
    public EnvironmentSO environmentSO;


    private const string KEY_GAME_DATA = "key_gamedata";
    public void Initialize()
    {
        LoadGameData();
    }
    private void LoadGameData()
    {
        _gameData = SaveLoadUtil.LoadDataPrefs<GameData>(KEY_GAME_DATA);
        if(_gameData == null)
        {
            _gameData = new GameData();
        }
    }
    public void SaveGameData()
    {
        SaveLoadUtil.SaveDataPrefs<GameData>(_gameData, KEY_GAME_DATA);
    }

    public bool IsAddRes(DataResource dataResource)
    {
        return GameRes.isAddRes(dataResource);
    }
    public static int GetRes(DataTypeResource type)
    {
        return GameRes.GetRes(type);
    }
    public static void AddRes(DataTypeResource type, int amount)
    {
        GameRes.AddRes(type, amount);
    }
    public void ReceiveRes(params DataResource[] dataResource)
    {
        int length = dataResource.Length;
        for (int i = 0; i < length; i++)
        {
            DataResource resource = dataResource[i];
            AddRes(resource.type, resource.amount);
        }
    }
    public int Gold
    {
        get
        {
            DataTypeResource dataTypeResource = new DataTypeResource();
            dataTypeResource.type = RES_type.Gold;
            dataTypeResource.id = 0;
            return DataManager.GetRes(dataTypeResource);
        }

    }
    public int CurrentLevel => _gameData.level;
}
[System.Serializable]
public class GameData
{
    public int level = 0;
    public DB_Character curCharacter;
    public int idCurMotor;
    public DB_Motorbike[] motorbikeDatas;
    public void BuyMotorbike(DB_Motorbike db)
    {
        List<DB_Motorbike> list = new List<DB_Motorbike>();
        if (motorbikeDatas != null)
        {
            list = motorbikeDatas.ToList();
        }
        bool isAdd = true;
        for (int i = 0;i < list.Count;i++)
        {
            if (list[i].idMotor == db.idMotor)
            {
                isAdd = false;
                break;
            }
        }
        if(isAdd)
        {
            list.Add(db);
        }
        motorbikeDatas = list.ToArray();
    }
}
[System.Serializable]
public class InforMotorbike
{
    public float maxSpeed = 100;
    public float acceleration = 20;
    public float handling = 30f;
    public float brake = 50f;

    public InforMotorbike Clone()
    {
        InforMotorbike infor = new InforMotorbike();
        infor.maxSpeed = maxSpeed;
        infor.acceleration = acceleration;
        infor.handling = handling;
        infor.brake = brake;
        return infor;
    }
}
