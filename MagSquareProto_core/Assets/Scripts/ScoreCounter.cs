using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    //static GameObject thisCanvas;
    static Text scoreValue; 
    // Start is called before the first frame update
    void Start()
    {
        scoreValue = this.transform.GetChild(0).GetComponent<Text>();
        //thisCanvas = this.transform.gameObject;
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    public void DisplayScore(string score)
    {
        scoreValue.text = score;
    }
}
