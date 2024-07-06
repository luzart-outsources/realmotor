using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DB_ResourceSO", fileName ="DB_ResourceSO")]
public class DB_ResourceSO : ScriptableObject
{
    [Space, Header("PassLevel")]
    public DataResource dataResBaseLevel;
    public DataResource dataResUpgradeLevel;

    [Space , Header("Position In Level")]
    public DataResource dataResPos;
    public DataResource dataResPosUpgrade;
    public DataResource dataResPosUpgradeLevel;


    public DataResource GetDataResourceBaseLevel(int level)
    {
        DataResource dataRes = new DataResource();
        dataRes.type = dataResBaseLevel.type;
        dataRes.amount = dataResBaseLevel.amount + dataResUpgradeLevel.amount*level;
        return dataRes;
    }

    public DataResource GetDataResourcePosition(int pos, int level)
    {
        DataResource dataRes = new DataResource();
        dataRes.type = dataResPos.type;
        dataRes.amount = dataResPos.amount + dataResPosUpgrade.amount*pos + dataResUpgradeLevel.amount * level;
        return dataRes;
    }
}
