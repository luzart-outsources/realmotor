using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupBaseSelect : MonoBehaviour
{
    public BaseSelect[] groupBaseSelect;
    public void Select(bool isStatus)
    { 
        if(groupBaseSelect == null || groupBaseSelect.Length <= 0)
        {
            return;
        }

        int length = groupBaseSelect.Length;
        for (int i = 0; i < length; i++)
        {
            var item = groupBaseSelect[i];
            if (item != null)
            {
                item.Select(isStatus);
            }
        }
    }
}
