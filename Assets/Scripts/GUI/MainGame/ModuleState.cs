using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleState : MonoBehaviour
{
    public GameObject moduleSlot;
    public GameObject[,] ModuleSlotUIMap;

    public float EdgeWidth;

    [SerializeField] private float UILength = 60f;
    [SerializeField] private float UIHeight = 60f;

    // Use this for initialization
    void Awake()
    {
        InitializeSlots();
        UpdateSlots();
        UpdateModuleIcon();

        //监听事件
        EventCenter.AddListener<ModuleObj>(EventCode.OnModuleDamage, UpdateSlotState);
        EventCenter.AddListener(EventCode.OnModuleDestory, UpdateSlotsShow);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<ModuleObj>(EventCode.OnModuleDamage, UpdateSlotState);
        EventCenter.RemoveListener(EventCode.OnModuleDestory, UpdateSlotsShow);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //创建装备栏
    public void InitializeSlots()
    {
        int raw = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(0);
        int rol = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(1);

        this.GetComponent<RectTransform>().sizeDelta = new Vector2(raw * UILength + 2 * EdgeWidth, rol * UILength + 2 * EdgeWidth);

        Vector3 origin = new Vector3((0 - UILength * raw / 2), (0 - UILength * rol / 2), 0f);

        ModuleSlotUIMap = new GameObject[raw, rol];

        for (int a = 0; a < raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                ModuleSlotUIMap[a, b] = Instantiate<GameObject>(moduleSlot, this.transform);
                ModuleSlotUIMap[a, b].GetComponent<RectTransform>().anchoredPosition = origin + new Vector3(0.5f * UILength + UILength * a, 0.5f * UIHeight + UIHeight * b, 0f);
                ModuleSlotUIMap[a, b].GetComponent<ModuleStateSlot>().rawIndex = a;
                ModuleSlotUIMap[a, b].GetComponent<ModuleStateSlot>().rolIndex = b;
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
                if (PlayerManager.instance.ModuleMap[a, b].type.Equals(ModuleType.Locked)|| PlayerManager.instance.ModuleMap[a, b].type.Equals(ModuleType.None))
                {
                    ModuleSlotUIMap[a, b].SetActive(false);
                }
            }
        }
    }

    //统一刷新装备栏UI的图标
    public void UpdateModuleIcon()
    {
        foreach (GameObject go in ModuleSlotUIMap)
        {
            go.GetComponent<ModuleStateSlot>().UpdateUI();
            go.GetComponent<ModuleStateSlot>().UpdateFunction(1, 1, 1, 1);
        }
    }

    //刷新装备栏UI
    public void UpdateSlotsShow()
    {
        UpdateSlots();
        UpdateModuleIcon();
    }

    //更新特定装备栏颜色
    public void UpdateSlotState(ModuleObj modObj)
    {
        ModuleSlotUIMap[modObj.rawIndex, modObj.rolIndex].GetComponent<ModuleStateSlot>().UpdateState(modObj.CalHpPercent());
    }
}
