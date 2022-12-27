using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectManager : MonoBehaviour
{
    [SerializeField] ParticleSystem _destroyParticle;

    protected Collider _collider;
    protected Rigidbody _rigidbody;

    [SerializeField] protected SpriteRenderer _line;
    [SerializeField] protected SpriteRenderer _body;

    protected float hp = 50;

    Color defaultColor = new Color(255, 255, 255, 1);
    Color debuffColor = new Color(255, 255, 0, 1);

    private void Awake()
    {
        if (_collider == null) _collider = GetComponent<Collider>();
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();

    }

    public void PlayParticle()
    {
        _destroyParticle.Play();
    }

    Coroutine coStop = null;
    public void Stop(float delay = 0f)
    {
        if (coStop != null)
        {
            StopCoroutine(coStop);
        }
        coStop = StartCoroutine(CoStop(delay));
    }

    protected bool _canMove = true;

    IEnumerator CoStop(float delay)
    {
        var beforeLineColor = _line.color;
        if (_line != null)
        {
            _line.color = debuffColor;
        }

        var color = _body.color;
        var beforeBodyColor = new Color(color.r, color.g, color.b);
        if (_body != null)
        {
            _body.color = debuffColor;
        }
        
        _canMove = false;

        yield return new WaitForSeconds(delay);

        if (_line != null)
        {
            _line.color = beforeLineColor;            
        }
        if (_body != null)
        {
            _body.color = beforeBodyColor;
        }
        
        _canMove = true;
    }

    public abstract void Damage(float damage, bool isCritical = false);

    public abstract void OnDestroyObject();
}
