using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMode : MonoBehaviour
{
    protected GameCoordinator gameCoordinator
    {
        get
        {
            return GameManager.Instance.gameCoordinator;
        }
    }
    public virtual void StartLevel(int level)
    {
        Debug.Log("StartGame");
    }
    public virtual void OnEndGame(bool isWin)
    {
        if(GameManager.Instance.EGameStatus == EGameState.None)
        {
            return;
        }
        GameManager.Instance.SetGameStatus(EGameState.None);
        if(isWin)
        {
            OnWinGame();
        }
        else
        {
            OnLoseGame();
        }
    }
    protected virtual void OnWinGame()
    {
        Debug.Log("WinGame");
    }
    protected virtual void OnLoseGame()
    {
        Debug.Log("LoseGame");
    }
    public virtual void PauseGame()
    {

    }
    public virtual void ResumeGame()
    {

    }
}
[System.Serializable]
public enum EGameMode
{
    None = 0,
    Classic = 1,
}