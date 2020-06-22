using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIManager : MonoBehaviour {

    //public Canvas CV;
    public ModuleSlotsUI mSlots;
    public ModuleStorageUI mStorage;
    //public GameObject icon;
    //public GameObject icon_ins;

    // Use this for initialization
    void Awake ()
    {
        DragAndDrop.OnLeftDrag += DragAndDrop_OnLeftDrag;
        DragAndDrop.OnEnter += DragAndDrop_OnEnter;
        DragAndDrop.OnExit += DragAndDrop_OnExit;
        DragAndDrop.OnLeftBeginDrag += DragAndDrop_OnLeftBeginDrag;
        DragAndDrop.OnLeftEndDrag += DragAndDrop_OnLeftEndDrag;
    }

    private void OnDestroy()
    {
        DragAndDrop.OnLeftDrag -= DragAndDrop_OnLeftDrag;
        DragAndDrop.OnEnter -= DragAndDrop_OnEnter;
        DragAndDrop.OnExit -= DragAndDrop_OnExit;
        DragAndDrop.OnLeftBeginDrag -= DragAndDrop_OnLeftBeginDrag;
        DragAndDrop.OnLeftEndDrag -= DragAndDrop_OnLeftEndDrag;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void DragAndDrop_OnEnter(Transform tf)
    {

    }

    public void DragAndDrop_OnExit()
    {
    }

    //对拖动开始事件的响应
    //读取信息并赋值给中间图标
    public void DragAndDrop_OnLeftBeginDrag(Transform tf)
    {
        /*Debug.Log("aab");
        //判断拖动的是什么格子
        if (tf.transform.tag.Equals("ModuleSlot"))//如果是装备栏，则从装备中提取信息
        {
            ModuleSlot ms = tf.GetComponent<ModuleSlot>();
            if (PlayerManager.instance.ModuleMap[ms.rawIndex, ms.rolIndex] != null)
            {
                icon_ins = Instantiate<GameObject>(icon,this.transform);
                Debug.Log("aac");
                icon_ins.GetComponent<Image>().sprite = PlayerManager.instance.ModuleMap[ms.rawIndex, ms.rolIndex].Icon;
                icon_ins.SetActive(true);
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
                icon_ins = Instantiate<GameObject>(icon, this.gameObject.transform);
                Debug.Log("aac");
                icon_ins.GetComponent<Image>().sprite = PlayerManager.instance.ModuleList[ms.Index].Icon;
                icon_ins.SetActive(true);
            }
            else
            {
                return;
            }
        }
        else
        { return; }*/
    }

    //对拖动结束事件的响应
    //对装备栏或仓库进行操作
    public void DragAndDrop_OnLeftEndDrag(Transform prevTransfrom, Transform enterTransfrom)
    {
        //隐藏中间图标
        //icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -100f);
        //icon.SetActive(false);

        ModuleSelectUI msu;
        ModuleSlot prevms;
        ModuleSlot enterms;

        //首先判断被拖动起始位置是什么格子
        if (prevTransfrom.tag.Equals("ModuleSelection"))//如果起始位置是仓库
        {
            //获取起始位置的引用
            msu = prevTransfrom.GetComponent<ModuleSelectUI>();

            //接下来判断拖动结束位置
            if (enterTransfrom.tag.Equals("ModuleSlot"))//如果结束位置是装备栏，则提取仓库中的模块赋值给装备栏对应位置
            {
                enterms = enterTransfrom.GetComponent<ModuleSlot>();
                PlayerManager.instance.ModuleMap[enterms.rawIndex, enterms.rolIndex] = PlayerManager.instance.GetModule(msu.Index);
                mSlots.UpdateModuleShow();
            }
            else //除此之外的其他地方什么都不做
            { }
        }
        else if (prevTransfrom.tag.Equals("ModuleSlot"))//如果起始位置是装备栏
        {
            //获取起始位置的引用
            prevms = prevTransfrom.GetComponent<ModuleSlot>();

            //接下来判断拖动结束位置
            if (enterTransfrom.tag.Equals("ModuleSlot"))//如果结束位置是装备栏，则与装备栏对应模块换位
            {
                enterms = enterTransfrom.GetComponent<ModuleSlot>();
                PlayerManager.instance.SwitchModule(prevms.rawIndex, prevms.rolIndex, enterms.rawIndex, enterms.rolIndex);
                //mSlots.UpdateModuleShow();
            }
            else //如果是其他位置，则将原位置设为空模块
            {
                PlayerManager.instance.ModuleMap[prevms.rawIndex, prevms.rolIndex] = PlayerManager.instance.NewModule(ModuleType.None);
            }
            //更新模块显示
            mSlots.UpdateModuleShow();
        }

        //判断是否报错
        if (PlayerManager.instance.CheckModNum())
        {
            if (!PlayerManager.instance.CheckCore())
            {
                EventCenter.Broadcast<string>(EventCode.ShowErrMsg, "机甲只能装备一个核心");
            }
            else
            {
                EventCenter.Broadcast(EventCode.HideErrMsg);
            }
        }

        PlayerManager.instance.PowerCal();
    }

    //对拖动中事件的响应
    //实时改变中间图标的位置
    public void DragAndDrop_OnLeftDrag( )
    {
        /*if (icon_ins.activeInHierarchy)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CV.GetComponent<RectTransform>(), Input.mousePosition, null, out position);
            icon_ins.GetComponent<RectTransform>().anchoredPosition = position;
        }*/
    }
}
