using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance = null;
    //public ModuleSlotsUI mSlots;
    //public ModuleStorageUI mStorage;
    //private GameObject icon;
    #region 变量

    [Header("Status")]
    public RobotDiagram CurrentDiagram;
    public int CurrentDiagramIndex;
    public Module[,] ModuleMap;
    public int currentPlanIndex;
    public bool[,] elcPlan;
    [SerializeField] public int totalElecRestore = 0;
    [SerializeField] public int totalElecContribution = 0;
    [SerializeField] public int computingUsing = 0;
    [SerializeField] public int computingProduction = 0;

    [Header("Storage")]
    public List<Module> ModuleList;
    public List<RobotDiagram> DiagramList;
    public List<bool[,]> planList;

    #endregion

    // Use this for initialization
    void Awake ()
    {
        //单例模式
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //各种初始化
        DiagramList = new List<RobotDiagram>();
        ModuleList = new List<Module>();
        planList = new List<bool[,]>();

        if (DiagramList.Count == 0 || ModuleList.Count == 0)
        {
            initStandardSet();
        }        
        initCurDiagram();
        initModuleMap();
        InitPlanList();

        //此处注册各种监听事件
        EventCenter.AddListener<int>(EventCode.SetNewDiagram, ChangeDiagram); //监听切换成新设计图的事件
        EventCenter.AddListener(EventCode.OnModuleDestory, PowerCal); //监听模块被摧毁时的事件
    }

    private void OnDestroy()
    {
        //取消注册事件监听
        EventCenter.RemoveListener<int>(EventCode.SetNewDiagram, ChangeDiagram);
        EventCenter.RemoveListener(EventCode.OnModuleDestory, PowerCal);
    }

    // Update is called once per frame
    void Update () {

    }

    //初始时建立一个标准套件
    //包含一个3*3的设计图以及所有示例模块
    private void initStandardSet()
    {
        AddDiagram(new RobotDiagram(3, 3, "Perfect"));

        AddModule(NewModule(ModuleType.Core));
        AddModule(NewModule(ModuleType.Energy));
        AddModule(NewModule(ModuleType.Computation));
        AddModule(NewModule(ModuleType.Power));
        AddModule(NewModule(ModuleType.Armor));
        AddModule(NewModule(ModuleType.Weapon));
        AddModule(NewModule(ModuleType.Battery));
    }

    #region 管理装备栏的方法

    //若当前未选中设计图，则初始化为第一个设计图
    private void initCurDiagram()
    {
        if (CurrentDiagram == null)
        {
            CurrentDiagramIndex = 0;
            CurrentDiagram = GetDiagram(CurrentDiagramIndex);
        }
    }

    //根据设计图初始化装备栏
    private void initModuleMap()
    {
        int raw = CurrentDiagram.Diagram.GetLength(0);
        int rol = CurrentDiagram.Diagram.GetLength(1);

        ModuleMap = new Module[raw, rol];

        for (int a = 0; a < raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                if (CurrentDiagram.Diagram[a, b] == 1)
                {
                    ModuleMap[a,b] = NewModule(ModuleType.None);
                }
                else
                {
                    ModuleMap[a, b] = NewModule(ModuleType.Locked);
                }
            }
        }

    }

    //检查核心数是否合理
    public bool CheckCore()
    {
        int coreCount = 0;   //核心计数

        //检查核心数
        foreach (Module a in ModuleMap)
        {
            if (a.type.Equals(ModuleType.Core))
            {
                coreCount += 1;
            }
        }

        //检查标准
        //有且仅有一个核心
        if (coreCount != 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //检查模块数是否合理
    public bool CheckModNum()
    {
        int moduleCount = 0; //模块计数
        
        //检查模块数
        foreach (Module a in ModuleMap)
        {
            if (a.type.Equals(ModuleType.None) || a.type.Equals(ModuleType.Locked))// “无”和“锁定”不算
            {
            }
            else
            {
                moduleCount += 1;
            }
        }

        //模块数不可为零
        if (moduleCount == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //交换两个模块的位置
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

    //获得模块
    public Module GetEquipedModule(int raw, int rol)
    {
        if ((raw < ModuleMap.GetLength(0)) && (rol < ModuleMap.GetLength(1)))
        {
            return ModuleMap[raw, rol];
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region 管理设计图的方法

    public void AddDiagram(RobotDiagram diagram)
    {
        DiagramList.Add(diagram);
    }

    //创建设计图方法和它的重构们
    //默认填充率
    public RobotDiagram NewDiagram(int length, int width)
    {
        return new RobotDiagram(length, width);
    }
    //可变填充率
    public RobotDiagram NewDiagram(int length, int width, float fillpercent)
    {
        return new RobotDiagram(length, width, fillpercent);
    }
    //完美填充
    public RobotDiagram NewDiagram(int length, int width, string cheatCode)
    {
        return new RobotDiagram(length, width, cheatCode);
    }

    public int GetDiagramCount()
    {
        return DiagramList.Count;
    }

    //获得设计图的引用
    public RobotDiagram GetDiagram(int index)
    {
        if (index < ModuleList.Count && DiagramList[index] != null)
        {
            return DiagramList[index];
        }
        else
        {
            return null;
        }
    }

    //切换设计图
    public void ChangeDiagram(int index)
    {
        if (index < DiagramList.Count)
        {
            CurrentDiagramIndex = index;
            CurrentDiagram = GetDiagram(CurrentDiagramIndex);
            initModuleMap();
            InitPlanList();
            EventCenter.Broadcast(EventCode.ChangeDiagram);
        }
    }

    #endregion

    #region 管理模块库的方法

    public Module NewModule(ModuleType type)
    {
        return Module.sample(type);
    }

    public void AddModule(Module module)
    {
        ModuleList.Add(module);
    }
    
    public int GetModuleCount()
    {
        return ModuleList.Count;
    }

    public Module GetModule(int index)
    {
        if (index < ModuleList.Count && ModuleList[index] != null)
        {
            return ModuleList[index];
        }
        else
        {
            return null;
        }
    }
    #endregion

    #region 管理电力方案的方法

    public void InitPlanList()
    {
        if (planList.Count >0)
        {
            planList = new List<bool[,]>();
        }
        NewPlan();
        currentPlanIndex = 0;
        elcPlan = GetCurrentPlan();
    }

    public void NewPlan()
    {
        int raw = CurrentDiagram.Diagram.GetLength(0);
        int rol = instance.CurrentDiagram.Diagram.GetLength(1);
        bool[,] newPlan = new bool[raw, rol];
        for (int i = 0; i < raw; i++)
        {
            for (int j = 0; j < rol; j++)
            {
                newPlan[i,j] = true;
            }
        }
        planList.Add(newPlan);
        SwitchToNextPlan();
    }

    public void SavePlan(int index, bool[,] plan)
    {
        planList[index] = plan;
    }

    public void DeletePlan(int index)
    {
        if (planList.Count > 1)
        {
            if (index > 0 && index < planList.Count)
            {
                SwitchToPrevPlan();
                planList.RemoveAt(index);
            }
            else if (index == 0)
            {
                planList.RemoveAt(index);
            }
        }
        EventCenter.Broadcast(EventCode.SwitchModule);
    }

    public void SetModuleOpen(int raw, int rol)
    {
        planList[currentPlanIndex][raw, rol] = !planList[currentPlanIndex][raw, rol];
        PowerCal();
        EventCenter.Broadcast(EventCode.SwitchModule);
    }

    public int GetPlanCount()
    {
        return planList.Count;
    }

    public bool[,] GetCurrentPlan()
    {
        return planList[currentPlanIndex];
    }

    public bool SwitchToPrevPlan()
    {
        if (currentPlanIndex > 0)
        {
            currentPlanIndex--;
            elcPlan = planList[currentPlanIndex];
            EventCenter.Broadcast(EventCode.SwitchModule);
            PowerCal();
            return true;
        }
        return false;
    }

    public void SwitchToFristPlan()
    {
        currentPlanIndex = 0;
        elcPlan = planList[currentPlanIndex];
        EventCenter.Broadcast(EventCode.SwitchModule);
        PowerCal();
    }

    public bool SwitchToNextPlan()
    {
        if (currentPlanIndex < planList.Count - 1)
        {
            currentPlanIndex++;
            elcPlan = planList[currentPlanIndex];
            EventCenter.Broadcast(EventCode.SwitchModule);
            PowerCal();
            return true;
        }
        return false;
    }

    //检查电力方案是否合理
    public void CheckPowerPlan()
    {

    }

    //计算当前电力方案的信息
    public void PowerCal()
    {
        totalElecRestore = totalElecContribution = computingProduction = computingUsing = 0;
        int raw = CurrentDiagram.Diagram.GetLength(0);
        int rol = CurrentDiagram.Diagram.GetLength(1);

        for (int a = 0; a < raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                if (GetCurrentPlan()[a, b])
                {
                    totalElecRestore += ModuleMap[a, b].ReturnElcRestore();
                    totalElecContribution += ModuleMap[a, b].ReturnElcContriution();
                    computingProduction += ModuleMap[a, b].ReturnComputContribution();
                    computingUsing += ModuleMap[a, b].ReturnComputUsing();
                }
            }
        }
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        EventCenter.Broadcast<int,int,int,int>(EventCode.StatusChanged, totalElecRestore,totalElecContribution, computingUsing, computingProduction);
    }
    #endregion

}
