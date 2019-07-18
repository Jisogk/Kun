using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributionPanel : MonoBehaviour
{
    public Text statusText;

    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < ModuleInfo.TOT; i++)
        {
            GameObject slot = transform.GetChild(i).gameObject;
            slot.GetComponent<DistributeSwitch>().position = i;
            /*if (ModuleInfo.instance.moduleList[i].type != ModuleType.None)
            {
                string name = ModuleInfo.instance.modImgMap[ModuleInfo.instance.moduleList[i].type];
                slot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + name);
            }
            else
            {
                slot.GetComponent<Image>().enabled = false;
            }*/
        }
    }

    void Start()
    {
        UpdatePanel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePanel()
    {
        int curIndex = PlanManager.instance.currentPlanIndex;
        for (int i = 0; i < ModuleInfo.TOT; i++)
        {
            GameObject slot = transform.GetChild(i).gameObject;
            slot.GetComponent<DistributeSwitch>().SetOpen(PlanManager.instance.planList[curIndex][i]);
        }
        UpdateStatusText();
    }

    public void UpdateStatusText()
    {
        int totalElecRestore = 0;
        int totalElecContribution = 0;
        int computingUsing = 0;
        int computingProduction = 0;
        for (int i = 0; i < ModuleInfo.TOT; i++)
        {
            //Module m = ModuleInfo.instance.moduleList[i];
            /*if (transform.GetChild(i).gameObject.GetComponent<DistributeSwitch>().isOpen)
            {
                totalElecRestore += m.elecRestore;
                totalElecContribution += m.elecContribution;
                if (m.computingContribution > 0)
                    computingProduction += m.computingContribution;
                else
                    computingUsing -= m.computingContribution;
            }*/
        }
        statusText.text = string.Format("Electricity: {0}/{0} {1:+0;-#}/s\nComputation: {2}/{3}",
        totalElecRestore, totalElecContribution, computingUsing, computingProduction);
    }
}
