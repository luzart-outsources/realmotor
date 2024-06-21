using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MotorSO", fileName = "MotorSO")]
public class MotorSO : ScriptableObject
{
    public MotorInforPath[] motorModelInfor;
    public DB_Motor[] dB_Motors;
    public DB_Motor[] db_MotorBots;

    private const string PATH_MOTOR = "Motorbike";

    public string GetPath(int id)
    {
        return $"{PATH_MOTOR}/{GetPathMotorVisualRaw(id)}";
    }
    private string GetPathMotorVisualRaw(int id)
    {
        for (int i = 0; i < motorModelInfor.Length; i++)
        {
            if (motorModelInfor[i].id == id)
            {
                return motorModelInfor[i].path;
            }
        }
        return null;
    }
    public DB_Motor GetDBMotor(int id)
    {
        return dB_Motors[id];
    }

    public DB_Motor GetDBMotorBot(int id)
    {
        return db_MotorBots[id];
    }

}
[System.Serializable]
public struct MotorInforPath
{
    public int id;
    public string path;
}
[System.Serializable]
public class DB_Motor
{
    public int idMotor;
    public InforMotorbike inforMotorbike;
    public InforMotorbike inforUpgrade;
    public InforMotorbike GetInforMotorbike(int levelUpgrade)
    {
        InforMotorbike infor = new InforMotorbike();
        infor.acceleration = inforMotorbike.acceleration + inforUpgrade.acceleration * levelUpgrade;
        infor.maxSpeed = inforMotorbike.maxSpeed + inforUpgrade.maxSpeed * levelUpgrade;
        infor.handling = inforMotorbike.handling + inforUpgrade.handling * levelUpgrade;
        infor.brake = inforMotorbike.brake + inforUpgrade.brake * levelUpgrade;
        return infor;
    }

}
