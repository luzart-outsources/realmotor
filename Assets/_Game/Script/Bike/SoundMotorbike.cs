namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using UnityEngine;
    
    public class SoundMotorbike : MonoBehaviour
    {
        //[SerializeField]
        //private AudioSource audioMotor;
    
        [SerializeField]
        protected AudioSource audioDrift;
        [SerializeField]
        public float soundEngine = 0.3f;
    
        protected BaseMotorbike baseMotorbike;
        public float volumnSFX;
        private bool IsInit = false;
        public virtual void Initialize(BaseMotorbike baseMotorbike)
        {
            this.baseMotorbike = baseMotorbike;
            IsInit = true;
            if (baseMotorbike.eTeam != ETeam.Player)
            {
                audioDrift.enabled = false;
                //audioMotor.enabled = false;
    
                return;
            }
            volumnSFX = AudioManager.Instance.volumnSFX * soundEngine;
            //audioMotor.volume = audioMotor.volume * volumSFX;
            //audioDrift.volume = volumnSFX;
    
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
                UpdateAudio();
            }
        }
        public virtual void UpdateAudio()
        {
            float minPitch = 0.5f; // Pitch tối thiểu
            float maxPitch = 2.5f;   // Pitch tối đa
            float step = 0.1f;     // Khoảng pitch
    
            // Tính RPM giả định dựa trên tốc độ của xe máy
            float minRPM = 1000f; // RPM tối thiểu
            float maxRPM = 8000f; // RPM tối đa
    
            // Tính RPM hiện tại dựa trên tốc độ của xe máy
            float currentRPM = Mathf.Lerp(minRPM, maxRPM, Mathf.Abs(baseMotorbike.Speed) / baseMotorbike.inforMotorbike.maxSpeed);
    
            // Tính pitch dựa trên RPM
            float targetPitch = Mathf.Lerp(minPitch, maxPitch, (currentRPM - minRPM) / (maxRPM - minRPM));
    
            // Làm tròn pitch đến giá trị gần nhất với khoảng step
            //audioMotor.pitch = Mathf.Round(targetPitch / step) * step;
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
}
