using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameRes
{
    private static string playerResourcesKey = "PlayerResources";
    private static PlayerResources cachedPlayerResources = null;

    public static bool isAddRes(DataResource resource)
    {
        PlayerResources playerResources = GetCachedPlayerResources();
        int amountCurrent = playerResources.GetResourceAmount(resource.type);
        return amountCurrent + resource.amount > 0;
    }

    public static int GetRes(DataTypeResource dataTypeResource)
    {
        PlayerResources playerResources = GetCachedPlayerResources();
        return playerResources.GetResourceAmount(dataTypeResource);
    }

    public static void AddRes(DataTypeResource dataTypeResource, int amount)
    {
        PlayerResources playerResources = GetCachedPlayerResources();
        playerResources.AddResource(new DataResource(dataTypeResource, amount));
        SavePlayerResources(playerResources);
        Debug.Log($"To Add RES {dataTypeResource.type}_{dataTypeResource.id} _ currentvalue {amount}");

        if (dataTypeResource.type == RES_type.Gold)
        {
            Observer.Instance.Notify(ObserverKey.CoinObserverNormal);
        }
    }

    private static void SavePlayerResources(PlayerResources playerResources)
    {
        string json = JsonConvert.SerializeObject(playerResources);
        PlayerPrefs.SetString(playerResourcesKey, json);
        PlayerPrefs.Save();
        cachedPlayerResources = playerResources; // Cập nhật bộ nhớ cache
    }

    private static PlayerResources GetCachedPlayerResources()
    {
        if (cachedPlayerResources == null)
        {
            cachedPlayerResources = LoadPlayerResources();
        }
        return cachedPlayerResources;
    }

    private static PlayerResources LoadPlayerResources()
    {
        if (PlayerPrefs.HasKey(playerResourcesKey))
        {
            string json = PlayerPrefs.GetString(playerResourcesKey);
            return JsonConvert.DeserializeObject<PlayerResources>(json);
        }
        else
        {
            PlayerResources playerResources = BackupOldData();
            SavePlayerResources(playerResources);
            return playerResources;
        }
    }

    private static PlayerResources BackupOldData()
    {
        PlayerResources playerResources = new PlayerResources();
        foreach (RES_type resType in System.Enum.GetValues(typeof(RES_type)))
        {
            if (resType == RES_type.None)
                continue;

            // Giả định rằng mỗi loại tài nguyên có một ID duy nhất từ 0 đến n
            int id = 0;
            while (true)
            {
                string oldKey = GetOldKey(resType, id);
                if (PlayerPrefs.HasKey(oldKey))
                {
                    int amount = PlayerPrefs.GetInt(oldKey, 0);
                    if (amount != 0)
                    {
                        DataTypeResource dataTypeResource = new DataTypeResource(resType, id);
                        playerResources.AddResource(new DataResource(dataTypeResource, amount));
                    }
                    PlayerPrefs.DeleteKey(oldKey);
                }
                else
                {
                    break; // Không có key nào nữa, thoát khỏi vòng lặp
                }
                id++;
            }
        }
        return playerResources;
    }

    private static string GetOldKey(RES_type resType, int id)
    {
        return $"{resType}_{id}";
    }
}

[System.Serializable]
public class PlayerResources
{
    public List<DataResource> resources;

    public PlayerResources()
    {
        resources = new List<DataResource>();
    }

    public void AddResource(DataResource resource)
    {
        DataResource existingResource = resources.Find(r => r.type.Compare(resource.type));
        if (existingResource != null)
        {
            existingResource.amount += resource.amount;
        }
        else
        {
            resources.Add(resource);
        }
    }

    public int GetResourceAmount(DataTypeResource dataTypeResource)
    {
        DataResource resource = resources.Find(r => r.type.Compare(dataTypeResource));
        return resource != null ? resource.amount : 0;
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
    public string GetKeyString(RES_type resType, int id)
    {
        return $"{resType}_{id}";
    }
    public string ToKeyString
    {
        get
        {
            return $"{type}_{id}";
        }
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
