using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiagramShowUI : MonoBehaviour {

    public GameObject DiagramShowGrid;
    public GameObject[,] DiagramShowGridMap;
    public int DiagramIndex = 0;

    [SerializeField] private float UILength = 20f;
    [SerializeField] private float UIHeight = 20f;
    [SerializeField] private float UIPadding = 5f;

    // Use this for initialization
    void Awake()
    {
        UpdateSlots();
        //UpdateModuleShow();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //创建并排列格子
    public void InitializeSlots()
    {
        int raw = PlayerManager.instance.GetDiagram(DiagramIndex).Diagram.GetLength(0);
        int rol = PlayerManager.instance.GetDiagram(DiagramIndex).Diagram.GetLength(1);

        Vector3 origin = new Vector3((0 - UILength * raw / 2),
                                        (0 - UILength * rol / 2), 0f);

        DiagramShowGridMap = new GameObject[raw, rol];


        for (int a = 0; a < raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                DiagramShowGridMap[a, b] = Instantiate<GameObject>(DiagramShowGrid, this.transform);
                DiagramShowGridMap[a, b].GetComponent<RectTransform>().anchoredPosition = origin + new Vector3(0.5f * UILength + UILength * a, 0.5f * UIHeight + UIHeight * b, 0f);
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
            foreach (GameObject go in DiagramShowGridMap)
            {
                GameObject.Destroy(go.gameObject);
            }
            InitializeSlots();
        }
    }

    //更新背景的显示
    public void UpdateBackground()
    {
        float newHeight = DiagramShowGridMap.GetLength(1) * UIHeight + 2 * UIPadding;
        float newLength = DiagramShowGridMap.GetLength(0) * UILength + 2 * UIPadding;

        this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newLength);
        this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

        if (DiagramIndex == PlayerManager.instance.CurrentDiagramIndex)
        {
            this.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        }
        else
        {
            this.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
    }

    //更新UI的显示
    public void UpdateSlots()
    {
        ResetSlots();

        int raw = 0;
        int rol = 0;

        if (DiagramIndex < PlayerManager.instance.GetDiagramCount())
        {
            raw = PlayerManager.instance.GetDiagram(DiagramIndex).Diagram.GetLength(0);
            rol = PlayerManager.instance.GetDiagram(DiagramIndex).Diagram.GetLength(1);

            for (int a = 0; a < raw; a++)
            {
                for (int b = 0; b < rol; b++)
                {
                    if (PlayerManager.instance.GetDiagram(DiagramIndex).Diagram[a, b] == 0)
                    {
                        DiagramShowGridMap[a, b].SetActive(false);
                    }
                }
            }
        }

        UpdateBackground();
    }

    public void OnChangeDiagram()
    {
        EventCenter.Broadcast<string, WarningMsg.TargetMethod>(EventCode.ShowWarningMsg, "更换设计图将重置能源方案与已装备模块\n是否继续？", ChangeDiagram);
    }

    public void ChangeDiagram()
    {
        EventCenter.Broadcast<int>(EventCode.SetNewDiagram, DiagramIndex);
    }
}
