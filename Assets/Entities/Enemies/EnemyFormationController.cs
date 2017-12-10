using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFormationController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject positionPrefab;
    public float speed = 5f; //move factor for the enemy formation
    public float width = 6f;
    public float height = 4f;

    private Vector3 cam_left; //left camera boundary
    private Vector3 cam_right; //right camera boundary
    private bool hasStarted = false;
    private bool left_edge_reached = false;
    private bool right_edge_reached = true;
    public float padding = 0.02f; //offset to the boundaries

    // Use this for initialization
    void Start()
    {
        /*
        //Automatically creates positions
        for (int i = 1; i<5; i++)
        {
            Vector3 pos = new Vector3((float)i, 4, 0);
            GameObject enemy = Instantiate(positionPrefab, pos, Quaternion.identity) as GameObject;
            enemy.transform.parent = transform;
        }
        */
        float zdepth = transform.position.z - Camera.main.transform.position.z;
        /*
        foreach (Transform child in transform)
        {
            //instantiate returns an object ; make it return a gameobject instead
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
        */

        //Vector3[] testPos = new Vector3[5];

        //Play space defined by camera
        cam_left = Camera.main.ViewportToWorldPoint(new Vector3(0f + padding, 0f, zdepth));
        cam_right = Camera.main.ViewportToWorldPoint(new Vector3(1f - padding, 0f, zdepth));

        StartCoroutine(Example());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        if (EmptyFormation() && hasStarted) { RespawnTest(); }
    }

    void Moving()
    {
        if (right_edge_reached) { transform.position += Vector3.left * speed * Time.deltaTime; }
        else if (left_edge_reached) { transform.position += Vector3.right * speed * Time.deltaTime; }

        float rightEdgeOfFormation = transform.position.x + (0.5f * width) - padding;
        float leftEdgeOfFormation = transform.position.x - (0.5f * width) + padding;

        if (leftEdgeOfFormation < cam_left.x)
        {
            right_edge_reached = false;
            left_edge_reached = true;
        }
        else if (rightEdgeOfFormation > cam_right.x)
        {
            right_edge_reached = true;
            left_edge_reached = false;
        }
    }

    void Moving_obsolete()
    {
        /*
        float x_clamped = transform.position.x;
        if (right_edge_reached)
        {
            //shipPosition.x -= speed * Time.deltaTime;
            //this.transform.position = shipPosition;
            transform.position += Vector3.left * speed * Time.deltaTime;
            x_clamped = Mathf.Clamp(transform.position.x, cam_left.x, cam_right.x);
            if (x_clamped != transform.position.x)
            {
                right_edge_reached = false;
                left_edge_reached = true;
            }
            transform.position = new Vector3(x_clamped, transform.position.y, transform.position.z);
        }
        else if (left_edge_reached)
        {
            //shipPosition.x += speed * Time.deltaTime;
            //this.transform.position = shipPosition;
            transform.position += Vector3.right * speed * Time.deltaTime;
            x_clamped = Mathf.Clamp(transform.position.x, cam_left.x, cam_right.x);
            if (x_clamped != transform.position.x)
            {
                right_edge_reached = true;
                left_edge_reached = false;
            }
            transform.position = new Vector3(x_clamped, transform.position.y, transform.position.z);
        }
        */
    }

    bool EmptyFormation()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount > 0)
            {
                return false;
            }
        }
        return true;
    }

    Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }
        return null;
    }

    void Respawn()
    {
        foreach (Transform child in transform)
        {
            //instantiate returns an object ; make it return a gameobject instead
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }

    void RespawnUntilFull()
    {
        
        Transform freePosition = NextFreePosition();
        //instantiate returns an object ; make it return a gameobject instead
        if (freePosition != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
            Invoke("RespawnUntilFull", 50.0f * Time.deltaTime);
        }
    }

    void RespawnTest()
    {
        foreach (Transform child in transform)
        {
            float posx = Mathf.Round(((Random.value * width) - (width / 2))*2)/2;
            float posy = Mathf.Round(((Random.value * height) - (height / 2))*2)/2;
            Vector3 pos = new Vector3(posx, posy, 0);
            child.transform.localPosition = pos;
            GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity) as GameObject;

            //Change enemy color
            enemy.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(Random.value, 1f, 1f);
            //

            enemy.transform.parent = child;
        }
    }

    IEnumerator Example()
    {
        Debug.Log("Waiting for prince/princess to rescue me...");
        yield return new WaitForSecondsRealtime(5);
        hasStarted = true;
        Debug.Log("Finally I have been rescued!");
    }
}