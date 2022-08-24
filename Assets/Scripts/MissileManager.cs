using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MissileInfo
{
    public GameObject projectileParticle;
    public GameObject impactParticle;
    public GameObject muzzleParticle;
}

public enum MissileType
{
    FireballSoftBlue,
    RocketMeshMissileBlue
}

public class MissileManager : MonoBehaviour
{
    public static MissileManager instance = null;

    public GameObject missileDefault;

    [SerializeField] List<MissileInfo> missileInfo;

    public GameObject GetMissile(MissileType type)
    {
        var missile = Instantiate(missileDefault);
        missile.GetComponent<MissileController>().Initialize(GetInfo(type));

        return missile;
    }

    public MissileInfo GetInfo(MissileType type)
    {
        return missileInfo[(int)type];
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            var count = FindObjectsOfType<GameManager>();
            if (count.Length > 1) return;

            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
