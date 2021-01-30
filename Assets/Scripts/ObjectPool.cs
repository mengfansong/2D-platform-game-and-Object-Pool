using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    public GameObject shadowPrefab;

    public int shadowCount;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;

        //初始化对象池
        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);

            //active设置为false，返回对象池
            ReturnPool(newShadow);

        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);

        availableObjects.Enqueue(gameObject);       //加入队列
    }

    public GameObject GetFromPool()
    {

        if(availableObjects.Count == 0)             //残影不够用了，重新填充对象池
        {
            FillPool();
        }

        var outShadow = availableObjects.Dequeue();
        outShadow.SetActive(true);
        return outShadow;
    }
}
