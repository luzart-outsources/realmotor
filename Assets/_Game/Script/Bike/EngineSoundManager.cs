using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSoundManager : SoundMotorbike
{
    public float MasterVolume; // in dB
    public AudioSource audioSource, audioSource2;
    public AudioClip[] Samples;
    public AnimationCurve EngineRpm, CrossFade, CrossFade2, EngineReleaseRpm;

    [Sirenix.OdinInspector.ShowInInspector]
    private int currentGear;
    [Sirenix.OdinInspector.ShowInInspector]
    private float prevPitch, prevPitch2;
    public bool revLimiter;

    [Range(0, 1)]
    public float revValue;

    public float EngineFlow = 1;

    public override void Initialize(BaseMotorbike baseMotorbike)
    {
        base.Initialize(baseMotorbike);

        bool isPlayer = baseMotorbike.eTeam == ETeam.Player;
        audioSource.enabled = isPlayer;
        audioSource2.enabled = isPlayer;
        if (isPlayer)
        {
            ResetAudioSources();
        }
    }

    private void ResetAudioSources()
    {
        audioSource.volume =  volumnSFX;
        audioSource2.volume = volumnSFX;
        audioDrift.volume = volumnSFX;
        //audioSource.volume = 0;
        //audioSource2.volume = 0;
    }

    public override void UpdateAudio()
    {
        UpdateRevValue();

        if (currentGear != baseMotorbike.currentGear)
        {
            currentGear = baseMotorbike.currentGear;
            //if (baseMotorbike.valueVerticle > 0 || currentGear == 0)
            {
                ChangeGearSound(currentGear);
            }
        }

        UpdateEngineSound();
    }

    private void UpdateRevValue()
    {
        //if (revLimiter && baseMotorbike.revValue > 0.8f && baseMotorbike.valueVerticle>0)
        //{
        //    revValue += Time.deltaTime * Random.Range(1, 4);
        //    revValue %= 1;

        //    if (revValue > 0.1f && revValue < 0.2f)
        //    {
        //        revValue = 0.85f;
        //    }
        //}
        //else
        {
            revValue = baseMotorbike.revValue;
        }
    }

    private void UpdateEngineSound()
    {
        UpdatePitchAndVolume();
        UpdateAudioMixer(audioSource, ref prevPitch);
        UpdateAudioMixer(audioSource2, ref prevPitch2);
    }

    private void UpdatePitchAndVolume()
    {
        if (baseMotorbike.valueVerticle > 0)
        {
            float pitch = (EngineRpm.Evaluate(revValue) + 1) - currentGear / (Samples.Length - 1);
            audioSource.pitch = pitch;
            audioSource2.pitch = pitch;
            audioSource.volume = CrossFade.Evaluate(revValue) * volumnSFX;
            audioSource2.volume = CrossFade2.Evaluate(revValue) * volumnSFX;
        }
        else
        {
            float pitch = (EngineReleaseRpm.Evaluate(revValue) + 1) - currentGear / (Samples.Length - 1);
            audioSource.pitch = pitch;
            audioSource2.pitch = pitch;
            if(revValue < preRevValue)
            {
                audioSource.volume = CrossFade.Evaluate(revValue) * volumnSFX;
                audioSource2.volume = CrossFade2.Evaluate(revValue) * volumnSFX;
            }

        }
        preRevValue = revValue;
    }
    private float preRevValue = 0;
    private void UpdateAudioMixer(AudioSource source, ref float prevPitch)
    {
        source.pitch = Mathf.Lerp(prevPitch, source.pitch, Time.deltaTime * EngineFlow);
        prevPitch = source.pitch;

        float speedRatio = baseMotorbike.Speed / baseMotorbike.inforMotorbike.maxSpeed;

        source.outputAudioMixerGroup.audioMixer.SetFloat("VolumeCompensation", MasterVolume - speedRatio);
        source.outputAudioMixerGroup.audioMixer.SetFloat("Distortion", speedRatio / 3 + 0.4f);
    }

    private void ChangeGearSound(int gear)
    {
        audioSource.Stop();
        audioSource.clip = Samples[gear];
        audioSource.Play();

        audioSource2.Stop();
        audioSource2.clip = Samples[gear + 1];
        audioSource2.Play();
    }
}
