using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourcesManager : Singleton<ResourcesManager>
{
    public MotorVisual LoadMotor(int id)
    {
        string path = DataManager.Instance.motorSO.GetPath(id);
        return Resources.Load<MotorVisual>(path);
    }

    public EnvironmentMap LoadMap(int id)
    {
        string path = DataManager.Instance.environmentSO.GetPath(id);
        return Resources.Load<EnvironmentMap>(path);
    }
    public void LoadMapScene(int id)
    {
        string path = DataManager.Instance.environmentSO.GetPathScene(id);
        SceneManager.LoadScene(path);
    }
    private const string nameSceneGarage = "Garage";
    private GarageManager garageManager = null;
    public bool LoadSceneGarage(Action<GarageManager> onDone)
    {
        GarageManager.ActionInitGarage =
            (garage) =>
            {
                garageManager = garage;
                onDone?.Invoke(garage);
            };
        Scene scene =  SceneManager.GetActiveScene();
        if (scene.name.Equals(nameSceneGarage))
        {
            onDone?.Invoke(garageManager);
            return false;
        }
        else
        {
            SceneManager.LoadScene(nameSceneGarage);
            return true;
        }


    }
    public Texture2D LoadHelmet(int id)
    {
        string path = DataManager.Instance.characterSO.GetPathHelmet(id);
        return Resources.Load<Texture2D>(path);
    }
    public Texture2D[] LoadBody(int id)
    {
        string path = DataManager.Instance.characterSO.GetPathClothes(id);
        return Resources.LoadAll<Texture2D>(path);
    }
}
