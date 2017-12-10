using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public float health = 300;
    public GameObject hitByEnemy;
    private PlayerController player;

    // Use this for initialization
    void Start()
    {
        player = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update ()
    {

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        if (projectile)
        {
            Vector3 pos = projectile.transform.position + new Vector3(0f, -0.1f, -0.1f);
            GameObject hitSparkles = Instantiate(hitByEnemy, pos, Quaternion.identity) as GameObject;

            hitSparkles.transform.parent = gameObject.transform;
            hitSparkles.transform.Rotate(new Vector3(0, 0, 255));

            health -= projectile.GetDamage();
            projectile.Hit();

            if (health <= 0)
            {
                player.BrokenShield();
                Destroy(gameObject);
            }
        }

    }
}
