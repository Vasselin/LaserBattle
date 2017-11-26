using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

    protected int score = 0;
    private Text scoreField;

    // Use this for initialization
    void Start()
    {
        scoreField = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreField.text = score.ToString();
    }

    public void ScoreUpdate(int points)
    {
        score += points;
    }

    public void Reset()
    {
        score = 0;
    }
}
