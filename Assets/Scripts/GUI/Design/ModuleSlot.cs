using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleSlot : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    //此为单个装备栏UI格子的控制类

    //本格坐标
    public int rawIndex;
    public int rolIndex;

    //显示图标
    public Image img;

    //默认图标
    public Sprite spr;

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerDrag.name);
        //Debug.Log("OPU");
        if (img.enabled && eventData.pointerDrag.tag == "ModuleSelection")
        {
            Image moduleImg = eventData.pointerDrag.GetComponent<Image>();
            img.sprite = moduleImg.sprite;
        }
    }

    // Use this for initialization
    void Awake()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //更新自己的显示
    public void UpdateUI()
    {
        img = GetComponent<Image>();

        if ((PlayerManager.instance.GetEquipedModule(rawIndex, rolIndex) != null) && (PlayerManager.instance.GetEquipedModule(rawIndex, rolIndex).Icon != null))
        {
            img.sprite = PlayerManager.instance.GetEquipedModule(rawIndex, rolIndex).Icon;
        }
        else
        {
            img.sprite = spr;
        }
        
        if (PlayerManager.instance.ModuleMap[rawIndex, rolIndex].type.Equals(ModuleType.None))
        {
            img.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
        else if (PlayerManager.instance.ModuleMap[rawIndex, rolIndex].type.Equals(ModuleType.Locked))
        {
            img.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
        else
        {
            img.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}
