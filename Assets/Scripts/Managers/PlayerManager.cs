using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance = null;

    [Header("Status")]
    public RobotDiagram CurrentDiagram;
    public Module[,] ModuleMap;
    public int currentPlanIndex;
    public List<bool[,]> planList;
    [SerializeField] private int totalElecRestore = 0;
    [SerializeField] private int totalElecContribution = 0;
    [SerializeField] private int computingUsing = 0;
    [SerializeField] private int computingProduction = 0;

    [Header("Storage")]
    public List<Module> ModuleList;
    public List<RobotDiagram> DiagramList;

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

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void initCurDiagram()
    {
        CurrentDiagram = StorageManager.instance.GetDiagram(0);
    }

    public bool CheckCore()
    {
        int count = 0;
        foreach (Module a in ModuleMap)
        {
            if (a.type.Equals(ModuleType.Core))
            {
                count += 1;
            }
        }

        if (count != 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void CheckPowerPlan()
    {

    }

    public void SwitchModule(int prevRawIndex, int prevRolIndex, int enterRawIndex, int enterRolIndex)
    {
        if (prevRawIndex < ModuleMap.GetLength(0) && enterRawIndex < ModuleMap.GetLength(0) && prevRolIndex < ModuleMap.GetLength(1) && enterRolIndex < ModuleMap.GetLength(1))
        {
            Module bufferModule = ModuleMap[enterRawIndex, enterRolIndex];
            ModuleMap[enterRawIndex, enterRolIndex] = ModuleMap[prevRawIndex, prevRolIndex];
            ModuleMap[prevRawIndex, prevRolIndex] = bufferModule;
        }
        else
        { }
    }


}
