using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BagOnDrag : MonoBehaviour,IDragHandler
{
    RectTransform currentRect;

    private void Awake()
    {
        currentRect = GetComponent<RectTransform>();
    }


    public void OnDrag(PointerEventData eventData)
    {
        currentRect.anchoredPosition += eventData.delta;
        
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
