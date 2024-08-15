using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ConfigStats 
{
    public static float[] Gear = new float[]
    {
        //0.11f,
        //0.22f,
        0f,
        0.22f,
        0.66f,
        1,
    };
    public static int GetGear(float currentSpeed, float maxSpeed)
    {
        float percent = currentSpeed / maxSpeed;
        int length = Gear.Length;
        for (int i = 0; i < length; i++)
        {
            if(percent <= Gear[i])
            {
                return i;
            }
        }
        return length;
    }
    public static float GetRevValue(float currentSpeed, float maxSpeed)
    {
        int indexGear = GetGear(currentSpeed, maxSpeed);
        if(indexGear == 0)
        {
            return 0;
        }
        int indexpreGear = indexGear - 1;
        float valueGear = Gear[indexGear];
        float valuePreGear = Gear[indexpreGear];
        float valueSpeedGear = valueGear*maxSpeed;
        float valueSpeedPreGear = valuePreGear*maxSpeed;
        float total = valueSpeedGear - valueSpeedPreGear;
        float current = currentSpeed - valueSpeedPreGear;
        float revValue = current/ total;    
        return revValue;
    }
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
    public static int[] GetLevelsUpgrade(int curMotor)
    {
        var list = DataManager.Instance.GameData.motorbikeDatas;
        return GetLevelsUpgrade(curMotor, list);
    }
}
public enum StatsMotorbike
{
    MaxSpeed = 0,
    Acceleration = 1,
    Handling = 2,
    Brake = 3,
}
