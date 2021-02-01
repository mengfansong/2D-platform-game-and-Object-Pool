using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemOnDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Transform originalParent;
    public Inventory myBag;
    private int currentItemId;          //当前选中物品id

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;                          //记录当前父节点（槽子节点）
        currentItemId = originalParent.GetComponent<Slot>().slotId;

        transform.SetParent(transform.parent.parent);           //提高层级，提高渲染级别
        transform.position = eventData.position;            //位置跟随
        GetComponent<CanvasGroup>().blocksRaycasts = false;         //拖拽中 关闭遮挡
    }

    public void OnDrag(PointerEventData eventData)
    {        
        transform.position = eventData.position;            //位置跟随
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "ItemImage")       //如果射线检测到的是itemimage，则说明是有物品的格子
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);        //设置父节点为目标位置的父节点
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;       //改变被拖拽物品位置

                //itemList存储的数据进行位置交换
                var temp = myBag.itemList[currentItemId];
                myBag.itemList[currentItemId] = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId];
                myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId] = temp;

                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;     //目标的位置变到被拖拽物品原来的位置
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);              ////目标的父节点改为被拖拽物品原来的父节点

                GetComponent<CanvasGroup>().blocksRaycasts = true;      //拖拽结束打开遮挡
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject.name == "ItemSlot(Clone)")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

                myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId] = myBag.itemList[currentItemId];

                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotId != currentItemId)
                {
                    myBag.itemList[currentItemId] = null;
                }
                GetComponent<CanvasGroup>().blocksRaycasts = true;      //拖拽结束打开遮挡
                return;
            }
        }          
        transform.SetParent(originalParent);
        transform.position = originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;      //拖拽结束打开遮挡
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
