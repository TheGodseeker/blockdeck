using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMaster : MonoBehaviour
{
    static int figuresCount = 0;
    static int takenFiguresCount = 0;
    public GameObject[] figureType;
    static float stepNum = 2.51f;
    static int userScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        FigureSpawn();
        ScoreGiver("=", userScore);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    void FigureSpawn()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector2 objLoc = new Vector2(this.transform.position.x + (stepNum * i), this.transform.position.y);
            int curFigureType = Random.Range(0, (figureType.Length - 1));
            GameObject figure = Instantiate(figureType[curFigureType], objLoc, figureType[curFigureType].transform.rotation);
            figure.name = "Figure [" + (i + figuresCount) + "]";
        }
    }
    public void TakeFigure()
    {
        takenFiguresCount++;
        if (takenFiguresCount == 3)
        {
            figuresCount += takenFiguresCount;
            FigureSpawn();
            takenFiguresCount = 0;
        }
    }
    public void ScoreGiver (string mode, int score)
    {
        switch (mode)
        {
            case "+":
                userScore += score;
                ScoreGiver("=", userScore);
                break;
            case "=":
                // GameObject.Find("Text").GetComponent<ScoreCounter>().ChangeScore(System.Convert.ToString(userScore));
                //string sentScore = ;
                GameObject.Find("Canvas").GetComponent<ScoreCounter>().DisplayScore(userScore.ToString());
                break;
        }
    }
}
