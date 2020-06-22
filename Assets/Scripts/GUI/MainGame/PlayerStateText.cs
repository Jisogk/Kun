using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateText : MonoBehaviour
{
    public Text ElecText;
    public Text ComputeText;
    public Text CoreHpText;

    public Module core;

    // Use this for initialization
    void Awake()
    {
        //从PlayerManager中初始化
        core = null;
        foreach (Module mod in PlayerManager.instance.ModuleMap)
        {
            if (mod.type.Equals(ModuleType.Core))
            {
                core = mod;
            }
        }
        CoreHpText.text = string.Format("Core Hp: {0}/{1}", core.hp, core.maxHp);
        
        ElecText.text = string.Format("Electricity: {0}/{1} {2}/s",
    PlayerManager.instance.totalElecRestore, PlayerManager.instance.totalElecRestore, PlayerManager.instance.totalElecContribution);
        ComputeText.text = string.Format("Computation: {0}/{1}", PlayerManager.instance.computingUsing, PlayerManager.instance.computingProduction);



        //添加监听
        EventCenter.AddListener<ModuleObj>(EventCode.OnModuleDamage, UpdateCoreHp);
        EventCenter.AddListener<float, float, int>(EventCode.OnEnergyChange, UpdateEnergyText);
        EventCenter.AddListener<int, int, int, int>(EventCode.StatusChanged, UpdateComputText);
    }

    private void OnDestroy()
    {
        //移除监听
        EventCenter.RemoveListener<ModuleObj>(EventCode.OnModuleDamage, UpdateCoreHp);
        EventCenter.RemoveListener<float, float, int>(EventCode.OnEnergyChange, UpdateEnergyText);
        EventCenter.RemoveListener<int, int, int, int>(EventCode.StatusChanged, UpdateComputText);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateCoreHp(ModuleObj CoreObj)
    {
        if (PlayerManager.instance.ModuleMap[CoreObj.rawIndex, CoreObj.rolIndex].type.Equals(ModuleType.Core))
        {
            CoreHpText.text = string.Format("Core Hp: {0}/{1}", CoreObj.currentHp, CoreObj.totalHp);
        }
    }

    public void UpdateEnergyText(float currentElecRestore, float totalElecRestore, int totalElecContribution)
    {
        ElecText.text = string.Format("Electricity: {0}/{1} {2}/s", currentElecRestore, totalElecRestore, totalElecContribution);
    }

    public void UpdateComputText(int ElecRestore, int ElecContribution, int computingUsing, int computingProduction)
    {
        ComputeText.text = string.Format("Computation: {0}/{1}", computingUsing, computingProduction);
    }
}
