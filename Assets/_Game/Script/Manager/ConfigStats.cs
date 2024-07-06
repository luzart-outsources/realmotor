using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ConfigStats 
{
    public static InforMotorbike GetInforMotorbike(int idMotor, int[] levelUpgrade)
    {
        InforMotorbike inforMotorbike = new InforMotorbike();
        inforMotorbike = DataManager.Instance.motorSO.GetDBMotor(idMotor).GetInforMotorbike(levelUpgrade);
        return inforMotorbike;
    }
    public static InforMotorbike GetInforMotorbikeBot(int idMotor, int levelUpgrade)
    {
        InforMotorbike inforMotorbike = new InforMotorbike();
        inforMotorbike = DataManager.Instance.motorSO.GetDBMotorBot(idMotor).GetInforMotorbike(levelUpgrade);
        return inforMotorbike;
    }
    public static int GetLevelUpgrade(int curMotor, List<DB_Motorbike> db)
    {
        for (int i = 0; i < db.Count; i++)
        {
            if (curMotor == db[i].idMotor)
            {
                return db[i].levelUpgrade;
            }
        }
        return 0;
    }
    public static int[] GetLevelsUpgrade(int curMotor, List<DB_Motorbike> db)
    {
        for(int i = 0;i < db.Count; i++)
        {
            if (curMotor == db[i].idMotor)
            {
                return db[i].levelUpgrades;
            }
        }
        return new int[4];
    }
}
public enum StatsMotorbike
{
    MaxSpeed = 0,
    Acceleration = 1,
    Handling = 2,
    Brake = 3,
}
