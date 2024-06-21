using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DB_CharacterSO", fileName = "DB_CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public HelmetPath[] helmetPaths;
    public ClothesPath[] clothesPaths;

    public const string PATH_HELMET = "Helmet";
    public const string PATH_CLOTHES = "Clothes";

    public DB_CharacterBot[] dB_CharacterBots;

    public DB_Character GetDB_CharacterBot(int index)
    {
        for (int i = 0; i < dB_CharacterBots.Length; i++)
        {
            if(index == dB_CharacterBots[i].id)
            {
                return dB_CharacterBots[i].db_Character;
            }
        }
        return null;
    }

    public string GetPathHelmet(int id)
    {
        return $"{PATH_HELMET}/{GetPathHelmetRaw(id)}";
    }
    private string GetPathHelmetRaw(int id)
    {
        for (int i = 0; i < helmetPaths.Length; i++)
        {
            if (helmetPaths[i].id == id)
            {
                return helmetPaths[i].path;
            }
        }
        return "";
    }
    public string GetPathClothes(int id)
    {
        return $"{PATH_CLOTHES}/{GetPathClothesRaw(id)}";
    }
    private string GetPathClothesRaw(int id)
    {
        for (int i = 0; i < clothesPaths.Length; i++)
        {
            if (clothesPaths[i].id == id)
            {
                return clothesPaths[i].path;
            }
        }
        return "";
    }


}
[System.Serializable]
public class DB_CharacterBot
{
    public int id;
    public DB_Character db_Character;
}

[System.Serializable]
public class DB_Character
{
    public int idHelmet;
    public int idClothes;

    public string pathHelmet
    {
        get
        {
            return DataManager.Instance.characterSO.GetPathHelmet(idHelmet);
        }
    }
    public string pathClothes
    {
        get
        {
            return DataManager.Instance.characterSO.GetPathClothes(idClothes);
        }
    }
}
[System.Serializable]
public struct HelmetPath
{
    public int id;
    public string path;
}
[System.Serializable]
public struct ClothesPath
{
    public int id;
    public string path;
}
