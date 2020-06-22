using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleStateSlot : MonoBehaviour
{
    //本格坐标
    public int rawIndex;
    public int rolIndex;

    //显示图标
    public Image img;

    //默认图标
    public Sprite spr;

    public Color originColor;

    // Use this for initialization
    void Awake()
    {
        img = GetComponent<Image>();
        originColor = img.color;

        EventCenter.AddListener<int,int,int,int>(EventCode.StatusChanged, UpdateFunction);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<int, int, int, int>(EventCode.StatusChanged, UpdateFunction);
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
    }

    public void UpdateState(float hpPercent)
    {
        if (hpPercent > 0.66)
        {
            img.color = Color.white;
        }
        else if (hpPercent > 0.33)
        {
            img.color = Color.yellow;
        }
        else if (hpPercent > 0)
        {
            img.color = Color.red;
        }
        else
        {
            img.color = Color.black;
        }
        originColor = img.color;
    }

    public void UpdateFunction(int a, int b, int c, int d)
    {
        float h, s, v;
        Color.RGBToHSV(originColor, out h, out s, out v);
        v = v / 2.0f;
        Color darkColor = Color.HSVToRGB(h, s, v);
        if (PlayerManager.instance.GetCurrentPlan()[rawIndex, rolIndex])
        {
            img.color = originColor;
        }
        else
        {
            img.color = darkColor;
        }
    }
}
