using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnvironmentMap : MonoBehaviour
{

    public WavingPointGizmos wavingPointGizmos;
    public Transform[] startPoint;
    public Transform parentMotorbike;

    public static Action<EnvironmentMap> actionMap;
    public void Start()
    {
        actionMap?.Invoke(this);
    }
    public Transform GetStartPoint(int index)
    {
        return startPoint[index];
    }

#if UNITY_EDITOR
    [Space, Header("Editor")]
    public Transform startPointEditor;
    [Sirenix.OdinInspector.Button]
    public void GetAllStartPoint()
    {
        startPoint = startPointEditor.GetComponentsInChildren<Transform>();
        List<Transform> listWave = new List<Transform>();
        for (int i = 0; i < startPoint.Length; i++)
        {
            if (!startPoint[i].name.Contains("StartPoint"))
            {
                continue;
            }
            listWave.Add(startPoint[i]);
        }
        startPoint = listWave.ToArray();
    }
    [Sirenix.OdinInspector.Button]
    public void SetUpMap()
    {
        SetAllColliderAndLayer();
        FindWavePoint();
        FindStartRace();
        FindParentMotorbike();

    }
    public void FindWavePoint()
    {
        GameObject ob = GameObject.Find("W-P-C");
        wavingPointGizmos = ob.GetComponent<WavingPointGizmos>();
        if (wavingPointGizmos == null)
        {
            wavingPointGizmos = ob.AddComponent<WavingPointGizmos>();
        }
        wavingPointGizmos.AddWavingPointChildren();
        wavingPointGizmos.GetAllWavePointEditor();
    }
    public void FindStartRace()
    {
        GameObject startRace = GameObject.Find("StartRace");
        if (startRace == null)
        {
            string prefabPath = "Assets/_Game/Prefabs/ToolMap/StartEndRace.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            PrefabUtility.InstantiatePrefab(prefab, transform);
        }
        startRace = GameObject.Find("StartRace");
        startPointEditor = startRace.transform;
        GetAllStartPoint();
    }
    public void FindParentMotorbike()
    {
        GameObject obParentMotorbike = GameObject.Find("ParentMotorbike");
        if (obParentMotorbike == null)
        {
            obParentMotorbike = new GameObject("ParentMotorbike");
            obParentMotorbike.transform.parent = transform;
            obParentMotorbike.transform.localPosition = Vector3.zero;
        }
        parentMotorbike = obParentMotorbike.transform;
    }
    private void SetAllColliderAndLayer()
    {
        LayerMask layerWall = LayerMask.NameToLayer("Wall");
        LayerMask layerGround = LayerMask.NameToLayer("Ground");
        LayerMask layerRoad = LayerMask.NameToLayer("Road");

        List<GameObject> obFences = FindGameObjectsByName("Collider-Fences");
        foreach (GameObject item in obFences)
        {
            item.layer = layerWall;
        }

        List<GameObject> obGrass = FindGameObjectsByName("Collider-Grass");
        foreach (GameObject item in obGrass)
        {
            item.layer = layerGround;
        }

        List<GameObject> obRoad = FindGameObjectsByName("Collider-Road");
        foreach (GameObject item in obRoad)
        {
            item.layer = layerRoad;
        }
    }
    public List<GameObject> FindGameObjectsByName(string name)
    {
        // Tạo danh sách để lưu các GameObject tìm được
        List<GameObject> objectsWithName = new List<GameObject>();

        // Lấy tất cả các GameObject trong scene
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        // Kiểm tra từng GameObject xem có tên là "collidd" không
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name)
            {
                objectsWithName.Add(obj);
            }
        }

        return objectsWithName;
    }
#endif
}
