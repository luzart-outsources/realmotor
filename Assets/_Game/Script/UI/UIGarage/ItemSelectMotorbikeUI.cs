namespace Luzart
{
    using System;
    using System.Data.Common;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class ItemSelectMotorbikeUI : MonoBehaviour
    {
        private RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if(_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }
        public ResUI resUI;
        public TMP_Text txtName;
        public Button btn;
        public GameObject obSelect;
        public BaseSelect baseSelectBg;
        public BaseSelect obLock;
        public Action<ItemSelectMotorbikeUI> actionMotor;
    
        public DB_Motorbike db_Motorbike = null;
        public int currentIndex;
        private DB_ResourcesBuy resourcesBuy;
        private void Start()
        {
            GameUtil.ButtonOnClick(btn, ClickMotor, true);
        }
    
        public void InitDB(DB_Motorbike dbMotorbike, Action<ItemSelectMotorbikeUI> onClick)
        {
            this.db_Motorbike = dbMotorbike;
            this.actionMotor = onClick;
            SetUnLock(dbMotorbike.isHas);
            DB_Motor db_motor = DataManager.Instance.motorSO.GetDBMotor(dbMotorbike.idMotor);
            txtName.text = db_motor.nameMotor;
            resourcesBuy = DataManager.Instance.resourceBuySO.GetResourcesBuy(new DataTypeResource(RES_type.Bike,dbMotorbike.idMotor), PlaceBuy.Garage);
            resUI.InitData(resourcesBuy.dataRes);
            SelectMotorBike(false);
        }
        private void ClickMotor()
        {
            actionMotor?.Invoke(this);
        }
        public void SelectMotorBike(bool status)
        {
            obSelect.SetActive(status);
            if((!status && !db_Motorbike.isHas) || status)
            {
                if(status && db_Motorbike.isHas)
                {
                    baseSelectBg.Select(true);
                }
                else
                {
                    baseSelectBg.Select(false);
                }
    
                obLock.gameObject.SetActive(true);
            }
            else
            {
                obLock.gameObject.SetActive(false);
                baseSelectBg.Select(false);
            }
            
        }
        public void SetUnLock(bool isStatus)
        {
            if (obLock != null)
                obLock.Select(isStatus);
        }
    }
}
