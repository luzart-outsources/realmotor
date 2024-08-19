using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InforRacing : MonoBehaviour
{
    public TMP_Text txtIndex;
    public TMP_Text txtName;

    public void InitName(string name)
    {
        txtName.text = name;
    }

    public void UpdateIndex(int index)
    {
        txtIndex.text = index.ToString();
    }
}
