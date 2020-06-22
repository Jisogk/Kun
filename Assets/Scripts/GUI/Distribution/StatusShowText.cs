using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusShowText : MonoBehaviour
{
    public Text StatusText;
    public Text PlanNumber;

    // Use this for initialization
    void Awake()
    {
        StatusText = GetComponent<Text>();
        initStatusText();
        EventCenter.AddListener<int, int, int, int>(EventCode.StatusChanged, UpdateStatusText);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<int, int, int, int>(EventCode.StatusChanged, UpdateStatusText);
    }

    public void UpdateStatusText(int totalElecRestore, int totalElecContribution, int computingUsing, int computingProduction)
    {
        StatusText.text = string.Format("Electricity: {0}/{0} {1:+0;-#}/s\nComputation: {2}/{3}",
        totalElecRestore, totalElecContribution, computingUsing, computingProduction);
        PlanNumber.text = string.Format("方案 {0}", PlayerManager.instance.currentPlanIndex+1);
    }

    public void initStatusText()
    {
        StatusText.text = string.Format("Electricity: {0}/{0} {1:+0;-#}/s\nComputation: {2}/{3}",
        PlayerManager.instance.totalElecRestore, PlayerManager.instance.totalElecContribution, PlayerManager.instance.computingUsing, PlayerManager.instance.computingProduction);
        PlanNumber.text = string.Format("方案 {0}", PlayerManager.instance.currentPlanIndex + 1);
    }
}
