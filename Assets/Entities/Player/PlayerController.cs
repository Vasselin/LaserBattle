using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Level Manager
    private LevelManager levelManager;

    //Shooting variables
    public GameObject playerProjectile;
    public GameObject hitByEnemy;
    public GameObject deathExplosion;
    public GameObject smokeDueToDamage;
    private float playerProjectileSpeed = 30f;
    private float playerFiringRate = 0.10f; //in seconds
    public float health = 5000f;
    private bool singleFire = true;
    private bool reloading = false;
    public float laserColor = 0.02f;

    //Health variables
    bool hp75mark = false;
    bool hp50mark = false;
    bool hp25mark = false;
    private float healthMax;
    private float healthPercent;

    //Movement variables
    public float speed = 15f; //move factor for the ship
    private Vector3 cam_left; //left camera boundary
    private Vector3 cam_right; //right camera boundary
    public float padding = 0.02f; //offset to the boundaries

    //Shield
    public GameObject Shield;
    private bool shielded = false;

    // Use this for initialization
    void Start()
    {
        float zdepth = transform.position.z - Camera.main.transform.position.z;
        healthMax = health;

        levelManager = GameObject.FindObjectOfType<LevelManager>();
        /*
        shipPosition = this.transform.position;
        this.transform.position = shipPosition;
        */
        //Play space defined by camera
        cam_left = Camera.main.ViewportToWorldPoint(new Vector3(0f + padding, 0f, zdepth));
        cam_right = Camera.main.ViewportToWorldPoint(new Vector3(1f - padding, 0f, zdepth));
    }

    // Update is called once per frame
    void Update()
    {
        //Color laser
        laserColorChange();


        //Pew pew
        if (Input.GetKey(KeyCode.Space))
        {
            if (reloading == false)
            {
                StartCoroutine(ShootTest());
            }
        }

        if (Input.GetKeyDown(KeyCode.S)) { singleFire = !singleFire; }
        
        //Move
        Moving();


       
    }

    void Moving()
    {
        float x_clamped = transform.position.x;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //shipPosition.x -= speed * Time.deltaTime;
            //this.transform.position = shipPosition;
            transform.position += Vector3.left * speed * Time.deltaTime;
            x_clamped = Mathf.Clamp(transform.position.x, cam_left.x, cam_right.x);
            transform.position = new Vector3(x_clamped, transform.position.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //shipPosition.x += speed * Time.deltaTime;
            //this.transform.position = shipPosition;
            transform.position += Vector3.right * speed * Time.deltaTime;
            x_clamped = Mathf.Clamp(transform.position.x, cam_left.x, cam_right.x);
            transform.position = new Vector3(x_clamped, transform.position.y, transform.position.z);
        }
    }

    IEnumerator ShootTest()
    {
        Shooting();
        reloading = true;
        yield return new WaitForSecondsRealtime(playerFiringRate);
        reloading = false;
    }

    void Shooting()
    {
        if (singleFire)
        {
            GameObject beam = Instantiate(playerProjectile, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, playerProjectileSpeed, 0f);
            beam.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(laserColor, 1f, 1f);
        }
        else
        {
            GameObject beam = Instantiate(playerProjectile, transform.position + new Vector3(0.5f, 0f, 0f), Quaternion.identity) as GameObject;
            GameObject beam2 = Instantiate(playerProjectile, transform.position + new Vector3(-0.5f, 0f, 0f), Quaternion.identity) as GameObject;
            beam2.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, playerProjectileSpeed, 0f);
            beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, playerProjectileSpeed, 0f);

            beam.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(laserColor, 1f, 1f);
            beam2.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(laserColor, 1f, 1f);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        PowerUp powerUp = collision.gameObject.GetComponent<PowerUp>();

        if (projectile)
        {
            //Check if shielded

            if (!shielded)
            {
                singleFire = true;

                //Sparkle effect at the location of the projectile
                Vector3 pos = projectile.transform.position + new Vector3(0f, -0.4f, -0.1f);
                GameObject hitSparkles = Instantiate(hitByEnemy, pos, Quaternion.identity) as GameObject;

                hitSparkles.transform.parent = gameObject.transform;
                hitSparkles.transform.Rotate(new Vector3(0, 0, 255));

                //Destroy the projectile
                Vector3 projectilePos = projectile.transform.position;
                projectile.Hit();
                //Lower health
                health -= projectile.GetDamage();
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
                if (health <= 0)
                {
                    //EXPLOOOOOSION
                    GameObject explosion = Instantiate(deathExplosion, transform.position, Quaternion.identity) as GameObject;
                    explosion.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, -playerProjectileSpeed, 0f);

                    levelManager.LoadLevel("Loose Screen");
                    Destroy(gameObject);
                }
            }
        }


        if (powerUp)
        {
            
            if ( powerUp.getPowerUp() == "DoubleShoot")
            {
                singleFire = false;
                
            }
            else if (powerUp.getPowerUp() == "Shield" && !shielded)
            {
                GameObject shield = Instantiate(Shield, this.transform.position, Quaternion.identity) as GameObject;
                shield.transform.parent = gameObject.transform;
                shielded = true;
            }
            powerUp.Hit();

        }
    }

    public void BrokenShield()
    {
        shielded = false;
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

    // Change the color of the laser

    void laserColorChange()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            laserColor += 0.01f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            laserColor -= 0.01f;
        }

        if (laserColor < 0f)
        {
            laserColor = 0.999f;
        }
        if (laserColor > 1f)
        {
            laserColor = 0f;
        }
    }

}
