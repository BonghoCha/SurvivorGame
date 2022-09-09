using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : ObjectManager
{
    private void Awake()
    {
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
        Destroy(transform.parent.gameObject);
    }
}
