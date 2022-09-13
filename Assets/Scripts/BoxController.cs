using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : ObjectManager
{
    MeshRenderer renderer;
    BoxCollider collider;

    float destroyDelay = 1f;
    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        collider = GetComponent<BoxCollider>();

        hp = 10;
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
        renderer.enabled = false;
        collider.enabled = false;

        StartCoroutine(CoDestroy());
    }

    IEnumerator CoDestroy()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(transform.parent.gameObject);
    }
}
