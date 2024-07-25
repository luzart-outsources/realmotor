using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEndRace : MonoBehaviour
{
    public Transform[] startPoint;
    [Sirenix.OdinInspector.Button]
    public void GetAllStartPoint()
    {
        GetChildStartPoint();

    }
    public void GetRaycast(Transform item)
    {
        int layer = LayerMask.NameToLayer("Road");
        LayerMask layerRoad = 1 << layer;
        RaycastHit rayUp, rayDown;
        bool isRayUp = Physics.Raycast(item.position, Vector3.up, out rayUp, Mathf.Infinity, layerRoad);
        bool isRayDown = Physics.Raycast(item.position + 100 * Vector3.up, Vector3.down, out rayDown, Mathf.Infinity, layerRoad);
        if (isRayUp)
        {
            item.transform.position = rayUp.point;
        }
        else if (isRayDown)
        {
            item.transform.position = rayDown.point;
        }
    }
    private void GetChildStartPoint()
    {
        List<GameObject> listEndRace = GameUtil.FindGameObjectsByName("EndRace");
        startPoint = transform.GetComponentsInChildren<Transform>();
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
        for (int i = 0;i < startPoint.Length;i++)
        {
            GetRaycast(startPoint[i]);
        }
        for (int i = 0; i < listEndRace.Count; i++)
        {
            GetRaycast(listEndRace[i].transform);
        }
    }
}
