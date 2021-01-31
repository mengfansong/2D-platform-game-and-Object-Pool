using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int itemHeld;                            //该物品的持有数量
    [TextArea(5,5)]
    public string itemInfo;                         //对该物品的描述

    public bool equip;                              //是否可被装备

     
    
}
