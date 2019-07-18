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
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitializeSlots()
    {
        int raw = PlayerStatus.instance.CurrentDiagram.Diagram.GetLength(0);
        int rol = PlayerStatus.instance.CurrentDiagram.Diagram.GetLength(1);

        ModuleSlotUIMap = new GameObject[raw,rol];

        for(int a=0;a<raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                ModuleSlotUIMap[a, b] = Instantiate<GameObject>(moduleSlot,this.transform);
                ModuleSlotUIMap[a, b].GetComponent<RectTransform>().anchoredPosition = new Vector3(0.5f* UILength + UILength * a, 0.5f* UIHeight + UIHeight * b, 0f);
                ModuleSlotUIMap[a, b].GetComponent<ModuleSlot>().rawIndex = a;
                ModuleSlotUIMap[a, b].GetComponent<ModuleSlot>().rolIndex = b;
            }
        }
    }

    public void UpdateSlots()
    {
        int raw = PlayerStatus.instance.CurrentDiagram.Diagram.GetLength(0);
        int rol = PlayerStatus.instance.CurrentDiagram.Diagram.GetLength(1);

        for (int a = 0; a < raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                if (PlayerStatus.instance.CurrentDiagram.Diagram[a, b] == 1)
                {
                    ModuleSlotUIMap[a, b].GetComponent<DragAndDrop>().moduleType = ModuleType.None;
                }
                else
                {
                    ModuleSlotUIMap[a, b].GetComponent<DragAndDrop>().moduleType = ModuleType.Locked;
                    ModuleSlotUIMap[a, b].SetActive(false);
                }
            }
        }
    }

    public void UpdateModuleShow()
    {
        int raw = PlayerStatus.instance.CurrentDiagram.Diagram.GetLength(0);
        int rol = PlayerStatus.instance.CurrentDiagram.Diagram.GetLength(1);

        for (int a = 0; a < raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                ModuleSlotUIMap[a, b].GetComponent<ModuleSlot>().UpdateUI();
            }
        }
    }

}
