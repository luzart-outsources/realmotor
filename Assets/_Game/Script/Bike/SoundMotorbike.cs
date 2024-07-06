using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SoundMotorbike : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioMotor;
    [SerializeField]
    private AudioSource audioDrift;
    [SerializeField]
    private AudioSource otherAudio;

    private BaseMotorbike baseMotorbike;

    private bool IsInit = false;
    public void Initialize(BaseMotorbike baseMotorbike)
    {
        this.baseMotorbike = baseMotorbike;
        IsInit = true;
        if (baseMotorbike.eTeam != ETeam.Player)
        {
            audioDrift.enabled = false;
            audioMotor.enabled = false;
            return;
        }

    }
    private void Update()
    {
        if (!IsInit)
        {
            return;
        }
        if (baseMotorbike.eTeam != ETeam.Player)
        {
            return;
        }
        if (AudioManager.Instance.isMuteSFX)
        {
            return;
        }
        if (baseMotorbike != null)
        {
            audioMotor.pitch = Mathf.Lerp(0.3f, 4f, Mathf.Abs(baseMotorbike.Speed) / baseMotorbike.inforMotorbike.maxSpeed);
        }
    }
    public void SoundDrifEnable(bool isEnable)
    {
        if (baseMotorbike.eTeam != ETeam.Player)
        {
            return;
        }
        if (AudioManager.Instance.isMuteSFX)
        {
            audioDrift.mute = false;
            return;
        }
        audioDrift.mute = !isEnable;
    }
}
