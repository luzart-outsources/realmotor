namespace Luzart
{
    using Newtonsoft.Json;
    using Sirenix.OdinInspector;
    using System.Collections;
    using BG_Library.Common;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Text;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "SO/MotorbikeSO", fileName = "MotorbikeSO")]
    public class MotorbikeSO : ScriptableObject
    {
        public MotorSO motorSO;
        public CharacterSO characterSO;
        public DB_MotorbikeBot[] db_Bots;
    
        string[] racerNames = new string[]
    {
        "Rossi",
        "Marquez",
        "Lorenzo",
        "Miller",
        "Mir",
        "Binder",
        "Zarco",
        "Nakano",
        "Stoner",
        "Hayden",
        "Bulega",
        "Roberts",
        "Suzuki",
        "Garcia",
        "Luthi",
        "Pirro",
        "Masia",
        "Kallio",
        "Manzi",
        "Savadori",
        "Redding",
        "Gardner",
        "Elias",
        "Smith",
        "Ogura",
        "Toba",
        "Dixon",
        "Hawk",
        "Blitz",
        "Delgado",
        "Tanaka",
        "Castro",
        "Fujimoto",
        "Kostrov",
        "Ivanov",
        "Russo",
        "Ramos",
        "Storm",
        "Wolfe",
        "Bianchi",
        "Kato",
        "Laverty",
        "Baresi",
        "Marini",
        "Rabatt",
        "Simeon",
        "Canet",
        "Ogier",
        "Guintoli",
        "Fernandez",
        "Loren",
        "Sasaki",
        "Oncu",
        "Pasini",
        "Lowes",
        "Hopkins",
        "Gardner",
        "Tomas",
        "Pedro",
        "Luis",
        "Rinaldi",
        "Novak",
        "Nilsson",
        "Petrov",
        "Sergio",
        "Marco",
        "Andre",
        "Paolo",
        "Nicky",
        "Toni",
        "Yamada",
        "Simeon",
        "Savador",
        "Schrot",
        "Toba",
        "Miguel",
        "Hiroshi",
        "Tomas",
        "Karel",
        "Marco",
        "Mika",
        "Toni",
        "Enzo",
        "Carlos",
        "Luca",
        "Roman",
        "Roman",
        "Deniz",
        "Arenas",
        "Vietti",
        "Gomez",
        "Giovanni",
        "Lorenzo",
        "Gino",
        "Stiven",
        "Valenti",
        "Martin",
        "Kosta",
        "Mika"
    };
    
    
        [Button]
        private void ResetDB()
        {
            int count = 0;
            List<DB_MotorbikeBot> list = new List<DB_MotorbikeBot>();
            var random = new RandomNoRepeat<DB_CharacterBot>(characterSO.dB_CharacterBots);
            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < motorSO.db_MotorBots.Length; i++)
                {
                    DB_MotorbikeBot bot = new DB_MotorbikeBot();
                    bot.db_Motorbike = new DB_Motorbike();
                    bot.db_Motorbike.idMotor = i;
                    bot.db_Motorbike.levelUpgrade = j;
                    bot.id = count;
                    bot.name = $"BOT {Random.Range(i, 1000)}";
                    bot.idCharacterBot = random.Random().id;
                    list.Add(bot);
                    count++;
                }
            }
            db_Bots = list.ToArray();
    
        }
        public DB_MotorbikeBot GetMotorbikeBot(int id)
        {
            return db_Bots[id];
        }
        [Button]
        private void ResetNameDB()
        {
            var random = new RandomNoRepeat<string>(racerNames);
            int length = db_Bots.Length;
            for (int i = 0; i < length; i++)
            {
                var bot = db_Bots[i];
                bot.name = random.Random();
            }
        }
    }
    [System.Serializable]
    public class DB_MotorbikeBot
    {
        public int id;
        public string name;
        public int idCharacterBot;
        public DB_Motorbike db_Motorbike;
    }
    [System.Serializable]
    public class DB_Motorbike
    {
        public DB_Motorbike()
        {
    
        }
        public DB_Motorbike(int idMotor, int levelUpgrade = 0)
        {
            this.idMotor = idMotor;
            this.levelUpgrade = levelUpgrade;
        }
        public int idMotor;
        public int[] levelUpgrades = new int[] { 0, 0, 0, 0 };
        [JsonIgnore]
        public int levelUpgrade;
        [JsonIgnore]
        public bool isHas;
    }
    
}
