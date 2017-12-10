using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationChoice : MonoBehaviour {

    public List<GameObject> enemyFormations = new List<GameObject>();
    private bool hasStarted = false;
    private int nbWaves = 0;
    private Text nbWavesField;

    // Use this for initialization
    void Start () {
        nbWavesField = GameObject.Find("NbWavesText").GetComponent<Text>();
        StartCoroutine(Example());
    }
	
	// Update is called once per frame
	void Update () {
        if (hasStarted)
        {
            CheckNoFormation();
            CheckEmptyFormation();
            nbWavesField.text = nbWaves.ToString();
        }
    }

    void CheckNoFormation()
    {
        if (transform.childCount == 0)
        {
            int prefabIndex = Random.Range(0, enemyFormations.Count);
            Debug.Log(prefabIndex);
            GameObject enemyFormation = Instantiate(enemyFormations[prefabIndex], transform.position, Quaternion.identity) as GameObject;
            enemyFormation.transform.parent = transform;
            nbWaves++;
        }
    }

    void CheckEmptyFormation()
    {
        foreach (Transform child in transform)
        {
            if(child)
            {
                EnemyFormationController enemyFormationController = child.gameObject.GetComponent<EnemyFormationController>();
                if (enemyFormationController.IsFormationEmpty() && enemyFormationController.IsFormationInstantiated())
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    IEnumerator Example()
    {
        Debug.Log("Waiting for prince/princess to rescue me...");
        yield return new WaitForSecondsRealtime(2);
        hasStarted = true;
        Debug.Log("Finally I have been rescued!");
    }
}
