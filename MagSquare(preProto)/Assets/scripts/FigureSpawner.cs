using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureSpawner : MonoBehaviour
{
    public GameObject [] figuresType;
    static float stepNum = 1.25f;
    static int typesNum;
    static int takenFigures = 0;
    // Start is called before the first frame update
    void Start()
    {
        typesNum = figuresType.Length - 1;
        SpawnFigures();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    //Random rndFType = new Random();
    //Random rnd = new Random();
    void SpawnFigures()
    {

        for (int i = 0; i < 3; i++)
        {
            Vector2 objLoc = new Vector2(this.transform.position.x + (stepNum * i), this.transform.position.y);
            //int curFigureType = rnd.Next(0, typesNum);
            int curFigureType = Random.Range(0, typesNum);
            GameObject figure = Instantiate(figuresType[curFigureType], objLoc, figuresType[curFigureType].transform.rotation);
            figure.name = "Figure ["+i+"]";
            //figure.transform.parent = this.transform;
        }
    }
   public void GiveFigure()
    {
        takenFigures++;
        if (takenFigures == 3)
        {
            SpawnFigures();
            takenFigures = 0;
        }
    }
}
