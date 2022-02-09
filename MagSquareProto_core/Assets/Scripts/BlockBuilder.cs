using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
    static int blocksCount;
    List<string> tochedObj = new List<string>();
    static GameObject parentObj;
    public bool isItMain = false;
    static bool didTouchField = false;
    // Start is called before the first frame update
    void Start()
    {
        parentObj = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(this.name + " totched " + other.name);
        string objTag = other.tag;  // Определяем тег объекта
        switch (objTag)
        {
            case "Field":
                tochedObj.Add(other.name);
                didTouchField = true;
                //parentObj.GetComponent<FigureBuiler>().TotchedBlockChange(1);
                break;
            default:
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //didTouchField = false;
        //parentObj.GetComponent<FigureBuiler>().TotchedBlockChange(-1);
    }
    public void BlockBase()
    {
        for (int i = tochedObj.Count - 1; i >= 0; i--)
        {
            GameObject obj = GameObject.Find(tochedObj[i]); //получение объекта (поле, с которым сопрекоснулся блок) под номером i из листа
            if (obj.transform.childCount == 0) //если к объекту ничего не прекреплено
            {
                this.transform.parent = obj.transform;
                this.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -1);
                parentObj = obj;
                this.transform.localScale = new Vector3(1, 1, 0);
                obj.GetComponent<Renderer>().enabled = false;
                this.GetComponent<Collider2D>().enabled = false;
                break;
                // тогда прикрепляем наш блок к данному объекту и разрываем цикл
            }

        }
    }
    public bool TouchDetect()
    {
        return didTouchField;
    }
}
