using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;
    
    public void PlayParticle()
    {
        _particleSystem.Play();
    }
}
