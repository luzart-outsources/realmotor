using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MotorbikeSO", fileName = "MotorbikeSO")]
public class MotorbikeSO : ScriptableObject
{
    public MotorSO motorSO;
    public CharacterSO characterSO;
    public DB_MotorbikeBot[] db_Bots;

    [Button]
    private void ResetDB()
    {
        int count = 0;
        List<DB_MotorbikeBot> list = new List<DB_MotorbikeBot>();
        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < motorSO.db_MotorBots.Length; i++)
            {
                DB_MotorbikeBot bot = new DB_MotorbikeBot();
                bot.db_Motorbike = new DB_Motorbike();
                bot.db_Motorbike.idMotor = i;
                bot.db_Motorbike.levelUpgrade = j;
                bot.id = count;
                bot.name = $"BOT {Random.Range(i, 1000)}";
                bot.idCharacterBot = Random.Range(0,characterSO.dB_CharacterBots.Length);
                list.Add(bot);
                count++;
            }
        }
        db_Bots = list.ToArray();

    }
    public DB_MotorbikeBot GetMotorbikeBot(int id)
    {
        return db_Bots[id];
    }
}

[System.Serializable]
public class DB_MotorbikeBot
{
    public int id;
    public string name;
    public int idCharacterBot;
    public DB_Motorbike db_Motorbike;
}
[System.Serializable]
public class DB_Motorbike
{
    public DB_Motorbike()
    {

    }
    public DB_Motorbike(int idMotor, int levelUpgrade = 0)
    {
        this.idMotor = idMotor;
        this.levelUpgrade = levelUpgrade;
    }
    public int idMotor;
    public int[] levelUpgrades = new int[] { 0, 0, 0, 0 };
    [JsonIgnore]
    public int levelUpgrade;
    [JsonIgnore]
    public bool isHas;
}

