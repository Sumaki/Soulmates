using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayObjectParticle : MonoBehaviour
{
    public GameObject particle_fire;
    public GameObject particle_explosion;
    public bool playParticle;




    private void Update()
    {
        if (playParticle)
        {
            particle_fire.GetComponent<ParticleSystem>().Play();
            particle_explosion.GetComponent<ParticleSystem>().Play();
            playParticle = false;
        }
    }
}
