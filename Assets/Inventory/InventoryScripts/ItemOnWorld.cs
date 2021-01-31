using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{   
    //在地图上面的物品

    public Item thisItem;
    public Inventory myBag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AddNewItem();                                                   //加入背包
            Destroy(gameObject);                                             //从场景中销毁       
        }
    }

    private void AddNewItem()
    {
        if (!myBag.itemList.Contains(thisItem))             //如果背包内没有这样物品
        {
            
            //myBag.itemList.Add(thisItem);         //作废
            for (int i = 0; i < myBag.itemList.Count; i++)                  //18个格子
            {
                if (myBag.itemList[i]==null)
                {
                    thisItem.itemHeld = 1;
                    myBag.itemList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {                                                                                    //如果背包里已经有了这样物品，持有数加一
            thisItem.itemHeld += 1;
        }

        UIManager.RefreshHeldItem();
    }
}
