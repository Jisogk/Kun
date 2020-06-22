using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributionPanel : MonoBehaviour
{
    public GameObject DistributeSwitchUI;
    public GameObject[,] DistributeSwitchUIMap;

    [SerializeField] private float UILength = 100f;
    [SerializeField] private float UIHeight = 100f;

    public Text statusText;

    // Use this for initialization
    void Awake()
    {
        InitializeSlots();
        PlayerManager.instance.PowerCal();

        //注册电力切换事件
        EventCenter.AddListener(EventCode.SwitchModule, UpdatePanel);
    }

    private void OnDestroy()
    {
        //取消注册电力切换事件
        EventCenter.RemoveListener(EventCode.SwitchModule, UpdatePanel);
    }

    void Start()
    {
        UpdatePanel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeSlots()
    {
        int raw = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(0);
        int rol = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(1);

        DistributeSwitchUIMap = new GameObject[raw, rol];

        Vector3 origin = new Vector3((0 - UILength * raw / 2), (0 - UILength * rol / 2), 0f);

        for (int a = 0; a < raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                DistributeSwitchUIMap[a, b] = Instantiate<GameObject>(DistributeSwitchUI, this.transform);
                DistributeSwitchUIMap[a, b].GetComponent<RectTransform>().anchoredPosition = origin + new Vector3(0.5f * UILength + UILength * a, 0.5f * UIHeight + UIHeight * b, 0f);
                DistributeSwitchUIMap[a, b].GetComponent<DistributeSwitch>().rawIndex = a;
                DistributeSwitchUIMap[a, b].GetComponent<DistributeSwitch>().rolIndex = b;

                if (PlayerManager.instance.ModuleMap[a, b].type.Equals(ModuleType.None) || PlayerManager.instance.ModuleMap[a, b].type.Equals(ModuleType.Locked))
                {
                    DistributeSwitchUIMap[a, b].SetActive(false);
                }
            }
        }
    }

    public void UpdatePanel()
    {
        //int curIndex = PlayerManager.instance.currentPlanIndex;
        int raw = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(0);
        int rol = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(1);
        
        for (int a = 0; a < raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                DistributeSwitchUIMap[a, b].GetComponent<DistributeSwitch>().UpdateUI();
            }
        }
    }

    /*public void UpdateStatusText()
    {
        int totalElecRestore = 0;
        int totalElecContribution = 0;
        int computingUsing = 0;
        int computingProduction = 0;
        for (int i = 0; i < ModuleInfo.TOT; i++)
        {
        }
        statusText.text = string.Format("Electricity: {0}/{0} {1:+0;-#}/s\nComputation: {2}/{3}",
        totalElecRestore, totalElecContribution, computingUsing, computingProduction);
    }*/
}
