using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRes 
{
    public static bool isAddRes(DataResource resource)
    {
        int amountCurrent = GetRes(resource.type);
        if(amountCurrent + resource.amount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static int GetRes(DataTypeResource dataTypeResource) 
    {
        string stringRes = getStringRes(dataTypeResource);
        return getRes(stringRes);
    }
    public static void AddRes(DataTypeResource dataTypeResource, int amount)
    {
        string stringRes = getStringRes(dataTypeResource);
        int preValue = getRes(stringRes);
        int targetValue = preValue + amount;
        PlayerPrefs.SetInt(stringRes, targetValue);
        Debug.Log($"To Add RES {stringRes} _ currentvalue {targetValue}");
        PlayerPrefs.Save();
    }
    private static string getStringRes(DataTypeResource dataTypeResource)
    {
        RES_type res = dataTypeResource.type;
        return $"{res}_{dataTypeResource.id}";
    }
    private static int getRes(string res)
    {
        return PlayerPrefs.GetInt(res, 0);
    }
}
[System.Serializable]
public class DataResource
{
    public DataTypeResource type;
    public int amount;
    [JsonIgnore]
    public int idIcon = 0;
    [JsonIgnore]
    public int idBg = 0;
}
[System.Serializable]
public struct DataTypeResource
{
    public RES_type type;
    public int id;
}
public enum RES_type
{
    None = 0,
    Gold = 1,
    Magnifier =2,
}
