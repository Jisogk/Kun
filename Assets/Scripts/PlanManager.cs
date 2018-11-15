using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanManager : MonoBehaviour {

  public static PlanManager instance;
  //[HideInInspector]
  public int currentPlanIndex;
  public List<bool[]> planList;

	// Use this for initialization
	void Awake () {
    if (instance == null)
    {
      instance = this;
    }
    else if (instance != this)
    {
      Destroy(gameObject);
    }
    DontDestroyOnLoad(gameObject);
    currentPlanIndex = 0;
    planList = new List<bool[]>();
    NewPlan();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void NewPlan()
  {
    bool[] newPlan = new bool[ModuleInfo.TOT];
    for(int i = 0; i < ModuleInfo.TOT; i ++)
    {
      newPlan[i] = true;
    }
    planList.Add(newPlan);
  }

  public void SavePlan(int index, bool[] plan)
  {
    planList[index] = plan;
  }

  public void DeletePlan(int index)
  {
    planList.RemoveAt(index);
  }

  public int GetPlanCount()
  {
    return planList.Count;
  }
}
