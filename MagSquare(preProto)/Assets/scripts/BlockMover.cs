using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure
{
    public bool takeable = true;    

    public void TakingPosibility(int mode)
    {
        
        switch(mode)
        {
            case 0:
                takeable = false;
                break;
            case 1:
                takeable = true;
                break;
        }                
    }
    public bool CanBeTaken()
    {
        return takeable;
    }    
}

public class BlockMover : MonoBehaviour
{
    private Vector3 startPos;
    Figure block = new Figure();
    GameObject mainBlock;
    public bool isBeingHeld = false;
    static int blocksCount;
    static int totchedBlocksCount = 0; // [на удаление]

    // Start is called before the first frame update
    void Start()
    {
        blocksCount = this.gameObject.transform.childCount; //получаем кол-во дочерних объектов (блоков)
        Debug.Log("Blocks = "+blocksCount+" at "+this.name+" figure");    
        ChildCicle(0); //находим глав.блок
    }

    // Update is called once per frame
    void Update()
    {
        if(isBeingHeld == true)
        {
            mouseCatch(2); // пока зажата ЛКМ, блок перемещается вместе с курсором
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // при нажатии на ЛКМ
        {
            startPos = this.gameObject.transform.localPosition; // задается изнач. позиция блока
            mouseCatch(1); // определяется координаты курсора для дальнейшего перемещения блока        
                           //ChildCicle(2); // активация коллайдеров у блоков

            int i = 0; 
            while (i < blocksCount)
            {
                GameObject curBlock = this.gameObject.transform.GetChild(i).gameObject;
                curBlock.GetComponent<Collider2D>().enabled = true;                
                switch (i)
                {
                    case 0:
                        continue;
                    default:
                        if (curBlock.transform.position.y > mainBlock.transform.position.y)
                        {                            
                           curBlock.transform.position = new Vector3(curBlock.transform.position.x, curBlock.transform.position.y + (0.15f * i), curBlock.transform.position.z);                            
                        }
                        if (curBlock.transform.position.x > mainBlock.transform.position.x)
                        {                            
                            curBlock.transform.position = new Vector3(curBlock.transform.position.x + (0.15f), curBlock.transform.position.y, curBlock.transform.position.z);                            
                        }
                        break;
                }
                i++;
            }
            //for(int i = 1; i < blocksCount; i++)
            //{
               // GameObject curBlock = this.gameObject.transform.GetChild(i).gameObject;
               // curBlock.transform.position = new Vector3(curBlock.transform.position.x, curBlock.transform.position.y + (0.15f * i), curBlock.transform.position.z);
            //}
            //ChildCicle(4);
            this.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnMouseUp()
    {
        //if(block.CanBeTaken() == true)
            //{

            if(CountTouchedFields() == blocksCount) // если блок сопрекоснулся с полем
            {
            //ChildCicle(1);
            //for(int i = 0; i < blocksCount; i++)
            int i = 0;
                while (i < blocksCount)
                {
                    GameObject curBlock = this.gameObject.transform.GetChild(i).gameObject;
                    curBlock.GetComponent<Block>().BlockBase();
                    i++;
                }
                GameObject.Find("FigureSpawner").GetComponent<FigureSpawner>().GiveFigure();
                GameObject.Find("Field").GetComponent<FieldBilder>().FindCleanable(0);
                //TotchedBlockChange(0);                
                Destroy(gameObject);
                
                //GameObject obj = GameObject.Find(tochedObj);
                //this.gameObject.transform.localPosition = new Vector3(obj.transform.position.x, obj.transform.position.y, -1); // блок перемещается на координаты поля            
                //block.TakingPosibility(0);
            }
            else 
            {
                this.gameObject.transform.localPosition = startPos; // если нет, то перемещается на изнач. позицию
                ChildCicle(3); //отключение колайдеров у блоков
                isBeingHeld = false;
                ChildCicle(4);
                TotchedBlockChange(0);
                //for(int i = 1; i < blocksCount; i++)
                //{
                  //  GameObject curBlock = this.gameObject.transform.GetChild(i).gameObject;
                   // curBlock.transform.position = new Vector3(curBlock.transform.position.x, curBlock.transform.position.y - (0.15f * i), curBlock.transform.position.z);
                //}
                this.GetComponent<Collider2D>().enabled = true;
            }
            
            //}    
    }
    int CountTouchedFields()
    {
        int touchedFieldsCount = 0;
        for (int i = 0; i <= blocksCount - 1; i++)
        {
            if(this.transform.GetChild(i).GetComponent<Block>().TouchDetect() == true)
                {
                    touchedFieldsCount++;
                }
        }
        Debug.Log("touchedFieldsCount = "+ touchedFieldsCount);
        return touchedFieldsCount;
    }
    void mouseCatch(int mode)
    {
        Vector3 mousePos;
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        switch (mode)
        {
            case 1:
                isBeingHeld = true;
                break;
            case 2:
                this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, -1); // перемещение объекта вслед за курсором
                //mainBlock.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, -1);
                break;
        }
    }
    public int CountSend()
    {
        return blocksCount;
    }
    public void TotchedBlockChange(int num) // [на удаление]
    {
        switch(num)
        {
            case 0:
                totchedBlocksCount = 0;
                break;
            case 1:
                totchedBlocksCount++;
                break;
            case -1:
                totchedBlocksCount--;
                break;
            default:
               Debug.LogError("Enter a number (1 or -1) into the code");
               break; 
        }
        Debug.Log("Totched Blocks = "+totchedBlocksCount);
    }
    void ChildCicle(int mode) //для циклов, связаных с блоками
    {
        for(int i = 0; i <= blocksCount - 1; i++) //проверяем на наличие главного блока
        {
            //if (i >= blocksCount) break;
            GameObject curBlock = this.gameObject.transform.GetChild(i).gameObject;
            switch(mode)
            {
                case 0:
                    if(curBlock.GetComponent<Block>().MainDetect() == true) //если находим глав.блок 
                    {   
                        mainBlock = curBlock; //то подключаем его к этому скрипту
                        Debug.Log("Main block of"+this.name+" is "+mainBlock.name);
                        break;
                    }
                    break;
                case 1:
                    this.gameObject.transform.GetChild(0).gameObject.GetComponent<Block>().BlockBase();
                    break;
                case 2:
                    curBlock.GetComponent<Collider2D>().enabled = true;
                    break;
                case 3:
                    curBlock.GetComponent<Collider2D>().enabled = false;
                    break;
                case 4:                    
                    switch(i)
                    {
                        case 0:
                            continue;
                        default:
                            if(curBlock.transform.position.y > mainBlock.transform.position.y)
                            {
                                if (isBeingHeld == true)
                                {
                                    curBlock.transform.position = new Vector3(curBlock.transform.position.x, curBlock.transform.position.y + (0.15f * i), curBlock.transform.position.z);
                                }
                                else curBlock.transform.position = new Vector3(curBlock.transform.position.x, curBlock.transform.position.y - (0.15f * i), curBlock.transform.position.z);
                            }
                            if(curBlock.transform.position.x > mainBlock.transform.position.x)
                            {
                                if (isBeingHeld == true)
                                {
                                    curBlock.transform.position = new Vector3(curBlock.transform.position.x  + (0.15f), curBlock.transform.position.y, curBlock.transform.position.z);
                                }
                                else curBlock.transform.position = new Vector3(curBlock.transform.position.x  - (0.15f), curBlock.transform.position.y, curBlock.transform.position.z);
                            } 
                            break;
                    }
                    break;
   
            }
            
        }
    }
}
