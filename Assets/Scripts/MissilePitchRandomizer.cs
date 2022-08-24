using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePitchRandomizer : MonoBehaviour
{
    public float randomPercent = 10;    

    // Start is called before the first frame update
    void Start()
    {
	    transform.GetComponent<AudioSource>().pitch *= 1 + Random.Range(-randomPercent / 100, randomPercent / 100);
    }
}
