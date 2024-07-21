using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectChangeGameObject : BaseSelect
{
    public GameObject[] obSelect; 
    public GameObject[] obUnSelect;

    public override void Select(bool isSelect)
    {
        int lengthSelect = obSelect.Length;
        int lengthUnSelect = obUnSelect.Length;
        int length = Mathf.Max(lengthSelect, lengthUnSelect);
        for (int i = 0; i < length; i++)
        {
            int index = i;
            if(index < lengthSelect)
            {
                SetActiveObject(obSelect[index], isSelect);
            }
            if(index < lengthUnSelect)
            {
                SetActiveObject(obUnSelect[index], !isSelect);
            }
        }

    }
    private void SetActiveObject(GameObject ob, bool status)
    {
        if(ob != null)
        {
            ob.SetActive(status);
        }

    }
}
