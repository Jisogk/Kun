using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleSlotsUI : MonoBehaviour
{
    public GameObject moduleSlot;
    public GameObject[,] ModuleSlotUIMap;

    [SerializeField] private float UILength = 100f;
    [SerializeField] private float UIHeight = 100f;

    // Use this for initialization
    void Awake () {
        
        InitializeSlots();
        UpdateSlots();
        UpdateModuleShow();

        //注册监听ChangeDiagram事件
        EventCenter.AddListener(EventCode.ChangeDiagram, UpdateSlotsShow);
    }

    private void OnDestroy()
    {
        //移除监听ChangeDiagram事件
        EventCenter.RemoveListener(EventCode.ChangeDiagram, UpdateSlotsShow);
    }

    // Update is called once per frame
    void Update () {
		
	}

    //创建装备栏
    public void InitializeSlots()
    {
        int raw = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(0);
        int rol = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(1);

        Vector3 origin = new Vector3((0 - UILength * raw / 2), (0 - UILength * rol / 2), 0f);

        ModuleSlotUIMap = new GameObject[raw,rol];

        for(int a=0;a<raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                ModuleSlotUIMap[a, b] = Instantiate<GameObject>(moduleSlot,this.transform);
                ModuleSlotUIMap[a, b].GetComponent<RectTransform>().anchoredPosition = origin + new Vector3(0.5f* UILength + UILength * a, 0.5f* UIHeight + UIHeight * b, 0f);
                ModuleSlotUIMap[a, b].GetComponent<ModuleSlot>().rawIndex = a;
                ModuleSlotUIMap[a, b].GetComponent<ModuleSlot>().rolIndex = b;
            }
        }
    }

    //更新装备栏UI的属性
    public void UpdateSlots()
    {
        int raw = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(0);
        int rol = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(1);

        for (int a = 0; a < raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                if (PlayerManager.instance.ModuleMap[a, b].type.Equals(ModuleType.Locked))
                {
                    ModuleSlotUIMap[a, b].SetActive(false);
                }
            }
        }
    }

    //统一刷新装备栏UI的显示
    public void UpdateModuleShow()
    {
        int raw = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(0);
        int rol = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(1);

        for (int a = 0; a < raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                ModuleSlotUIMap[a, b].GetComponent<ModuleSlot>().UpdateUI();
            }
        }
    }

    //修正格子的排列
    public void ResetSlots()
    {
        if (this.transform.childCount < 1)
        {
            InitializeSlots();
        }
        else
        {
            for (int a = 0; a < ModuleSlotUIMap.GetLength(0); a++)
            {
                for (int b = 0; b < ModuleSlotUIMap.GetLength(1); b++)
                {
                    GameObject.Destroy(ModuleSlotUIMap[a, b].gameObject);
                }
            }
        }
        InitializeSlots();
    }

    //刷新装备栏UI
    public void UpdateSlotsShow()
    {
        ResetSlots();
        UpdateSlots();
        UpdateModuleShow();
    }
}
