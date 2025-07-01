namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    
    public class ItemWinDashboardUI : ItemLeaderboard
    {
        public TMP_Text txtIndex;
        public TMP_Text txtName;
        public TMP_Text txtTime;
        public TMP_Text txtPR;
        public TMP_Text txtNameModel;
        public GroupBaseSelect GroupBaseSelect;
    
        public void InitData(bool isMe, DataItemWinLeaderboardUI data)
        {
            SetText(txtIndex, data.index);
            SetText(txtName, data.name);
            SetText(txtTime, data.time);
            SetText(txtPR, $"PR {data.PR}");
            SetText(txtNameModel, data.nameModel);
            GroupBaseSelect.Select(isMe);
        }
        private void SetText(TMP_Text txt, string str)
        {
            if(txt == null)
            {
                return;
            }
            txt.text = str;
        }
    }
    [System.Serializable]
    public class DataItemWinLeaderboardUI
    {
        public string index;
        public string name;
        public string time;
        public string PR;
        public string nameModel;
    }
}
