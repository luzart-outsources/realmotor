using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/LevelSO", fileName ="LevelSO")]
public class LevelSO : ScriptableObject
{
    [SerializeField]
    private DB_Level[] db_Levels;

    private Dictionary<int, DB_Level> dictLevel = new Dictionary<int, DB_Level>();
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
    private void Reset()
    {
        List<DB_Level> list = new List<DB_Level>();
        for (int i = 0; i < 20; i++)
        {
            DB_Level level = new DB_Level();
            level.level = i;
            level.idEnvironment = i % 3;
            level.lapRequire = i / 5 + 1;
            int num = Random.Range(3, 7);
            level.idBot = new int[num];
            for (int j = 0; j < num; j++)
            {
                level.idBot[j] = Random.Range(0, i*2+4);
            }
            level.indexStart = Random.Range(0, num+2);
            level.indexStartBot = new int[num];
            int iSB = 0;
            for(int j = 0;j < num; j++)
            {
                if(j == level.indexStart)
                {
                    iSB++;
                }
                level.indexStartBot[j] = iSB;
                iSB++;
            }
            list.Add(level);
        }
        db_Levels = list.ToArray();
    }
}

[System.Serializable]
public class DB_Level
{
    public int level;
    public int idEnvironment;
    public int lapRequire = 1;

    public int[] idBot;

    [Header("StartIndex")]
    public int indexStart = 5;
    public int[] indexStartBot;
}
