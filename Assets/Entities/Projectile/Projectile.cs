﻿using System.Collections;
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

    public float getH()
    {
        float H;
        float S;
        float V;
        Color.RGBToHSV(this.GetComponent<SpriteRenderer>().color, out H, out S, out V);

        return H;
    }
}
