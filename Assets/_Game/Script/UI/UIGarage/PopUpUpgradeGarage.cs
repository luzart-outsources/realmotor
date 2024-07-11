using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpUpgradeGarage : MonoBehaviour
{
    public TMP_Text txtName;
    public TMP_Text txtNameModel;
    public TMP_Text txtRank;
    public ItemUpgradeGarageRef[] itemUpgradeGarageRefs;
    public RectTransform rectTransformLineRef;
    private DB_Motor DB_Motor;
    private float widthLine;
    private void Awake()
    {
        widthLine = rectTransformLineRef.rect.width;
    }
    public void SelectDB(DB_Motor db, bool[] isMaxData)
    {
        DB_Motor = db;
        txtName.text = db.nameMotor;
        txtNameModel.text = db.nameModelMotor;
        txtRank.text = $"Rank-{db.rank}";
        ShowData(itemUpgradeGarageRefs[0], "Top Speed", db.inforMotorbike.maxSpeed, db.inforUpgrade.maxSpeed, isMaxData[0]);
        ShowData(itemUpgradeGarageRefs[1], "Acceleration", db.inforMotorbike.acceleration, db.inforUpgrade.acceleration, isMaxData[1]);
        ShowData(itemUpgradeGarageRefs[2], "Handling", db.inforMotorbike.handling, db.inforUpgrade.handling, isMaxData[2]);
        ShowData(itemUpgradeGarageRefs[3], "Brake", db.inforMotorbike.brake, db.inforUpgrade.brake, isMaxData[3]);
    }
    private void ShowData(ItemUpgradeGarageRef itemUpgrade, string title, float curData, float upGradeData,bool isMaxData)
    {
        itemUpgrade.txtTitle.text = title;
        itemUpgrade.txtCurValue.text = curData.ToString();
        SetLine(itemUpgrade.rtCurLine, curData,itemUpgrade.maxRange);

        if (isMaxData)
        {
            itemUpgrade.txtUpgrade.text = "";
            SetLine(itemUpgrade.rtUpgrade, 0, itemUpgrade.maxRange);
        }
        else
        {
            itemUpgrade.txtUpgrade.text = upGradeData.ToString();
            SetLine(itemUpgrade.rtUpgrade, upGradeData, itemUpgrade.maxRange);
        }
        itemUpgrade.obUp.SetActive(!isMaxData);
    }
    private void SetLine(RectTransform line, float curValue, float maxValue)
    {
        float delta = curValue/maxValue;
        float valueLine = widthLine * delta;
        Vector2 size = line.sizeDelta;
        line.sizeDelta = new Vector2(valueLine, size.y);
    }

}
[System.Serializable]
public class ItemUpgradeGarageRef
{
    public TMP_Text txtTitle;
    public TMP_Text txtCurValue;
    public GameObject obUp;
    public TMP_Text txtUpgrade;
    public RectTransform rtCurLine;
    public RectTransform rtUpgrade;
    public int maxRange;
}
