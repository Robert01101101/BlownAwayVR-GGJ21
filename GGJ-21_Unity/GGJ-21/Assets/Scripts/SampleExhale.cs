using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleExhale : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.EmissionModule emissionModule;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        emissionModule = ps.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if (ExhaleInput.isExhaling)
        {
            if (!ps.isPlaying) ps.Play();
        } else
        {
            if (ps.isPlaying) ps.Stop();
        }




        emissionModule.rateOverTime = ExhaleInput.strength * 40;
    }
}
