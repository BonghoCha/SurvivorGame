using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : ObjectManager
{
    MeshRenderer _renderer;

    float destroyDelay = 1f;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public override void Damage(float damage, bool isCritical = false)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = -1;

            PlayParticle();
            OnDestroyObject();
        }
    }

    public override void OnDestroyObject()
    {
        _renderer.enabled = false;
        _collider.enabled = false;

        StartCoroutine(CoDestroy());
    }

    IEnumerator CoDestroy()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(transform.parent.gameObject);
    }
}
