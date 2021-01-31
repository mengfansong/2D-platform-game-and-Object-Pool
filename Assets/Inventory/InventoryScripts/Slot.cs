using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item slotItem;
    public Image slotImage;
    public Text slotCount;
    public string slotInfo;


    public GameObject ItemInSlot;

    public void ItemOnClick()
    {
        UIManager.instance.UpdateDescription(slotInfo);
    }

    public  void SetupSlot(Item item)
    {
        if (item == null)
        {
            ItemInSlot.SetActive(false);            //空槽子不显示图片
            return;
        }

        slotImage.sprite = item.itemImage;
        slotCount.text = item.itemHeld.ToString();
        slotInfo = item.itemInfo;
    }


}
