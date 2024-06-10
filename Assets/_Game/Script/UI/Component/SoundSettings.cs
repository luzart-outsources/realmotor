using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    private bool isSetup { get; set; } = false;
    public Button btnMusic, btnSFX, btnVibrate;
    public Image imMusic, imSFX, imVibrate;
    public Sprite[] spMusics, spSFXs, spVibrates;
    private void Setup()
    {
        SetUpButton();
    }
    public void Show()
    {
        if(!isSetup)
        {
            Setup();
            isSetup = true;
        }
        SetUpSprite();
    }
    private void SetUpSprite()
    {
        SetSpriteMusic();
        SetSpriteSFX();
        SetSpriteVibra();
    }

    private void SetUpButton()
    {
        GameUtil.ButtonOnClick(btnMusic, ClickMusic, true);
        GameUtil.ButtonOnClick(btnSFX, ClickSFX, true);
        GameUtil.ButtonOnClick(btnVibrate, ClickVibrate, true);
    }
    private void ClickMusic()
    {
        bool isMuteMusic = AudioManager.Instance.isMuteMusic;
        isMuteMusic = !isMuteMusic;
        AudioManager.Instance.isMuteMusic = isMuteMusic;
        SetSpriteMusic();
    }
    private void ClickSFX()
    {
        bool isMuteSFX = AudioManager.Instance.isMuteSFX;
        isMuteSFX = !isMuteSFX;
        AudioManager.Instance.isMuteSFX = isMuteSFX;
        SetSpriteSFX();
    }
    private void ClickVibrate()
    {
        bool isMuteVibra = AudioManager.Instance.isMuteVibra;
        isMuteVibra = !isMuteVibra;
        AudioManager.Instance.isMuteVibra = isMuteVibra;
        SetSpriteVibra();
    }
    private void SetSpriteMusic()
    {
        imMusic.sprite = AudioManager.Instance.isMuteMusic ? spMusics[0] : spMusics[1];
    }
    private void SetSpriteSFX()
    {
        imSFX.sprite = AudioManager.Instance.isMuteSFX ? spSFXs[0] : spSFXs[1];
    }

    private void SetSpriteVibra()
    {
        imVibrate.sprite = AudioManager.Instance.isMuteVibra ? spVibrates[0] : spVibrates[1];
    }
}
