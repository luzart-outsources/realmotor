namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class LineUpgradeSlider : MonoBehaviour
    {
        public LineUpgrade[] allLines; 
        public void SetLevelUpgrade(int levelUpgrade)
        {
            SetArray(levelUpgrade);
        }
        private void SetArray(int index)
        {
            for (int i = 0; i < allLines.Length; i++)
            {
                bool isLight = false;
                if(i < index)
                {
                    isLight = true;
                }
                else
                {
                    isLight= false;
                }
                allLines[i].SetActiveLine(isLight);
            }
        }
    }
}
