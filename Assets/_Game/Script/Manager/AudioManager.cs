using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSFX;
    private AudioSource audioMusic;

    [Space, Header("In Game")]
    [SerializeField] private AudioClip clipCrashMotor;
    [SerializeField] private AudioClip clipCrashWall;
    [Space, Header("UI")]
    [SerializeField] private AudioClip clipClick;
    [SerializeField] private AudioClip clipUnlockMotor;
    [SerializeField] private AudioClip clipGoldFly;
    [SerializeField] private AudioClip clipUpgrade;
    [SerializeField] private AudioClip clipWin;
    [SerializeField] private AudioClip clipMusicBg;
    [SerializeField] private AudioClip[] clipMusicBgInGame;


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
            int value = PlayerPrefs.GetInt(MUTE_VIBRA, 0);
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

    private const string VOLUMN_SFX = "volumn_sfx";
    public float volumnSFX
    {
        get
        {
            return PlayerPrefs.GetFloat(VOLUMN_SFX, 1f);
        }
        set
        {
            audioSFX.volume = value / 2;
            PlayerPrefs.SetFloat(VOLUMN_SFX, value);
            PlayerPrefs.Save();
        }
    }
    private const string VOLUMN_MUSIC = "volumn_music";
    public float volumnMusic
    {
        get
        {
            return PlayerPrefs.GetFloat(VOLUMN_MUSIC, 1f);
        }
        set
        {
            audioMusic.volume = value / 3;
            PlayerPrefs.SetFloat(VOLUMN_MUSIC, value);
            PlayerPrefs.Save();
        }
    }





    private void Awake()
    {
        audioSFX = gameObject.AddComponent<AudioSource>();
        audioMusic = gameObject.AddComponent<AudioSource>();
        audioSFX.mute = isMuteSFX;
        audioMusic.mute = isMuteMusic;
        _isMuteVibra = isMuteVibra;
        audioSFX.volume = volumnSFX / 2;
        audioMusic.volume = volumnMusic / 2;
    }
    public void PlaySFXBtn()
    {
        PlaySFXOneshot(clipClick);
    }
    public void PlayMusicBgInGame()
    {
        int index = DataManager.Instance.CurrentLevel % clipMusicBgInGame.Length;
        audioMusic.clip = clipMusicBgInGame[index];
        audioMusic.loop = true;
        audioMusic.Play();
    }
    public void PlayMusicBg()
    {
        if (audioMusic.clip == clipMusicBg)
        {
            return;
        }
        audioMusic.clip = clipMusicBg;
        audioMusic.loop = true;
        audioMusic.Play();
    }

    public void PlaySFXCrashMotor()
    {
        PlaySFXOneshot(clipCrashMotor);
    }

    public void PlaySFXCrashWall()
    {
        PlaySFXOneshot(clipCrashWall);
    }
    public void PlaySFXCoin()
    {
        PlaySFXOneshot(clipGoldFly);
    }
    public void PlaySFXWin()
    {
        PlaySFXOneshot(clipWin);
    }
    public void PlaySFXUnlockMotor()
    {
        PlaySFXOneshot(clipUnlockMotor);
    }
    public void PlaySFXUpgradeMotor()
    {
        PlaySFXOneshot(clipUpgrade);
    }

    public void PlaySFXOneshot(AudioClip clip)
    {
        if (audioSFX != null || clip != null)
        {
            audioSFX.PlayOneShot(clip);
        }
    }

    public void StopMusic()
    {
        audioMusic.Stop();
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
