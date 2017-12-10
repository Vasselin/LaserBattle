using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSparkles : MonoBehaviour
{

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }
}

