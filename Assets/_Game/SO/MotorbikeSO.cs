using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MotorbikeSO", fileName = "MotorbikeSO")]
public class MotorbikeSO : ScriptableObject
{
    public DB_MotorbikeBot[] db_Bots;
    private void Reset()
    {
        int count = 0;
        List<DB_MotorbikeBot> list = new List<DB_MotorbikeBot>();
        for (int j = 0; j < 11; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                DB_MotorbikeBot bot = new DB_MotorbikeBot();
                bot.db_Motorbike = new DB_Motorbike();
                bot.db_Motorbike.idMotor = i;
                bot.db_Motorbike.levelUpgrade = j;
                bot.id = count;
                bot.idCharacterBot = 0;
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
    public int idCharacterBot;
    public DB_Motorbike db_Motorbike;
}
[System.Serializable]
public class DB_Motorbike
{
    public int idMotor;
    public int levelUpgrade;
}

