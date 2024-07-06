using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="ResourcesSO", menuName = "Config/ResourcesSO")]
public class ResourcesSO : ScriptableObject
{
    public DB_ResourcesBuy[] dbResBuyBike;
    public DB_ResourcesBuy[] dbResBuyHelmet;
    public DB_ResourcesBuy[] dbResBuyBody;

    public DB_ResourcesBuy GetResourcesBuy(DataTypeResource dataTypeResource, PlaceBuy place)
    {
        int length = dbResBuyBike.Length;
        for (int i = 0; i < length; i++)
        {
            var data = dbResBuyBike[i];
            if (dataTypeResource.Compare(data.dataRes.type) && data.placeBuy == place)
            {
                return data;
            }
        }
        return null;
    }
#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    private void SetDBResBuyBike()
    {
        List<DB_ResourcesBuy> list= new List<DB_ResourcesBuy> ();
        int length = 14;
        for (int i = 0; i < length; i++)
        {
            DB_ResourcesBuy newA = new DB_ResourcesBuy();
            var data = new DataResource();
            data.amount = 1;
            data.type = new DataTypeResource(RES_type.Bike, i);
            newA.dataRes = data;
            newA.placeBuy = PlaceBuy.Garage;
            newA.valueBuy = 500 + 500*i;
            newA.typeBuy = TypeBuy.Gold;
            list.Add(newA);
        }
        dbResBuyBike = list.ToArray();
    }
    [Sirenix.OdinInspector.Button]
    private void SetDBResBuyHelmet()
    {
        List<DB_ResourcesBuy> list = new List<DB_ResourcesBuy>();
        int length = 6;
        for (int i = 0; i < length; i++)
        {
            DB_ResourcesBuy newA = new DB_ResourcesBuy();
            var data = new DataResource();
            data.amount = 1;
            data.type = new DataTypeResource(RES_type.Helmet, i);
            newA.dataRes = data;
            newA.placeBuy = PlaceBuy.Garage;
            newA.valueBuy = 1000;
            newA.typeBuy = TypeBuy.Gold;
            list.Add(newA);
        }
        dbResBuyHelmet = list.ToArray();
    }
    [Sirenix.OdinInspector.Button]
    private void SetDBResBuyBody()
    {
        List<DB_ResourcesBuy> list = new List<DB_ResourcesBuy>();
        int length = 6;
        for (int i = 0; i < length; i++)
        {
            DB_ResourcesBuy newA = new DB_ResourcesBuy();
            var data = new DataResource();
            data.amount = 1;
            data.type = new DataTypeResource(RES_type.Body, i);
            newA.dataRes = data;
            newA.placeBuy = PlaceBuy.Garage;
            newA.valueBuy = 1000;
            newA.typeBuy = TypeBuy.Gold;
            list.Add(newA);
        }
        dbResBuyBody = list.ToArray();
    }
#endif
}
[System.Serializable]
public class DB_ResourcesBuy
{
    public string titleBuy;
    public DataResource dataRes;
    public PlaceBuy placeBuy;
    public TypeBuy typeBuy;
    public int valueBuy;
}
public enum PlaceBuy
{
    None = 0,
    Garage = 1,
}
public enum TypeBuy
{
    None = 0,
    Gold = 1,
    Ads =2,
    IAP = 3,
    Other = 4,
}
