using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    public Canvas CV;
    public Image Img;

    // Use this for initialization
    void Awake()
    {
        //获得image引用
        Img = GetComponent<Image>();

        //首先隐藏自己
        this.gameObject.SetActive(false);

        //注册DragAndDrop的相关事件
        DragAndDrop.OnLeftDrag += DragAndDrop_OnLeftDrag;
        DragAndDrop.OnLeftBeginDrag += DragAndDrop_OnLeftBeginDrag;
        DragAndDrop.OnLeftEndDrag += DragAndDrop_OnLeftEndDrag;
    }

    private void OnDestroy()
    {
        DragAndDrop.OnLeftDrag -= DragAndDrop_OnLeftDrag;
        DragAndDrop.OnLeftBeginDrag -= DragAndDrop_OnLeftBeginDrag;
        DragAndDrop.OnLeftEndDrag -= DragAndDrop_OnLeftEndDrag;
    }

    // Update is called once per frame
    void Update()
    {

    }


    //对拖动开始事件的响应
    //读取信息并赋值给Image
    public void DragAndDrop_OnLeftBeginDrag(Transform tf)
    {
        //判断拖动的是什么格子
        if (tf.transform.tag.Equals("ModuleSlot"))//如果是装备栏，则从装备中提取信息
        {
            ModuleSlot ms = tf.GetComponent<ModuleSlot>();
            if (PlayerManager.instance.ModuleMap[ms.rawIndex, ms.rolIndex] != null)
            {
                Img.sprite = PlayerManager.instance.ModuleMap[ms.rawIndex, ms.rolIndex].Icon;
                this.gameObject.SetActive(true);
            }
            else
            {
                return;
            }
        }
        else if (tf.transform.tag.Equals("ModuleSelection"))//如果是仓库，则从仓库中提取信息
        {
            ModuleSelectUI ms = tf.GetComponent<ModuleSelectUI>();

            if (PlayerManager.instance.ModuleList[ms.Index] != null)
            {
                Img.sprite = PlayerManager.instance.ModuleList[ms.Index].Icon;
                this.gameObject.SetActive(true);
            }
            else
            {
                return;
            }
        }
        else
        { return; }
    }

    //对拖动结束事件的响应
    public void DragAndDrop_OnLeftEndDrag(Transform prevTransfrom, Transform enterTransfrom)
    {
        //隐藏自己
        GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -100f);
        this.gameObject.SetActive(false);
    }

    //对拖动中事件的响应
    //实时改变图标的位置
    public void DragAndDrop_OnLeftDrag()
    {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CV.GetComponent<RectTransform>(), Input.mousePosition, null, out position);
            GetComponent<RectTransform>().anchoredPosition = position;
    }
}
