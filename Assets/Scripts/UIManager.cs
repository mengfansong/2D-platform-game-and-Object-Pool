using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour                  //管理ui的显示以及背包的逻辑，最好还是让背包的逻辑在另一个脚本中进行管理。
{
    // Start is called before the first frame update
    public static UIManager instance;

    public GameObject InventoryBtn;             //打开背包的按键

    


    [Header("背包系统")]
    public GameObject inventory;
    private bool isInventoryPageDisplay = false;

    public Inventory myBag;             //背包（背包、铁匠铺、杂货店等等）

    public GameObject slotGrid;         //背包内的格子容器

    //public Slot slotPrefab;                 //不再需要动态生成槽子了
    public GameObject emptySlot;            //空的槽子


    public List<GameObject> slots = new List<GameObject>();         //槽子的list
    public Text itemInfo;

    private void OnEnable()
    {
        RefreshHeldItem();
        instance.itemInfo.text = "";
    }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }



    private void Start()
    {
        inventory.SetActive(false);         //初始时背包处于关闭状态
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))                                            //开闭背包是I键
        {
            InventoryPageDisplay();
        }
    }

    public void InventoryPageDisplay()         //打开背包界面
    {
        InventoryBtn.SetActive(isInventoryPageDisplay);
        isInventoryPageDisplay = !isInventoryPageDisplay;
        inventory.SetActive(isInventoryPageDisplay);        
    }

    //添加拖拽等功能后，需要让背包中的每一个槽子都提前设置好，因此下面这个方法需要彻底改掉。

    /*          
    public static void CreateNewItem(Item item)
    {
        Slot newSlot = Instantiate(instance.slotPrefab,instance.slotGrid.transform.position,Quaternion.identity);
        newSlot.gameObject.transform.SetParent(instance.slotGrid.transform,false);          //跟随父节点改变
        newSlot.slotItem = item;
        newSlot.slotImage.sprite = item.itemImage;
        newSlot.slotCount.text = item.itemHeld.ToString();
        
    }
    */
    

    public static void RefreshHeldItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)            //显示出来的背包内容
        {
            if (instance.slotGrid.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }

        for (int i = 0; i < instance.myBag.itemList.Count; i++)             //背包内容的数据
        {
            //CreateNewItem(instance.myBag.itemList[i]);            //作废
            instance.slots.Add(Instantiate(instance.emptySlot));
            instance.slots[i].transform.SetParent(instance.slotGrid.transform,false);

            instance.slots[i].GetComponent<Slot>().slotId = i;
            instance.slots[i].GetComponent<Slot>().SetupSlot(instance.myBag.itemList[i]);

        }
    }


    public void UpdateDescription(string description)
    {
        instance.itemInfo.text = description;
    }

    
}
