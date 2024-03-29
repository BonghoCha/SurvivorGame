﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public GameObject projectileParticle;
    public GameObject impactParticle;
    public GameObject muzzleParticle;

    public float currentPower;

    bool isCritical = false;

    [SerializeField] float liftTime = 3f;
    WaitForSeconds liftTimeDelay;

    Action<ObjectManager> onExtraEffect;

    Rigidbody rigidbody;
    SphereCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            // 크리티컬 테스트
            var testNum = 0.5f;
            var rand = UnityEngine.Random.Range(0f, 1f);
            if (rand > PlayerInfo.Critical)
            {
                isCritical = true;
                testNum = 3f;
            }

            GameManager.instance.onCameraEffect(testNum);

            var objectManager = other.transform.GetComponent<ObjectManager>();
            if (objectManager == null)
            {
                // 박스 오브젝트는 구조가 다르기 때문에 임시로 참조
                objectManager = other.transform.parent.GetComponent<ObjectManager>();
            }

            if (objectManager != null)
            {
                objectManager.Damage((PlayerInfo.Power + currentPower) * (isCritical ? PlayerInfo.CriticalPower : 1), isCritical);
                if (onExtraEffect != null)
                {
                    onExtraEffect(objectManager);
                }

            }

            GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, collider.ClosestPoint(transform.position).normalized)) as GameObject; // Spawns impact effect

            ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>(); // Gets a list of particle systems, as we need to detach the trails
                                                                                 //Component at [0] is that of the parent i.e. this object (if there is any)
            for (int i = 1; i < trails.Length; i++) // Loop to cycle through found particle systems
            {
                ParticleSystem trail = trails[i];

                if (trail.gameObject.name.Contains("Trail"))
                {
                    trail.transform.SetParent(null); // Detaches the trail from the projectile
                    Destroy(trail.gameObject, 2f); // Removes the trail after seconds
                }
            }

            Destroy(projectileParticle, 3f); // Removes particle effect after delay
            Destroy(impactP, 3.5f); // Removes impact effect after delay
            Destroy(gameObject); // Removes the projectile
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
        if (collider == null) collider = GetComponent<SphereCollider>();

        liftTimeDelay = new WaitForSeconds(liftTime);
    }

    IEnumerator LiftTime()
    {
        yield return liftTimeDelay;

        Destroy(gameObject);
    }

    public void Initialize(MissileInfo info)
    {
        projectileParticle = info.projectileParticle;
        impactParticle = info.impactParticle;
        muzzleParticle = info.muzzleParticle;

        currentPower = info.power;

        if (info.hasExtraEffect)
        {
            onExtraEffect = null;
            onExtraEffect += info.OnExtraEffect;
        }

        // 발사할 때 폭발 효과
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation, transform);
        if (muzzleParticle == null)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // 2nd parameter is lifetime of effect in seconds
        }

        // 라이프 타임
        StartCoroutine(LiftTime());
    }
}
