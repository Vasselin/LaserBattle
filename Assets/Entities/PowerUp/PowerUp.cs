using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public string puType;
    public GameObject PuPrefab;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	}


    public void Hit()
    {
        Destroy(gameObject);
    }

    public string getPowerUp()
    {
        return puType;
    }
}
