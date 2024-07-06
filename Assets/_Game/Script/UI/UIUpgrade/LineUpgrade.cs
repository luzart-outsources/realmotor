using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpgrade : MonoBehaviour
{
    public GameObject obBlackLine;
    public GameObject obGreenLine;

    public void SetActiveLine(bool active)
    {
        obGreenLine.SetActive(active);
    }
}
