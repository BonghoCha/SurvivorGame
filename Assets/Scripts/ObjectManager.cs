using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectManager : MonoBehaviour
{
    [SerializeField] ParticleSystem _destroyParticle;

    public void PlayParticle()
    {
        _destroyParticle.Play();
    }

    public abstract void Damage(bool isCritical = false);

    public abstract void OnDestroyObject();
}
