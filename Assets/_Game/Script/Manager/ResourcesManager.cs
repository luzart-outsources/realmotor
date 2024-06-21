using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

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
}
