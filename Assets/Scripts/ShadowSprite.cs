using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;       //获得player的坐标

    private SpriteRenderer thisSprite;          //残影的当前图像
    private SpriteRenderer playerSprite;        //玩家的当前图像

    private Color color;

    [Header("时间控制参数")]
    public float activeTime;            //每个残影的存活时间长度
    public float activeStart;           //记录这个残影开始显示的时刻

    [Header("残影的不透明度控制")]
    private float alpha;
    public float alphaSet;          //不透明度的初始值
    public float alphaMultipler;            //小于0的因子，用来不断相乘令残影逐渐趋于透明

    private void OnEnable()
    {
        player=GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;            //生成残影
        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;        //记录开始时刻 
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        alpha *= alphaMultipler;
        color = new Color(0.5f, 0.5f, 1, alpha);

        thisSprite.color = color;

        if (Time.time > activeStart + activeTime)       //残影的存在时间够了，该回收了
        {
            //返回对象池
            ObjectPool.instance.ReturnPool(this.gameObject);
        }
    }
}
