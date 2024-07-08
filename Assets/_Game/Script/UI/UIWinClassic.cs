using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWinClassic : UIBase
{
    public Button btnClaim;
    public TMP_Text txt_Title;
    public TMP_Text txtValuePos;
    public TMP_Text txtValueLevel;
    public TMP_Text txtTotal;
    private DataValueWin dataWin;

    protected override void Setup()
    {
        base.Setup();
        GameUtil.ButtonOnClick(btnClaim, ClickClaim, true);
    }
    public void InitDataRes(bool isWin, DataValueWin db)
    {
        this.dataWin = db;
        if (isWin )
        {
            txt_Title.text = "Success";
        }
        else
        {
            txt_Title.text = "Failed";
        }
        txtValuePos.text = $"{dataWin.valuePos}";
        txtValueLevel.text = $"{dataWin.valueLevel}";
        txtTotal.text = $"{dataWin.valuePos + dataWin.valueLevel}";
    }
    private void ClickClaim()
    {
        UIManager.Instance.HideAll();
        UIManager.Instance.ShowGarage();
    }
}
[System.Serializable]
public class DataValueWin
{
    public int valuePos;
    public int valueLevel;
}
