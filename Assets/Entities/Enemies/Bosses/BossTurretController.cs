using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretController : MonoBehaviour
{
    private GameObject target;

	// Use this for initialization
	void Start () {
        target = GameObject.Find("Player01");
    }
	
	// Update is called once per frame
	void Update () {
        /*
        Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y, 0);
        transform.rotation = Quaternion.FromToRotation(new Vector3(transform.position.x, transform.position.y, 0), targetPos);*/
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, 100f);

        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10f);
    }
}
