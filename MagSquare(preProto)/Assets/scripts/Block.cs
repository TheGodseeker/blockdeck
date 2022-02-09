using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    static int blocksCount;
    //private static string tochedObj = "null";
    //private static string prevTochedObj = "null";
    List<string> tochedObj = new List<string>();
    static GameObject parentObj;
    public bool isItMain = false;
    static bool didTouchField = false;

    void Start()
    {
        parentObj = this.transform.parent.gameObject;
        if(isItMain == true) 
        {
            blocksCount = parentObj.GetComponent<BlockMover>().CountSend();
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log(this.name+ " totched "+other.name);
        string objTag = other.tag;  // Определяем тег объекта
        switch (objTag)
        {
            case "Field":
                //if(tochedObj != "null") prevTochedObj = tochedObj;
                //tochedObj = other.name; // если это поле, то записываем его имя
                tochedObj.Add(other.name);
                //parentObj.GetComponent<BlockMover>().TotchedBlockChange(1);
                didTouchField = true;
                //Debug.Log("Previous toched field by"+this.name+" is "+prevTochedObj);
                break;
            default:
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        //tochedObj = "null"; // очищаем переменую от имени поля в движке
        //if (parentObj.tag == "BlockMover")
        //{
            //parentObj.GetComponent<BlockMover>().TotchedBlockChange(-1);
        //}
        //Debug.Log("Boi is "+tochedObj);
        //didTouchField = false;
    }
    public bool MainDetect()
    {
        //if(isItMain == true) Debug.Log("Main block of "+parentObj.name+" is "+ this.name);
        return isItMain;
    }
    public bool TouchDetect()
    {
        return didTouchField;
    }
    public void BlockBase()
    {
        for(int i = tochedObj.Count - 1; i >= 0; i--)
        {
            GameObject obj = GameObject.Find(tochedObj[i]); //получение объекта (поле, с которым сопрекоснулся блок) под номером i из листа
            if (obj.transform.childCount == 0) //если к объекту ничего не прекреплено
            {
                this.transform.parent = obj.transform;
                this.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y,-1);                
                parentObj = obj;
                this.transform.localScale = new Vector3(1, 1, 0);
                obj.GetComponent<Renderer>().enabled = false;
                this.GetComponent<Collider2D>().enabled = false;
                break;
                // тогда прикрепляем наш блок к данному объекту и разрываем цикл
            } 
            
        }
        //GameObject obj = GameObject.Find(tochedObj);
        //if( obj.transform.childCount > 0) obj = GameObject.Find(prevTochedObj);
        //Debug.Log("Totched object is "+obj);
        //this.transform.parent = obj.transform;
        //this.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y,-1);                
        //parentObj = obj;
        //this.transform.localScale = new Vector3(1, 1, 0);
        //obj.GetComponent<Renderer>().enabled = false;
    }
}
