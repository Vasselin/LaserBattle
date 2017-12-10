using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //Shooting variables
    public float health = 150;
    public GameObject enemyProjectile;
    public GameObject deathExplosion;
    public GameObject hitByPlayer;
    public GameObject smokeDueToDamage;


    public float enemyProjectileSpeed = -15f;
    public float enemyFiringRate = 0.5f;
    private float prob;

    //PowerUp Parameter
    public GameObject Shield;
    public GameObject DoubleShoot;
    public float randPowerUp;


    //Damage parameters
    public float sigma = 0.05f;

    //Hitpoint variables
    bool hp75mark = false;
    bool hp50mark = false;
    bool hp25mark = false;
    private float healthMax;
    private float healthPercent;

    //Score variables
    private ScoreKeeper scoreKeeper;
    public int scoreValue = 150;

    // Use this for initialization
    void Start()
    {
        healthMax = health;
        scoreKeeper = GameObject.Find("ScoreText").GetComponent<ScoreKeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        prob = Time.deltaTime * enemyFiringRate;
        if (Random.value < prob) { Shooting(); }

        SparklesRemover();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        if (projectile)
        {
            //Sparkle effect at the location of the projectile
            GameObject hitSparkles = Instantiate(hitByPlayer, projectile.transform.position + new Vector3(0f, 0.4f, -0.1f), Quaternion.identity) as GameObject;
            hitSparkles.transform.parent = gameObject.transform;
            //Destroy the projectile
            Vector3 projectilePos = projectile.transform.position;
            projectile.Hit();

            //Lower health

            health -= projectile.GetDamage();
            //health -= DamageTaken(projectile.GetDamage(), projectile.getH());

            //Check damage and apply graphic effects accordingly
            healthPercent = health / healthMax * 100;
            if (healthPercent <= 75 && !hp75mark)
            {
                hp75mark = true;
                SmokeDueToDamage();
            }
            else if (healthPercent <= 50 && !hp50mark)
            {
                hp50mark = true;
                SmokeDueToDamage();
            }
            else if (healthPercent <= 25 && !hp25mark)
            {
                hp25mark = true;
                SmokeDueToDamage();
            }
            //Check death
            else if (health <= 0)
            {
                //EXPLOOOOOSION
                GameObject explosion = Instantiate(deathExplosion, transform.position, Quaternion.identity) as GameObject;
                explosion.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, enemyProjectileSpeed, 0f);
                scoreKeeper.ScoreUpdate(scoreValue);


                //Generate a random PowerUp
                if (Random.value < randPowerUp)
                {
                    Vector3 pos = transform.position;
                    if (Random.value < 0.5)
                    {
                        GameObject PU = Instantiate(Shield, pos, Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        GameObject PU = Instantiate(DoubleShoot, pos, Quaternion.identity) as GameObject;
                    }

                }
                Destroy(gameObject);
            }
        }
    }

    void SmokeDueToDamage()
    {
        //Spawn fancy effects on enemy spaceships when they're damaged
        Vector3 hitPos = transform.position;
        hitPos.x = Mathf.Clamp((Random.value - 1f) / 2 + hitPos.x, hitPos.x - 0.35f, hitPos.x + 0.35f);
        hitPos.y = Mathf.Clamp((Random.value) / 2 + hitPos.y, hitPos.y - 0.4f, hitPos.y + 0.4f);
        GameObject smoke = Instantiate(smokeDueToDamage, hitPos, Quaternion.identity) as GameObject;
        smoke.transform.parent = gameObject.transform;
    }

    void Shooting()
    {
        //Pew pew
        GameObject beam = Instantiate(enemyProjectile, transform.position + new Vector3(0f,0f,0f), Quaternion.identity) as GameObject;
       
        //Change beam color
        beam.GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
        //

        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, enemyProjectileSpeed, 0f);
    }

    void SparklesRemover()
    {
        //Destroy sparkle effects when they've been displayed
        HitSparkles isThereHitSparkles = gameObject.GetComponent<HitSparkles>();
        if (isThereHitSparkles)
        {
            foreach (Transform child in transform)
            {
                ParticleSystem hitSparkle = child.GetComponent<ParticleSystem>();
                if (!hitSparkle.IsAlive())
                {
                    Destroy(hitSparkle);
                }
            }
        }
    }


    private float DamageTaken(float projectileDamage, float projectileH)
    {
        float damageTaken = 0f;
        float H;
        float S;
        float V;
        Color.RGBToHSV(this.GetComponent<SpriteRenderer>().color,out H,out S,out V);

        damageTaken = Mathf.Exp(- ( projectileH - H) * (projectileH - H) / (2 * sigma * sigma) );

        return damageTaken * projectileDamage;
    }
}
