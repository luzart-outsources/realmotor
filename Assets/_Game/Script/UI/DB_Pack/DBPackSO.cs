using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DB_PackSO", menuName = "SO/DB_PackSO")]
public class DBPackSO : ScriptableObject
{
    public DB_Pack[] dbPack;

    private Dictionary<string, DB_Pack> dictDBPack = new Dictionary<string, DB_Pack>();

    private void InitDictDBPack()
    {
        if (dictDBPack != null && dictDBPack.Count > 0)
        {
            return;
        }
        int length = dbPack.Length;
        for (int i = 0; i < length; i++)
        {
            var db = dbPack[i];
            dictDBPack.Add(db.productId, db);
        }
    }

    public DB_Pack GetDBPack(string productId)
    {
        InitDictDBPack();
        if (IsHasDBPack(productId))
        {
            return dictDBPack[productId];
        }
        else
        {
            return null;
        }
    }
    public bool IsHasDBPack(string productId)
    {
        return dictDBPack.ContainsKey(productId);
    }
}
