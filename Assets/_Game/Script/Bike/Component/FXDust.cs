namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class FXDust : MonoBehaviour
    {
        private float dustGround = 0.5f;
        public ParticleSystem Fx;
        public void EmissionOnVelocity(float velocity)
        {
            if (Fx != null)
            {
                var emission = Fx.emission;
                int vel = (int)velocity;
                emission.rateOverTime = (vel * vel * dustGround) ;
            }
        }
    }
}
