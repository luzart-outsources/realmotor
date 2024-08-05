using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemWinDashboardUI : ItemLeaderboard
{
    public TMP_Text txtIndex;
    public TMP_Text txtName;
    public TMP_Text txtTime;
    public TMP_Text txtTimeAll;
    public TMP_Text txtNameModel;
    public GameObject obRed;

    public void InitData(bool isMe, DataItemWinLeaderboardUI data)
    {
        SetText(txtIndex, data.index);
        SetText(txtName, data.name);
        SetText(txtTime, data.time);
        SetText(txtTimeAll, data.timeAll);
        SetText(txtNameModel, data.nameModel);
        obRed.SetActive(isMe);
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
    public string timeAll;
    public string nameModel;
}
