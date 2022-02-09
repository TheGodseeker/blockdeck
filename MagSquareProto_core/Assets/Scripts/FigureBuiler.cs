using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureBuiler : MonoBehaviour
{
    private Vector3 startPos;
    public bool isBeingHeld = false;
    static int blocksCount;
    List<string> blockName = new List<string>();
    static int totchedBlocksCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        blocksCount = this.gameObject.transform.childCount;
        Debug.Log("Figure "+this.name+" have "+blocksCount+" block(-s)");
        for (int i = 0; i < blocksCount; i++)
        {
            GameObject curBlock = this.gameObject.transform.GetChild(i).gameObject;
            curBlock.name = "Block " + i + " <" + this.name + ">";
            blockName.Add(curBlock.name); 
        }
        //получение имен блоков для последующего вызова
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingHeld == true)
        {
            mouseCatch(2); // пока зажата ЛКМ, блок перемещается вместе с курсором
        }
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
                break;
        }
    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // при нажатии на ЛКМ
        {
            startPos = this.gameObject.transform.localPosition; // задается изнач. позиция блока
            mouseCatch(1); // определяется координаты курсора для дальнейшего перемещения блока        
                           //ChildCicle(2); // активация коллайдеров у блоков

            this.GetComponent<Collider2D>().enabled = false;
            blocksCount = this.gameObject.transform.childCount;
            Debug.Log("Figure " + this.name + " have " + blocksCount + " block(-s)");
            for (int i = 0; i < blocksCount; i++)
            {
                GameObject curBlock = GameObject.Find(blockName[i]);
                //GameObject curBlock = GameObject.Find(blockName.ElementAt(i));
                curBlock.GetComponent<Collider2D>().enabled = true;
                //GameObject.Find(blockName[i]).GetComponent<Collider2D>().enabled = true;
            }
        }
    }
    private void OnMouseUp()
    {
        //blocksCount = this.gameObject.transform.childCount;
        if (CountTouchedFields() == blocksCount) // если блок сопрекоснулся с полем
        //if(totchedBlocksCount == blocksCount)
        // проверяем сопрекосновение первого блока
        // если первый блок мопрекоснулся, то проверяем его блоки (если они есть) на сопрекосновение с соответствующими блоками 
        // если блоки встали где надо, то устанавливаем их
        {
            for(int i = 0; i < blocksCount; i++)
            {
                GameObject curBlock = GameObject.Find(blockName[i]);
                curBlock.GetComponent<BlockBuilder>().BlockBase();
            }
            //GameObject.Find("FigureSpawner").GetComponent<FigureSpawner>().GiveFigure();
            GameObject.Find("Field").GetComponent<FieldBilder>().FindCleanable(0);
            GameObject.Find("DM").GetComponent<DungeonMaster>().TakeFigure();
            GameObject.Find("DM").GetComponent<DungeonMaster>().ScoreGiver("+", (blocksCount * 2));
            Destroy(gameObject);
        }
        else
        {
            this.gameObject.transform.localPosition = startPos; // если нет, то перемещается на изнач. позицию
            for (int i = 0; i < blocksCount; i++)
            {
                GameObject curBlock = GameObject.Find(blockName[i]);
                curBlock.GetComponent<Collider2D>().enabled = false;

            }
            isBeingHeld = false;
            this.GetComponent<Collider2D>().enabled = true;
            //TotchedBlockChange(0);
        }
    }
    int CountTouchedFields()
    {
        int touchedFieldsCount = 0;
        for (int i = 0; i <= blocksCount - 1; i++)
        {
            if (GameObject.Find(blockName[i]).GetComponent<BlockBuilder>().TouchDetect() == true)
            {
                touchedFieldsCount++;
            }
        }
        Debug.Log("touchedFieldsCount = " + touchedFieldsCount);
        return touchedFieldsCount;
    }
    public void TotchedBlockChange(int num)
    {
        switch (num)
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
        Debug.Log("Totched Blocks = " + totchedBlocksCount+" at figure "+this.name);
    }
}
