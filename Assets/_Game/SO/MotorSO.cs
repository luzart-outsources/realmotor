using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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

    //[Button]
    //public void AddMotorModelInfor()
    //{
    //    List<MotorInforPath> list = new List<MotorInforPath>();
    //    list = motorModelInfor.ToList();
    //    MotorInforPath newM = new MotorInforPath();
    //    newM.id = list.Count;
    //    newM.path = $"Motor_{newM.id}";
    //    list.Add(newM);
    //    motorModelInfor = list.ToArray();
    //}
    //[Button]
    //public void AddDB_Motor()
    //{
    //    int length = 14;
    //    string[] tenXes = new string[] { "Xe Rom Nhat", "Xe Rom Vua", "Xe Rom", "Xe binh thuong", "Xe Xin", "Xe Xin Hon", "Xe Xin Nhat" };
    //    int lengthTenXe = tenXes.Length;
    //    List<DB_Motor> list = new List<DB_Motor>();
    //    for (int i = 0; i < length; i++)
    //    {
    //        DB_Motor _ = new DB_Motor();
    //        int indexTenXe = Mathf.RoundToInt( ((float)(lengthTenXe-1) / (float)(length - 1)) * (float)i);
    //        _.nameMotor = $"{tenXes[indexTenXe]} {i}";
    //        _.nameModelMotor = "Khum bit dou";
    //        int indexRank = Mathf.RoundToInt(((float)(5-1)/ (float)(length-1)) * (float)i);
    //        _.rank = (ClassRankMotor)indexRank;
    //        _.idMotor = i;
    //        _.inforMotorbike = new InforMotorbike();
    //        _.inforMotorbike.maxSpeed = 50 + 5 * i;
    //        _.inforMotorbike.acceleration = 10 + 5 * i;
    //        _.inforMotorbike.handling = 30 + 5 * i;
    //        _.inforMotorbike.brake = 20 + 5 * i;
    //        _.inforUpgrade = new InforMotorbike();
    //        _.inforUpgrade.maxSpeed = 5 +  i;
    //        _.inforUpgrade.acceleration = 4 +  i;
    //        _.inforUpgrade.handling = 3 +  i;
    //        _.inforUpgrade.brake = 2 +  i;
    //        list.Add(_);
    //    }
    //    dB_Motors = list.ToArray();

    //}
    //[Button]
    //public void ChangeDataMotor()
    //{
    //    int length = dB_Motors.Length;
    //    //dB_Motors[0].inforMotorbike.maxSpeed = 80;
    //    for (int i = 0; i < length; i++)
    //    {
    //        InforMotorbike infor = new InforMotorbike();
    //        if(i-1 >= 0)
    //        {
    //            infor.maxSpeed = dB_Motors[i - 1].inforMotorbike.maxSpeed + Random.Range(5,10);
    //            infor.acceleration = dB_Motors[i - 1].inforMotorbike.acceleration + 5;
    //            infor.handling = dB_Motors[i - 1].inforMotorbike.handling + 5;
    //            infor.brake = dB_Motors[i - 1].inforMotorbike.brake + 5 ;
    //            dB_Motors[i].inforMotorbike = infor;
    //        }

           
    //        InforMotorbike inforUpgrade = new InforMotorbike();
    //        inforUpgrade.maxSpeed = 1;
    //        inforUpgrade.acceleration = 1;
    //        inforUpgrade.handling = 1;
    //        inforUpgrade.brake = 1;

    //        dB_Motors[i].inforUpgrade = inforUpgrade;

    //    }
    //}
    [Button]
    public void ChangeDataMotorBot()
    {
        int length = db_MotorBots.Length;
        db_MotorBots[0].inforMotorbike.maxSpeed = 50;
        for (int i = 0; i < length; i++)
        {
            InforMotorbike infor = new InforMotorbike();
            if (i - 1 >= 0)
            {
                infor.maxSpeed = db_MotorBots[i - 1].inforMotorbike.maxSpeed + Random.Range(3,8);
            }
            else
            {
                infor.maxSpeed = 50;
            }

            infor.acceleration = 5 + 5 * i;
            infor.handling = 20 + 5 * i;
            infor.brake = 10 + 5 * i;
            InforMotorbike inforUpgrade = new InforMotorbike();
            inforUpgrade.maxSpeed = 1;
            inforUpgrade.acceleration = 1;
            inforUpgrade.handling = 1;
            inforUpgrade.brake =1;
            db_MotorBots[i].inforMotorbike = infor;
            db_MotorBots[i].inforUpgrade = inforUpgrade;

        }
    }
    //    [Button]
    //    public void AddDB_MotorBot()
    //    {
    //        int length = 14;
    //        string[] tenXes = new string[] { "Xe Rom Nhat", "Xe Rom Vua", "Xe Rom", "Xe binh thuong", "Xe Xin", "Xe Xin Hon", "Xe Xin Nhat" };
    //        int lengthTenXe = tenXes.Length;
    //        List<DB_Motor> list = new List<DB_Motor>();
    //        for (int i = 0; i < length; i++)
    //        {
    //            DB_Motor _ = new DB_Motor();
    //            int indexTenXe = Mathf.RoundToInt(((float)(lengthTenXe - 1) / (float)(length - 1)) * (float)i);
    //            _.nameMotor = $"{tenXes[indexTenXe]} {i}";
    //            _.nameModelMotor = "Khum bit dou";
    //            int indexRank = Mathf.RoundToInt(((float)(5 - 1) / (float)(length - 1)) * (float)i);
    //            _.rank = (ClassRankMotor)indexRank;
    //            _.idMotor = i;
    //            _.inforMotorbike = new InforMotorbike();
    //            _.inforMotorbike.maxSpeed = 20 + 5 * i;
    //            _.inforMotorbike.acceleration = 5 + 5 * i;
    //            _.inforMotorbike.handling = 10 + 5 * i;
    //            _.inforMotorbike.brake = 15 + 5 * i;
    //            _.inforUpgrade = new InforMotorbike();
    //            _.inforUpgrade.maxSpeed = 5 + i;
    //            _.inforUpgrade.acceleration = 4 + i;
    //            _.inforUpgrade.handling = 3 + i;
    //            _.inforUpgrade.brake = 2 + i;
    //            list.Add(_);
    //        }
    //        db_MotorBots = list.ToArray();

    //    }
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
    public string nameMotor;
    public string nameModelMotor;
    public ClassRankMotor rank;
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
    public InforMotorbike GetInforMotorbike(int[] levelUpgrade)
    {
        InforMotorbike infor = new InforMotorbike();
        infor.maxSpeed = inforMotorbike.maxSpeed + inforUpgrade.maxSpeed * levelUpgrade[0];
        infor.acceleration = inforMotorbike.acceleration + inforUpgrade.acceleration * levelUpgrade[1];
        infor.handling = inforMotorbike.handling + inforUpgrade.handling * levelUpgrade[2];
        infor.brake = inforMotorbike.brake + inforUpgrade.brake * levelUpgrade[3];
        return infor;
    }
    public InforMotorbike GetInforMotorbikeMax()
    {
        int[] levelUpgrades = new int[] { 5, 5, 5, 5 };
        return GetInforMotorbike(levelUpgrades);
    }

    public DB_Motor Clone()
    {
        DB_Motor newDB = new DB_Motor();
        newDB.nameMotor = nameMotor;
        newDB.nameModelMotor = nameModelMotor;
        newDB.rank = rank;
        newDB.idMotor = idMotor;
        newDB.inforMotorbike = inforMotorbike.Clone();
        newDB.inforUpgrade = inforUpgrade.Clone();
        return newDB;
    }
}
public enum ClassRankMotor
{
    D =0,
    C =1,
    B = 2,
    A =3,
    S =4,
}
