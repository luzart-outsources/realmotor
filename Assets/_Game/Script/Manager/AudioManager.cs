using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSFX;
    private AudioSource audioMusic;
    private const string MUTE_SFX = "mute_sfx";
    public bool isMuteSFX
    {
        get
        {
            if (!PlayerPrefs.HasKey(MUTE_SFX))
            {
                PlayerPrefs.SetInt(MUTE_SFX, 0);
                return false;
            }
            int value = PlayerPrefs.GetInt(MUTE_SFX);
            if (value == 0)
            {
                return false;
            }
            return true;
        }
        set
        {
            PlayerPrefs.SetInt(MUTE_SFX, value ? 1 : 0);
            if (audioSFX != null)
            {
                audioSFX.mute = value;
            }
            PlayerPrefs.Save();
        }
    }
    private const string MUTE_MUSIC = "mute_music";
    public bool isMuteMusic
    {
        get
        {
            if (!PlayerPrefs.HasKey(MUTE_MUSIC))
            {
                PlayerPrefs.SetInt(MUTE_MUSIC, 0);
                return false;
            }
            int value = PlayerPrefs.GetInt(MUTE_MUSIC);
            if (value == 0)
            {
                return false;
            }
            return true;
        }
        set
        {
            PlayerPrefs.SetInt(MUTE_MUSIC, value ? 1 : 0);
            if (audioMusic != null)
            {
                audioMusic.mute = value;
            }
            PlayerPrefs.Save();
        }
    }
    private const string MUTE_VIBRA = "mute_vibra";
    private bool _isMuteVibra = false;
    public bool isMuteVibra
    {
        get
        {
            int value = PlayerPrefs.GetInt(MUTE_VIBRA,0);
            if (value == 0)
            {
                _isMuteVibra = false;
            }
            else
            {
                _isMuteVibra = true;
            }

            return _isMuteVibra;
        }
        set
        {
            _isMuteVibra = value;
            PlayerPrefs.SetInt(MUTE_VIBRA, _isMuteVibra ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    [SerializeField] private AudioClip audioClick;


    private void Awake()
    {
        audioSFX = gameObject.AddComponent<AudioSource>();
        audioMusic = gameObject.AddComponent<AudioSource>();
        audioSFX.mute = isMuteSFX;
        audioMusic.mute = isMuteMusic;
        _isMuteVibra = isMuteVibra;
    }
    public void PlaySFXBtn()
    {
        audioSFX.PlayOneShot(audioClick);
    }
    public void Vibrate()
    {
        if (_isMuteVibra)
        {
            return;
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
