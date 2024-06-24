using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnWavingPoint : MonoBehaviour
{
    public Transform[] allWavePoint;
    public Transform[] allWavePointEditor;
    public Transform parentWaving;
    public Transform parentWavingPointSpawn;
    public Transform itemMove;
    public WavingPoint wavingPointPrefabs;
    [Space, Header("Start- End Pos")]
    public Transform startPos;
    public Transform endPos;

    //
    public int startIndex;
    [Button]
    private void GetStartIndex()
    {
        startIndex = GetNearestPoint(startPos);
    }

    //
    public int endIndex;
    [Button]
    private void GetEndIndex()
    {
        endIndex = GetNearestPoint(endPos);
    }

    public float velocity;
    [Range(-1,1)]
    public int direct;
    
    public bool IsStartMove = false;
    public bool IsDrawGizmos = false;
    

    //


    public int currentIndex;

    private int GetNearestPoint(Transform transMove)
    {
        float dis = 100000000f;
        int currentIndex = 0;
        for (int i = 0;i < allWavePoint.Length;i++)
        {
            var point = allWavePoint[i];
            float disPoint = Distance(transMove, point);
            if (disPoint < dis )
            {
                currentIndex = i;
                dis = disPoint;
            }
        }
        return currentIndex;
    }
    private float Distance(Transform transMove ,Transform t)
    {
        return Vector3.Distance(transMove.position, t.position);
    }
    public int draw = 0;
    public int drawSpawn = 100;
    private void MoveItem(Transform target)
    {
        itemMove.transform.position = itemMove.position + (target.position - itemMove.position).normalized  * velocity;
        itemMove.LookAt(target.position);
        UpdateOnFrame();
    }
    public int child;
    private void UpdateOnFrame()
    {
        draw++;
        if(draw % drawSpawn == 0)
        {
            PrefabUtility.InstantiatePrefab(wavingPointPrefabs, parentWavingPointSpawn);
            var wavingPoint = parentWavingPointSpawn.GetChild(child);
            if (wavingPoint != null)
            {
                wavingPoint.transform.position = itemMove.transform.position;
                wavingPoint.transform.rotation = itemMove.transform.rotation;
                child++;
            }
        }


    }


    private void OnDrawGizmos()
    {
        if (IsDrawGizmos)
        {
            DrawGizmos();
        }
        if (IsStartMove)
        {
            float dis = Distance(itemMove, allWavePoint[currentIndex]);
            if(dis <= 1f)
            {
                currentIndex += direct;
            }
            if (currentIndex == allWavePoint.Length)
            {
                currentIndex = 0;
            }
            if (currentIndex == -1)
            {
                currentIndex = allWavePoint.Length -1 ;
            }
            if (currentIndex == endIndex)
            {
                float disEnd = Distance(itemMove, allWavePoint[currentIndex]);
                if(disEnd <= 3f)
                {
                    IsStartMove = false;
                    return;
                }
            }
            MoveItem(allWavePoint[currentIndex]);
        }
    }

    [Button]
    private void StartMove()
    {
        IsStartMove = true;
        itemMove.transform.position = startPos.transform.position;
        var asd = parentWavingPointSpawn.GetComponentsInChildren<WavingPoint>();
        for (int i = 0; i < asd.Length; i++)
        {
            DestroyImmediate(asd[i].gameObject);
        }
        currentIndex = startIndex;
        currentIndex += direct;
        child = 0;
        draw = 0;
    }

    [Button]
    private void DrawGizmos()
    {
        allWavePointEditor = parentWaving.GetComponentsInChildren<Transform>();
        List<Transform> listWave = new List<Transform>();
        for (int i = 0; i < allWavePointEditor.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }
            listWave.Add(allWavePointEditor[i]);
        }
        allWavePoint = listWave.ToArray();
        for (int i = 0; i < listWave.Count; i++)
        {
            Gizmos.color = Color.yellow;
            int nextIndex = i + 1;
            if (i >= listWave.Count - 1)
            {
                nextIndex = 0;
            }
            Gizmos.DrawLine(listWave[i].position, listWave[nextIndex].position);
            listWave[i].LookAt(listWave[nextIndex].position);
        }
    }
}
