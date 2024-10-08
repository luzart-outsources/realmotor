using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DB_ResourceSO", fileName ="DB_ResourceSO")]
public class DB_ResourceSO : ScriptableObject
{
    [Space, Header("PassLevel")]
    public DataResource dataResBaseLevel;
    public DataResource dataResUpgradeLevel;

    //[Space , Header("Position In Level")]
    //public DataResource dataResPos;
    //public DataResource dataResPosUpgrade;
    //public DataResource dataResPosUpgradeLevel;

    //[Space, Header("Join")]
    //public DataResource dataResJoin;
    //[Space, Header("Result")]
    //public DataResource dataResResultMin;
    //public DataResource dataResResultMax;
    public int[] GoldInLevel = new int[]
{
    600, 750, 900, 1050, 1200, 1350, 1500, 1650, 1800, 1950,
    2100, 2250, 2400, 2550, 2700, 2850, 3000, 3150, 3300, 3450,
    3600, 3750, 3900, 4050, 4200, 4350, 4500, 4650, 4800, 4950,
    5100, 5250, 5400, 5550, 5700, 5850, 6000, 6150, 6300, 6450,
    6600, 6750, 6900, 7050
};
    public DataResource GetDataResourcePosition(int pos, int level)
    {
        DataResource dataRes = new DataResource();
        dataRes.type = dataResBaseLevel.type;
        int gold = GoldInLevel[level];
        if(pos == 1)
        {
            gold = (int)((float)gold * 0.9f);
        }
        else if (pos == 2)
        {
            gold = (int)((float)gold * 0.8f);
        }
        else if (pos >= 3)
        {
            gold = (int)((float)gold * 1/6);
        }
        dataRes.amount = gold;
        return dataRes;
    }
    //public DataResource GetDataResResult()
    //{
    //    return new DataResource(dataResResultMax.type,Random.Range(dataResResultMin.amount, dataResResultMax.amount));
    //}

    //public DataResource GetDataResourceBaseLevel(int level)
    //{
    //    DataResource dataRes = new DataResource();
    //    dataRes.type = dataResBaseLevel.type;
    //    dataRes.amount = dataResBaseLevel.amount + dataResUpgradeLevel.amount*level;
    //    return dataRes;
    //}

    //public DataResource GetDataResourcePosition(int pos, int level)
    //{
    //    DataResource dataRes = new DataResource();
    //    dataRes.type = dataResPos.type;
    //    dataRes.amount = dataResPos.amount + dataResPosUpgrade.amount * pos + dataResUpgradeLevel.amount * level;
    //    return dataRes;
    //}

    public DataResource resGiftLevel;
}
