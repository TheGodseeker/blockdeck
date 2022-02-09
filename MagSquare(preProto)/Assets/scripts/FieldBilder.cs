using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FieldBilder : MonoBehaviour
{
    public GameObject[] fieldType;
    //GameObject [,] field = new GameObject[9,9];
    static string [,] fieldName = new string [9,9];
    static float stepNum = 0.45f;
    static int [] HLBlocksCount = new int [9]; // счет блоков в гориз. строчках
    static int [] VLBlocksCount = new int [9]; // счет блоков в верт. строчках
    static int [,] SqBlocksCount = new int [3,3]; // счет блоков в секторах 3х3

    // Start is called before the first frame update
    void Start()
    {
        FieldCicle(0); //строим поле 9х9        
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    public void LnClean(string axis, int x, int y) //для очищения от верт. и гориз. линий
    {
        switch (axis)
        {
            case "H":
                for (int i = 0; i < 9; i++)
                    {
                        GameObject curField = GameObject.Find(fieldName[i, x]);
                        GameObject curBlock = curField.gameObject.transform.GetChild(0).gameObject;
                        Destroy(curBlock);
                        curField.GetComponent<Renderer>().enabled = true;                           
                    }
                break;
            case "V":
                for (int i = 0; i < 9; i++)
                    {
                        GameObject curField = GameObject.Find(fieldName[y, i]);
                        GameObject curBlock = curField.gameObject.transform.GetChild(0).gameObject;
                        Destroy(curBlock);
                        curField.GetComponent<Renderer>().enabled = true;                          
                    }
                break;
        }
    }
    public void SqClean(int sY, int sX)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int ii = 0; ii < 3; ii++)
            {
                GameObject curField = GameObject.Find(fieldName[(ii + (3 * sX)), (i+(3 * sY))]);
                GameObject curBlock = curField.gameObject.transform.GetChild(0).gameObject;
                Destroy(curBlock);
                curField.GetComponent<Renderer>().enabled = true;
            }
        }
    }
    public void FieldCicle(int mode) //для циклов, связаных с полями
    {
         GameObject field;
         for(int i = 0; i < 9; i++)
        {
            for(int ii = 0; ii < 9; ii++)
            {
                switch (mode)
                {                   
                    case 0:
                        Vector2 objLoc = new Vector2(this.transform.position.x + (stepNum * ii), this.transform.position.y - (stepNum * i));
                        field = Instantiate(fieldType[0], objLoc, fieldType[0].transform.rotation);
                        field.transform.parent = this.transform;
                        field.name = "Normal ["+(ii + 1)+" ,"+(i + 1)+"]";
                        fieldName [ii,i] = field.name;
                        break;
                    case 1:
                        field = GameObject.Find(fieldName[ii, i]);
                        if (field.transform.childCount > 0) HLBlocksCount[i]++;                        
                        break;
                    case 2:
                        field = GameObject.Find(fieldName[i, ii]);
                        if (field.transform.childCount > 0) VLBlocksCount[i]++;                       
                        break;
                }
            }
            switch(mode)
            {
                case 1:
                    if (HLBlocksCount[i] == 9) LnClean ("H", i, 0);
                    HLBlocksCount[i] = 0; 
                    break;
                case 2:
                    if (VLBlocksCount[i] == 9) LnClean("V", 0, i);
                    VLBlocksCount[i] = 0;
                    break;
                default:
                    break;
            }
        }
    }
    public void FindCleanable(int mode)
    {
        switch (mode)
        {
            case 0: //проверка гориз. линии
                FieldCicle(1);
                FindCleanable(1);                
                break;
            case 1: //проверка верт. линии
                FieldCicle(2);
                FindCleanable(2);
                break;
            case 2:
                for (int sY = 0; sY < 3; sY++)
                {
                    for (int sX = 0; sX < 3; sX++)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int ii = 0; ii < 3; ii++)
                            {
                                GameObject field = GameObject.Find(fieldName[(ii + (3 * sX)), (i + (3 * sY))]);
                                if (field.transform.childCount > 0) SqBlocksCount[sY,sX]++;
                            }
                        }
                        if (SqBlocksCount[sY,sX] == 9) SqClean(sY,sX);
                        SqBlocksCount[sY,sX] = 0;
                    }
                }
                // нахождение нужного сектора
                // проверка ячеек в нем
                break;           
        }
    
    }
}
