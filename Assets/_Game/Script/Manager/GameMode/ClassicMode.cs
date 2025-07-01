namespace Luzart
{
    using BG_Library.NET;
    using DynamicShadowProjector.Sample;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    public class ClassicMode : BaseMode
    {
        private UIGameplay uIGameplay;
        private DB_Level db_Level;
        private int currentLevel;
        private bool isInit { get; set; } = false;
    
        public override void StartLevel(int level)
        {
            base.StartLevel(level);
            InitData(level);
            UIManager.Instance.LoadScene(() =>
            {
                gameCoordinator.InitStartGame(db_Level);
            }, () =>
            {
                gameCoordinator.OnStartVisualInGame();
            });
            gameCoordinator.ActionOnEndGame = OnEndGame;
        }
        private void InitData(int level)
        {
            currentLevel = level;
            db_Level = DataManager.Instance.levelSO.GetDB_Level(currentLevel);
        }
        private bool isWin;
        public override void OnEndGame(bool isWin)
        {
            if (GameManager.Instance.EGameStatus == EGameState.None)
            {
                return;
            }
            base.OnEndGame(isWin);
            this.isWin = isWin;
            GameManager.Instance.gameCoordinator.EndGameData(isWin);
            if(GameManager.Instance.gameCoordinator.myMotorbike.eState == EStateMotorbike.Finish)
            {
                ReceiveRewardWin(isWin);
            }
    
        }
        private void ReceiveRewardWin(bool isWin)
        {
            OnDataWin();
            OnReceiveRes();
            ShowPopUp(isWin);
        }
        private void OnDataWin()
        {
            int count = GameManager.Instance.gameCoordinator.countLeaderBoard;
            int length = GameManager.Instance.gameCoordinator.listLeaderBoard.Count;
            int valueCountLeaderBoard = length - count;
            int level = db_Level.level;
            int goldPassLevel = DataManager.Instance.dB_ResourceSO.GetDataResourcePosition(count, level).amount;
            dataValueWin = new DataValueWin();
            dataValueWin.valuePos = Mathf.RoundToInt(goldPassLevel * 0.1f);
            dataValueWin.valueLevel = Mathf.RoundToInt(goldPassLevel * 0.1f);
            dataValueWin.valueJoin = Mathf.RoundToInt(goldPassLevel * 0.3f);
            dataValueWin.valueResult = Mathf.RoundToInt(goldPassLevel * 0.5f);
        }
        private void OnReceiveRes()
        {
            DataResource dataResource = new DataResource();
            dataResource.type = new DataTypeResource(RES_type.Gold);
            dataResource.amount = dataValueWin.valueLevel + dataValueWin.valuePos;
            DataManager.Instance.ReceiveRes(dataResource);
        }
        protected override void OnWinGame()
        {
            base.OnWinGame();
            if(DataManager.Instance.CurrentLevel <= db_Level.level)
            {
                DataManager.Instance.GameData.level++;
                DataManager.Instance.SaveGameData();
            }
    
        }
        private DataValueWin dataValueWin;
        private void ShowPopUp(bool isWin)
        {
            UIManager.Instance.HideUiActive(UIName.Gameplay);
            var ui = UIManager.Instance.ShowUI<UIWinClassic>(UIName.WinClassic);
    
            if(ui != null)
            {
                ui.InitLevel(currentLevel);
                ui.InitDataRes(isWin, dataValueWin);
                ui.InitDataDashboard(GameManager.Instance.gameCoordinator.listDataItemWinLeaderBoard);
                ui.OnSequenceVisual();
                ui.OnShowCompleteAnim = CheckShowPopUpRate;
            }
        }
        private void CheckShowPopUpRate()
        {
            if(isWin && IsShowRate(DataManager.Instance.CurrentLevel-1) && IsShowRate(currentLevel))
            {
                UIManager.Instance.ShowUI(UIName.Rate);
            }
    
    
    
            bool IsShowRate(int data)
            {
                return
                    data == 4 ||
                    data == 10 ||
                    data == 14 ||
                    data == 19;
            }
        }
        private void OnDisable()
        {
            if(gameCoordinator!= null)
            gameCoordinator.DestroyAllBike();
        }
    }
}
