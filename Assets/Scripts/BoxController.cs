using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : ObjectManager
{
    public override void Damage(bool isCritical = false)
    {
        PlayParticle();
        OnDestroyObject();        
    }

    public override void OnDestroyObject()
    {
        Destroy(gameObject);
    }
}
