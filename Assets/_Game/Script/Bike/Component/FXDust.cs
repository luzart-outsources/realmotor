using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXDust : MonoBehaviour
{
    public ParticleSystem Fx;
    public void EmissionOnVelocity(float velocity)
    {
        if (Fx != null)
        {
            var emission = Fx.emission;
            int vel = (int)velocity;
            emission.rateOverTime = (vel * 8) ;
        }
    }
}
