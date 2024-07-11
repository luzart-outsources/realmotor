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
        if (dataTypeResource.type == RES_type.Gold)
        {
            Observer.Instance.Notify(ObserverKey.CoinObserverNormal);
        }
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
    public DataResource()
    {

    }
    public DataResource(DataTypeResource type,  int amount)
    {
        this.type = type;
        this.amount = amount;
    }
    public DataTypeResource type;
    public int amount;
    [JsonIgnore]
    public int idIcon = 0;
    [JsonIgnore]
    public int idBg = 0;
    [JsonIgnore]
    public Sprite spIcon;
    [JsonIgnore]
    public Sprite spBg;
    public DataResource Clone()
    {
        return new DataResource(this.type, this.amount);
    }
}
[System.Serializable]
public struct DataTypeResource
{
    public DataTypeResource(RES_type type, int id = 0)
    {
        this.type = type;
        this.id = id;
    }
    public RES_type type;
    public int id;
    public bool Compare(DataTypeResource dataOther)
    {
        if(type == dataOther.type &&  id == dataOther.id)
        {
            return true;
        }
        return false;
    }
}
public enum RES_type
{
    None = 0,
    Gold = 1,
    Bike =2,
    Helmet = 3,
    Body = 4,
}
