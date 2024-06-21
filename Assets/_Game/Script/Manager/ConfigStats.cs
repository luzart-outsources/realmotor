using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ConfigStats 
{
    public static InforMotorbike GetInforMotorbike(int idMotor, int levelUpgrade)
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
    public static int GetLevelUpgrade(int curMotor, DB_Motorbike[] db)
    {
        for (int i = 0; i < db.Length; i++)
        {
            if (curMotor == db[i].idMotor)
            {
                return i;
            }
        }
        return 0;
    }
}
