using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DistributeSwitch : MonoBehaviour, IPointerClickHandler
{

    public int rawIndex;
    public int rolIndex;
    public Image img;
    public Sprite spr;

    [HideInInspector]
    public bool isOpen;

    [HideInInspector]
    public int position;
    private Color origColor;
    private Color darkColor;

    // Use this for initialization
    void Awake()
    {
        isOpen = true;
        img = GetComponent<Image>();
        origColor = img.color;
        float h, s, v;
        Color.RGBToHSV(origColor, out h, out s, out v);
        v = v / 2.0f;
        darkColor = Color.HSVToRGB(h, s, v);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerManager.instance.ModuleMap[rawIndex, rolIndex].type.Equals(ModuleType.Weapon) || PlayerManager.instance.ModuleMap[rawIndex, rolIndex].type.Equals(ModuleType.Power))
        {
            PlayerManager.instance.SetModuleOpen(rawIndex, rolIndex);
        }
        //transform.parent.gameObject.GetComponent<DistributionPanel>().UpdateStatusText();
    }
    
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

        if (PlayerManager.instance.ModuleMap[rawIndex, rolIndex].type.Equals(ModuleType.None) || PlayerManager.instance.ModuleMap[rawIndex, rolIndex].type.Equals(ModuleType.Locked))
        {
            img.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }

        if (PlayerManager.instance.GetCurrentPlan()[rawIndex, rolIndex])
        {
            img.color = origColor;
        }
        else
        {
            img.color = darkColor;
        }
    }
}
