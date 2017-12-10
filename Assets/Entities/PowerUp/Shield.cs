using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public float health = 300;
    public GameObject hitByEnemy;
    public PlayerController player;
    // Use this for initialization
	// Update is called once per frame
	void Update () {

	}

    public void Hit()
    {

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        health -= projectile.GetDamage();
        projectile.Hit();

        if (health <= 0)
        {
            Destroy(gameObject);

        }


    */

    }
}
