using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectManager : MonoBehaviour
{
    [SerializeField] ParticleSystem _destroyParticle;

    protected Collider _collider;
    protected Rigidbody _rigidbody;

    protected SpriteRenderer _sprite;

    protected float hp = 50;

    private void Awake()
    {
        if (_sprite == null) _sprite = GetComponent<SpriteRenderer>();
        if (_collider == null) _collider = GetComponent<Collider>();
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();

    }

    public void PlayParticle()
    {
        _destroyParticle.Play();
    }

    public abstract void Damage(float damage, bool isCritical = false);

    public abstract void OnDestroyObject();
}
