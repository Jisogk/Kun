using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleInfo : MonoBehaviour {

  public static ModuleInfo instance = null;
  [HideInInspector]
  public Module[] moduleList;
  public const int TOT = 9;

  public Dictionary<ModuleType, string> modImgMap;

  // Use this for initialization
  void Awake () {
		if(instance == null)
    {
      instance = this;
    }
    else if(instance != this)
    {
      Destroy(gameObject);
    }

    DontDestroyOnLoad(gameObject);

    moduleList = new Module[TOT];
    modImgMap = new Dictionary<ModuleType, string>();
    modImgMap.Add(ModuleType.Core, "001");
    modImgMap.Add(ModuleType.Energy, "002");
    modImgMap.Add(ModuleType.Battery, "003");
    modImgMap.Add(ModuleType.Armor, "004");
    modImgMap.Add(ModuleType.Computation, "005");
    modImgMap.Add(ModuleType.Weapon, "007");
    modImgMap.Add(ModuleType.Power, "009");
    modImgMap.Add(ModuleType.Repair, "010");
    modImgMap.Add(ModuleType.None, "");
    modImgMap.Add(ModuleType.Locked, "");
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  public bool checkModules()
  {
    GameObject panel = GameObject.FindWithTag("ModuleSlotPanel");
    if (panel == null)
    {
      Debug.Log("panel not found");
      return false;
    }

    int coreCount = 0;
    for(int i = 0; i < TOT; i ++)
    {
      GameObject slot = panel.transform.GetChild(i).gameObject;
      moduleList[i] = Module.sample(slot.GetComponent<DragAndDrop>().moduleType);
      if (moduleList[i] != null && moduleList[i].type == ModuleType.Core)
        coreCount++;
    }

    return coreCount == 1;
  }
}
