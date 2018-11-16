using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DistributionPageButton : MonoBehaviour {

  public Text planNumberText;
  public GameObject distributionPanel;

	// Use this for initialization
	void Awake () {
    if (name == "DeleteButton")
      gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void NewPlanClick()
  {
    PlanManager.instance.NewPlan();
    int planCount = PlanManager.instance.GetPlanCount();
    planNumberText.text = "方案" + planCount;
    PlanManager.instance.currentPlanIndex = planCount - 1;
    distributionPanel.GetComponent<DistributionPanel>().UpdatePanel();
    transform.parent.Find("DeleteButton").gameObject.SetActive(true);
  }

  public void SavePlanClick()
  {
    bool[] plan = new bool[ModuleInfo.TOT];
    for(int i = 0; i < ModuleInfo.TOT; i ++)
    {
      GameObject slot = distributionPanel.transform.GetChild(i).gameObject;
      DistributeSwitch distributeSwitch = slot.GetComponent<DistributeSwitch>();
      plan[i] = distributeSwitch.isOpen;
    }
    PlanManager.instance.SavePlan(PlanManager.instance.currentPlanIndex, plan);
  }

  public void DeletePlanClick()
  {
    PlanManager.instance.DeletePlan(PlanManager.instance.currentPlanIndex);
    PlanManager.instance.currentPlanIndex --;
    if (PlanManager.instance.currentPlanIndex < 0)
      PlanManager.instance.currentPlanIndex++;
    planNumberText.text = "方案" + (PlanManager.instance.currentPlanIndex + 1);
    distributionPanel.GetComponent<DistributionPanel>().UpdatePanel();
    if (name == "DeleteButton" && PlanManager.instance.GetPlanCount() <= 1)
    {
      gameObject.SetActive(false);
    }
  }

  public void NextSceneClick()
  {
    SceneManager.LoadScene("Maingame");
  }

  public void PrevPlanClick()
  {
    PlanManager.instance.SwitchToPrevPlan();
    planNumberText.text = "方案" + (PlanManager.instance.currentPlanIndex + 1);
    distributionPanel.GetComponent<DistributionPanel>().UpdatePanel();
  }

  public void NextPlanClick()
  {
    PlanManager.instance.SwitchToNextPlan();
    planNumberText.text = "方案" + (PlanManager.instance.currentPlanIndex + 1);
    distributionPanel.GetComponent<DistributionPanel>().UpdatePanel();
  }
}
