using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectChangeGameObject : BaseSelect
{
    public GameObject[] obSelect; 
    public GameObject[] obUnSelect;

    public override void Select(bool isSelect)
    {
        int length = obSelect.Length;
        for (int i = 0; i < length; i++)
        {
            obSelect[i].SetActive(isSelect);
            obUnSelect[i].SetActive(!isSelect);
        }

    }
}
