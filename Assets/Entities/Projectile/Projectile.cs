using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileDamage = 100f;

    public float GetDamage()
    {
        return projectileDamage;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
