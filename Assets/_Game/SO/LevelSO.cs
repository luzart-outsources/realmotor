using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/LevelSO", fileName ="LevelSO")]
public class LevelSO : ScriptableObject
{
    [SerializeField]
    private DB_Level[] db_Levels;

    private Dictionary<int, DB_Level> dictLevel = new Dictionary<int, DB_Level>();
    private Dictionary<ThemeLevel, List<DB_Level>> dictThemeLevel = new Dictionary<ThemeLevel, List<DB_Level>>();
    public DB_Level GetDB_Level(int level)
    {
        InitDBLevel();
        if (dictLevel.ContainsKey(level))
        {
            return dictLevel[level];
        }
        return null;
    }
    private void InitDBLevel()
    {
        if(dictLevel != null && dictLevel.Count > 0)
        {
            return;
        }
        dictLevel.Clear();
        for (int i = 0; i < db_Levels.Length; i++)
        {
            var db = db_Levels[i];
            dictLevel.Add(db.level, db);
        }
    }
    private void InitThemeLevel()
    {
        if(dictThemeLevel != null && dictThemeLevel.Count > 0)
        {
            return;
        }
        dictThemeLevel.Clear();
        for(int i = 0;i < db_Levels.Length; i++)
        {
            var db = db_Levels[i];
            if (!dictThemeLevel.ContainsKey(db.themeLevel))
            {
                dictThemeLevel.Add(db.themeLevel, new List<DB_Level>());
            }
            dictThemeLevel[db.themeLevel].Add(db);
        }
    }
    public List<DB_Level> GetAllDBThemeLevel(ThemeLevel themeLevel)
    {
        InitThemeLevel();
        return dictThemeLevel[themeLevel];
    }
    public EnvironmentSO environmentSO;
    public MotorbikeSO motorbikeSO;
    
    public DB_Level GetDBLevelThemeLevel(int level)
    {
        InitDBLevel();
        if (!dictLevel.ContainsKey(level))
        {
            return null;
        }
        DB_Level dB_Level = dictLevel[level];
        return dB_Level;
    }
    public int GetIndexThemeLevel(int level)
    {
        DB_Level db = GetDBLevelThemeLevel(level);
        if (db == null)
        {
            int[] arrrayTheme = GameUtil.GetArrayThemeLevel();
            return arrrayTheme.Length - 1;
        }
        else
        {
            return (int)db.themeLevel;
        }


        
    }

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    private void ResetLevel()
    {
        List<DB_Level> list = new List<DB_Level>();
        for (int i = 0; i < 45; i++)
        {
            DB_Level level = new DB_Level();
            level.level = i;
            level.idEnvironment = i % (environmentSO.allEnvironment.Length - 1);
            level.lapRequire = i / 10 + 1;
            int randomLevel = Random.Range(6, 8);
            level.themeLevel = (ThemeLevel)(i / randomLevel);
            int num = Random.Range(8, 10);
            level.idBot = new int[num];
            int maxMotor = motorbikeSO.db_Bots.Length;
            for (int j = 0; j < num; j++)
            {
                int Min = Mathf.Clamp(i * 2 - 4, 0, maxMotor);
                int Max = Mathf.Clamp(i * 2 + 4, 0, maxMotor);
                level.idBot[j] = Random.Range(Min, Max);
            }
            level.indexStart = Random.Range(0, num + 2);
            level.indexStartBot = new int[num];
            int iSB = 0;
            for (int j = 0; j < num; j++)
            {
                if (j == level.indexStart)
                {
                    iSB++;
                }
                level.indexStartBot[j] = iSB;
                iSB++;
            }
            list.Add(level);
        }
        db_Levels = list.ToArray();
        SetAllNameIcon();
    }
    [Sirenix.OdinInspector.Button]
    public void SetAllNameIcon()
    {
        List<Sprite> list = LoadFifthImage();
        int length = db_Levels.Length;
        for (int i = 0; i < length; i++)
        {
            var item = db_Levels[i];
            item.spIcon = list[item.idEnvironment];
            item.name = list[item.idEnvironment].name;
        }
    }
    public List<Sprite> LoadFifthImage()
    {
        List<Sprite> list = new List<Sprite>();
        string folderPath = "Assets/_Game/Art/MapDemo";
        string[] fileEntries = System.IO.Directory.GetFiles(folderPath);
        string[] imageFiles = System.Array.FindAll(fileEntries, IsImageFile);
        int length = imageFiles.Length;
        for (int i = 0; i < length; i++)
        {
            Sprite texture = AssetDatabase.LoadAssetAtPath<Sprite>(imageFiles[i]);
            list.Add(texture);
        }
        return list;
    }
#endif
    private static bool IsImageFile(string filePath)
    {
        string extension = System.IO.Path.GetExtension(filePath).ToLower();
        return extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".tga";
    }
}

[System.Serializable]
public class DB_Level
{
    public int level;
    public int idEnvironment;
    public int lapRequire = 1;
    public ThemeLevel themeLevel;
    public Sprite spIcon;
    public string name;
    [Header("BOT")]
    public int[] idBot;
    [Header("StartIndex")]
    public int indexStart = 5;
    public int[] indexStartBot;
}
[System.Serializable]
public enum ThemeLevel
{
    Amateur = 0,
    SemiPro = 1,
    Pro =2,
    Expert = 3,
    Master = 4,
    WorldClass =5,
    Legend =6,
}
