namespace Luzart
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class DataManager : Singleton<DataManager>
    {
        [Space, Header("BaseGameLuzart")]
        public GameData _gameData;
        public GameData GameData => _gameData;
        public DBPackSO dbPackSO;
    
    
        [Space, Header("DB")]
        public LevelSO levelSO;
        public MotorbikeSO motorbikeSO;
        public MotorSO motorSO;
        public CharacterSO characterSO;
        public EnvironmentSO environmentSO;
        public DB_ResourceSO dB_ResourceSO;
        public ResourcesSO resourceBuySO;
    
        [Space, Header("SO Other")]
        public SpriteResourcesSO spriteResourceSO;
    
        [Space, Header("DailyRewardManager")]
        public DailyRewardManager dailyRewardManager;
    
        private const string KEY_GAME_DATA = "key_gamedata";
        public void Initialize()
        {
            LoadGameData();
            dailyRewardManager.InitData();
        }
        private void LoadGameData()
        {
            _gameData = SaveLoadUtil.LoadDataPrefs<GameData>(KEY_GAME_DATA);
            if (_gameData == null)
            {
                _gameData = new GameData();
            }
        }
        public void SaveGameData()
        {
            SaveLoadUtil.SaveDataPrefs<GameData>(_gameData, KEY_GAME_DATA);
        }
    
        public bool IsAddRes(DataResource dataResource)
        {
            return GameRes.isAddRes(dataResource);
        }
        public static int GetRes(DataTypeResource type)
        {
            return GameRes.GetRes(type);
        }
        public void AddRes(DataResource dataRes, Action onDone = null)
        {
            bool isAddRes = GameRes.isAddRes(dataRes);
            if (isAddRes)
            {
                AddRes(dataRes.type, dataRes.amount);
                onDone?.Invoke();
            }
            else
            {
                UIManager.Instance.ShowUI(UIName.AddCoin);
            }
    
    
        }
        private void AddRes(DataTypeResource type, int amount)
        {
            GameRes.AddRes(type, amount);
        }
        public void ReceiveRes(params DataResource[] dataResource)
        {
            int length = dataResource.Length;
            for (int i = 0; i < length; i++)
            {
                DataResource resource = dataResource[i];
                if (resource.type.type == RES_type.Bike)
                {
                    DB_Motorbike db = new DB_Motorbike(resource.type.id);
                    BuyMotorbike(db);
                }
                else if (resource.type.type == RES_type.Helmet)
                {
                    BuyHelmet(resource.type.id);
                }
                else if (resource.type.type == RES_type.Body)
                {
                    BuyBody(resource.type.id);
                }
                AddRes(resource);
    
                LogFirebase(resource);
    
            }
    
            SaveGameData();
    
            void LogFirebase(DataResource resource)
            {
                var dataRes = new ParameterFirebaseCustom(KeyTypeFirebase.Res, $"{resource.type.ToKeyString}");
                var dataAmount = new ParameterFirebaseCustom(KeyTypeFirebase.Amount, $"{resource.amount}");
                ParameterFirebaseCustom[] para = new ParameterFirebaseCustom[] { dataRes, dataAmount };
                FirebaseNotificationLog.LogWithLevelMax(KeyFirebase.ReceiveResource, para);
            }
        }
        public int Gold
        {
            get
            {
                DataTypeResource dataTypeResource = new DataTypeResource();
                dataTypeResource.type = RES_type.Gold;
                dataTypeResource.id = 0;
                return DataManager.GetRes(dataTypeResource);
            }
    
        }
        public int CurrentLevel => _gameData.level;
    
        public int GetAdsCurrentResource(DataTypeResource dataType)
        {
            DataGetByAds data = GetDataGetByAds(dataType);
            if (data != null)
            {
                return data.ads;
            }
            else
            {
                return 0;
            }
        }
        private DataGetByAds GetDataGetByAds(DataTypeResource dataType)
        {
            var list = GameData.getBuyAds;
            if (list == null || list.Count == 0)
            {
                return null;
            }
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i].type.Compare(dataType))
                {
                    return list[i];
                }
            }
            return null;
        }
        public void UpgradeMotor(int idMotor, int indexStats)
        {
            int length = GameData.motorbikeDatas.Count;
            for (int i = 0; i < length; i++)
            {
                var car = GameData.motorbikeDatas[i];
                if (car.idMotor == idMotor)
                {
                    car.levelUpgrades[indexStats]++;
                }
            }
            SaveGameData();
    
        }
        public void AddGetAdsResource(DataTypeResource dataType, int count)
        {
            DataGetByAds data = GetDataGetByAds(dataType);
            int value = 0;
            if (data != null)
            {
                value = data.ads;
            }
            value += count;
            if (data != null)
            {
                data.ads = value;
            }
            else
            {
                DataGetByAds dataNew = new DataGetByAds();
                dataNew.ads = value;
                dataNew.type = dataType;
                _gameData.getBuyAds.Add(dataNew);
            }
            SaveGameData();
    
        }
    
        public void BuyMotorbike(DB_Motorbike db)
        {
            List<DB_Motorbike> list = new List<DB_Motorbike>();
            if (_gameData.motorbikeDatas != null)
            {
                list = _gameData.motorbikeDatas;
            }
            bool isAdd = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].idMotor == db.idMotor)
                {
                    isAdd = false;
                    break;
                }
            }
            if (isAdd)
            {
                list.Add(db);
            }
            _gameData.motorbikeDatas = list;
        }
    
        public void BuyHelmet(int id)
        {
            List<int> list = new List<int>();
            if (_gameData.listHelmet != null)
            {
                list = _gameData.listHelmet;
            }
            if (!list.Contains(id))
            {
                list.Add(id);
            }
            _gameData.listHelmet = list;
            if (_gameData.curCharacter == null)
            {
                _gameData.curCharacter = new DB_Character();
            }
            _gameData.curCharacter.idHelmet = id;
            SaveGameData();
        }
    
        public void BuyBody(int id)
        {
            List<int> list = new List<int>();
            if (_gameData.listBody != null)
            {
                list = _gameData.listBody;
            }
            if (!list.Contains(id))
            {
                list.Add(id);
            }
            _gameData.listBody = list;
            if (_gameData.curCharacter == null)
            {
                _gameData.curCharacter = new DB_Character();
            }
            _gameData.curCharacter.idClothes = id;
            SaveGameData();
        }
    
        public bool IsHasMotor(int idMotor, ref int[] levelUpgrades)
        {
            levelUpgrades = new int[4];
            for (int i = 0; i < _gameData.motorbikeDatas.Count; i++)
            {
                var data = _gameData.motorbikeDatas[i];
                if (idMotor == data.idMotor)
                {
                    levelUpgrades = data.levelUpgrades;
                    return true;
                }
            }
            return false;
        }
        public bool IsHasMotor(int idMotor)
        {
            for (int i = 0; i < _gameData.motorbikeDatas.Count; i++)
            {
                var data = _gameData.motorbikeDatas[i];
                if (idMotor == data.idMotor)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsMaxData(int idMotor)
        {
            int[] levelUpgrades = new int[4];
            bool isHas = IsHasMotor(idMotor, ref levelUpgrades);
            bool IsMaxLevel = true;
            if (isHas)
            {
                for (int i = 0; i < levelUpgrades.Length; i++)
                {
                    if (levelUpgrades[i] < 5)
                    {
                        IsMaxLevel = false;
                    }
                }
            }
            return IsMaxLevel;
        }
        public bool[] IsMaxDataArray(int idMotor)
        {
            int[] levelUpgrades = new int[4];
            bool isHas = IsHasMotor(idMotor, ref levelUpgrades);
            bool[] array = new bool[levelUpgrades.Length];
            for (int i = 0; i < array.Length; i++)
            {
                if (levelUpgrades[i] >= 5)
                {
                    array[i] = true;
                }
            }
            return array;
        }
        public bool IsHasHelmet(int id)
        {
            if (_gameData.listHelmet == null || _gameData.listHelmet.Count == 0)
            {
                return false;
            }
            return _gameData.listHelmet.Contains(id);
        }
        public bool IsHasBody(int id)
        {
            if (_gameData.listBody == null || _gameData.listBody.Count == 0)
            {
                return false;
            }
            return _gameData.listBody.Contains(id);
        }
        public bool IsHasBuyPack(string productId)
        {
            return GameData.listPack.Exists(pack => pack.productId == productId);
        }
    
        public int GetPackPurchaseCount(string productId)
        {
            PackPurchaseData pack = GameData.listPack.Find(p => p.productId == productId);
            return pack != null ? pack.count : 0;
        }
    
        public void SaveBuyPack(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return;
            }
    
            // Kiểm tra xem gói đã tồn tại trong danh sách chưa
            PackPurchaseData existingPack = GameData.listPack.Find(pack => pack.productId == productId);
            if (existingPack != null)
            {
                existingPack.count++; // Tăng số lần mua nếu đã tồn tại
            }
            else
            {
                GameData.listPack.Add(new PackPurchaseData(productId, 1)); // Thêm gói mới với số lần mua là 1
            }
    
            SaveGameData();
        }
    }
    [System.Serializable]
    public class GameData
    {
        public string name = "Player";
        public int level = 0;
        public DB_Character curCharacter;
        public bool isUserIAP = false;
        public int idCurMotor;
        public List<DB_Motorbike> motorbikeDatas = new List<DB_Motorbike>();
        public List<DataGetByAds> getBuyAds = new List<DataGetByAds>();
        public List<int> listHelmet = new List<int>();
        public List<int> listBody = new List<int>();
        public DataDailyReward dailyReward = new DataDailyReward();
        public List<PackPurchaseData> listPack = new List<PackPurchaseData>();
        public bool isBeginnerBundle = false;
        public bool isFirstWatchRemoveAd = false;
        public bool isFirstWatchBundle = false;
    }
    [System.Serializable]
    public class PackPurchaseData
    {
        public string productId;
        public int count;
    
        public PackPurchaseData(string productId, int count)
        {
            this.productId = productId;
            this.count = count;
        }
    }
    [System.Serializable]
    public class InforMotorbike
    {
        public float maxSpeed = 100;
        public float acceleration = 20;
        public float handling = 30f;
        public float brake = 50f;
    
        public InforMotorbike Clone()
        {
            InforMotorbike infor = new InforMotorbike();
            infor.maxSpeed = maxSpeed;
            infor.acceleration = acceleration;
            infor.handling = handling;
            infor.brake = brake;
            return infor;
        }
        public int[] ToArray()
        {
            int[] array = new int[4];
            array[0] = (int)maxSpeed;
            array[1] = (int)acceleration;
            array[2] = (int)handling;
            array[3] = (int)brake;
            return array;
        }
        public float PR
        {
            get
            {
                return maxSpeed + acceleration + handling + brake;
            }
        }
    }
    [System.Serializable]
    public class DataGetByAds
    {
        public DataTypeResource type;
        public int ads;
    }
}
