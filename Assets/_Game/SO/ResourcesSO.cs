using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="ResourcesSO", menuName = "Config/ResourcesSO")]
public class ResourcesSO : ScriptableObject
{
    public DB_ResourcesBuy[] dbResBuyBike;
    public DB_ResourcesBuy[] dbResBuyHelmet;
    public DB_ResourcesBuy[] dbResBuyBody;

    public DB_ResourcesUpgradeMotor[] dbResUpgradeMotor;

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
    public DB_ResourcesBuy GetResourcesBuyUpgradeMotor(int idMotor, int indexStats, int level)
    {
        for (int i = 0;i < dbResUpgradeMotor.Length;i++)
        {
            var dbResMotor = dbResUpgradeMotor[i];
            if(idMotor == dbResMotor.idMotor)
            {
                for (int j = 0; j < dbResMotor.dB_ResourcesUpgradeMotorLevel.Length; j++)
                {
                    if(indexStats == j)
                    {
                        var dbResLevel = dbResMotor.dB_ResourcesUpgradeMotorLevel[j];
                        for (int k = 0; k < dbResLevel.dB_ResourcesBuys.Length; k++)
                        {
                            if (k == level)
                            {
                                return dbResLevel.dB_ResourcesBuys[k];
                            }

                        }
                    }

                }
            }
        }
        return null;
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    private void SetDBResBuyBike()
    {
        List<DB_ResourcesBuy> list = new List<DB_ResourcesBuy>();
        int length = 14;
        for (int i = 0; i < length; i++)
        {
            DB_ResourcesBuy newA = new DB_ResourcesBuy();
            var data = new DataResource();
            data.amount = 1;
            data.type = new DataTypeResource(RES_type.Bike, i);
            newA.dataRes = data;
            newA.placeBuy = PlaceBuy.Garage;
            newA.valueBuy = 500 + 500 * i;
            newA.typeBuy = TypeBuy.Gold;
            list.Add(newA);
        }
        dbResBuyBike = list.ToArray();
    }
    [Sirenix.OdinInspector.Button]
    private void SetDBResUpgradeButton()
    {
        List<DB_ResourcesUpgradeMotor> list = new List<DB_ResourcesUpgradeMotor>();
        int length = 14;
        int thongSoXe = 4;
        int maxLevel = 5;
        for (int i = 0; i < length; i++)
        {
            DB_ResourcesUpgradeMotor newA = new DB_ResourcesUpgradeMotor();
            newA.idMotor = i;
            newA.dB_ResourcesUpgradeMotorLevel = new DB_ResourcesUpgradeMotorLevel[thongSoXe];

            for (int j = 0; j < thongSoXe; j++)
            {
                newA.dB_ResourcesUpgradeMotorLevel[j] = new DB_ResourcesUpgradeMotorLevel();
                newA.dB_ResourcesUpgradeMotorLevel[j].dB_ResourcesBuys = new DB_ResourcesBuy[maxLevel];

                for (int k = 0; k < maxLevel; k++)
                {
                    var dataBuy = new DB_ResourcesBuy();
                    dataBuy.placeBuy = PlaceBuy.Upgrade;
                    dataBuy.valueBuy = 50 * i + 15 * k;
                    dataBuy.typeBuy = TypeBuy.Gold;
                    newA.dB_ResourcesUpgradeMotorLevel[j].dB_ResourcesBuys[k] = dataBuy;
                }
            }
            list.Add(newA);
        }
        dbResUpgradeMotor = list.ToArray();
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
    [JsonIgnore]
    [HideInInspector]
    public bool isHas;
}
public enum PlaceBuy
{
    None = 0,
    Garage = 1,
    Upgrade =2,
}
public enum TypeBuy
{
    None = 0,
    Gold = 1,
    Ads =2,
    IAP = 3,
    Other = 4,
}
[System.Serializable]
public class DB_ResourcesUpgradeMotor
{
    public int idMotor;
    public DB_ResourcesUpgradeMotorLevel[] dB_ResourcesUpgradeMotorLevel = new DB_ResourcesUpgradeMotorLevel[5];
}
[System.Serializable]
public class DB_ResourcesUpgradeMotorLevel
{
    public DB_ResourcesBuy[] dB_ResourcesBuys = new DB_ResourcesBuy[4];
}
