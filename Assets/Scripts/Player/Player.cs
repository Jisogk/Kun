using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    public GameObject bullet;
    public GameObject emptyModule;
    public GameObject weaponModule;
    //public float bulletSpeed;
    public float damp; //移动的阻力
    private Vector2 moveDirection;
    public float gamma;
    public float alpha;
    public float ModObjLength;

    public PlayerAction action;
    public bool IsOperating;

    public GameObject[,] ModuleObjectMap;

    private List<GameObject> bulletList;

    private float distToGround;
    private LayerMask groundLayer;

    public float totalElecRestore = 0;
    public float currentElecRestore = 0;
    public int totalElecContribution = 0;
    public int testComputingProduction = 0;
    public int testComputingUsing = 0;
    private int totalMaxHp = 0;
    private int totalHp = 0;
    private int totalLoad = 0;
    private int workingModuleCount = 0;
    private float squarePower = 0;
    private float totalOmega = 0;
    private float velocity;
    private float pushForce;

    private float timer;
    private float beginPushTime;

    //private bool[] curPlan;
    //private Vector3[] modulePostion;

    // Use this for initialization
    void Awake()
    {
        //ModObjLength = 1.5f;
        timer = 0;

        action = GetComponent<PlayerAction>();

        InitModuleObject();

        //从PlayerManager中读取必要的数据
        totalElecRestore = PlayerManager.instance.totalElecRestore;
        totalElecContribution = PlayerManager.instance.totalElecContribution;
        //totalComputingProduction = PlayerManager.instance.computingProduction;
        //totalComputingUsing = PlayerManager.instance.computingUsing;

        //初始化数据
        currentElecRestore = totalElecRestore;

        //添加事件监听
        EventCenter.AddListener<int, int, int, int>(EventCode.StatusChanged, changeState);
        EventCenter.AddListener(EventCode.OnModuleDestory, ResetState);
    }

    private void OnDestroy()
    {
        //移除事件监听
        EventCenter.RemoveListener<int, int, int, int>(EventCode.StatusChanged, changeState);
        EventCenter.RemoveListener(EventCode.OnModuleDestory, ResetState);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(moduleList[0].hp);
        timer += Time.deltaTime;
        if (timer > 1)
        {
            currentElecRestore += totalElecContribution;
            if (currentElecRestore > totalElecRestore)
            {
                currentElecRestore = totalElecRestore;
            }
            else if (currentElecRestore < 0)
            {
                currentElecRestore = 0;
            }
            EventCenter.Broadcast<float, float, int>(EventCode.OnEnergyChange, currentElecRestore, totalElecRestore, totalElecContribution);
            timer = 0;
        }

        //Debug.Log(FuntionCheck());

        IsOperating = FuntionCheck();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerManager.instance.SwitchToPrevPlan();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerManager.instance.SwitchToNextPlan();
        }

        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveDirection = moveDirection.normalized;

    }

    void FixedUpdate()
    {
        if (IsOperating)
        {
            action.Move(moveDirection);
        }
    }

    //创建各个模块的Object
    private void InitModuleObject()
    {
        int raw = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(0);
        int rol = PlayerManager.instance.CurrentDiagram.Diagram.GetLength(1);

        Vector3 origin = new Vector3((0 - ModObjLength * raw / 2), (0 - ModObjLength * rol / 2), 0f);

        ModuleObjectMap = new GameObject[raw, rol];

        for (int a = 0; a < raw; a++)
        {
            for (int b = 0; b < rol; b++)
            {
                if (!PlayerManager.instance.ModuleMap[a, b].type.Equals(ModuleType.None))
                {
                    if (PlayerManager.instance.ModuleMap[a, b].type.Equals(ModuleType.Weapon))
                    {
                        ModuleObjectMap[a, b] = Instantiate<GameObject>(weaponModule, this.transform);
                        ModuleObjectMap[a, b].GetComponent<WeaponModObj>().GetWeapon(a, b);
                    }
                    else
                    {
                        ModuleObjectMap[a, b] = Instantiate<GameObject>(emptyModule, this.transform);
                    }
                    ModuleObjectMap[a, b].transform.localPosition = origin + new Vector3(0.5f * ModObjLength + ModObjLength * a, 0.5f * ModObjLength + ModObjLength * b, 0f);
                    ModuleObjectMap[a, b].GetComponent<ModuleObj>().rawIndex = a;
                    ModuleObjectMap[a, b].GetComponent<ModuleObj>().rolIndex = b;

                    ModuleObjectMap[a, b].GetComponent<ModuleObj>().Init();

                    if (PlayerManager.instance.ModuleMap[a, b].type.Equals(ModuleType.Core))
                    {
                        ModuleObjectMap[a, b].GetComponent<ModuleObj>().IsCore = true;
                    }
                }
            }
        }
    }

    //判断本机目前是否可以正常行动
    private bool FuntionCheck()
    {
        testComputingProduction = PlayerManager.instance.computingProduction;
        testComputingUsing = PlayerManager.instance.computingUsing;
        if (currentElecRestore <= 0)
        {
            currentElecRestore = 0;
            return false;
        }
        if (PlayerManager.instance.computingUsing > PlayerManager.instance.computingProduction)
        {
            return false;
        }

        return true;
    }

    private void changeState(int ElecRestore, int ElecContribution, int computingUsing, int computingProduction)
    {
        totalElecRestore = ElecRestore;
        totalElecContribution = ElecContribution;
    }

    private void ResetState()
    {
        PlayerManager.instance.PowerCal();
    }

    /*
    void ChangePlan()
    {
        curPlan = PlanManager.instance.GetCurrentPlan();

        totalElecContribution = 0;
        totalComputingProduction = 0;
        totalComputingUsing = 0;
        squarePower = 0;
        totalOmega = 0;

        for (int i = 0; i < ModuleInfo.TOT; i++)
        {
            Module m = ModuleInfo.instance.moduleList[i];
            bool isOpen = curPlan[i];
            if (m.type != ModuleType.None)
            {
                if (isOpen)
                {
                    totalElecContribution += m.elecContribution;
                    if (m.computingContribution > 0)
                        totalComputingProduction += m.computingContribution;
                    else
                        totalComputingUsing += (-m.computingContribution);
                    if (m.type == ModuleType.Power)
                    {
                        squarePower += (m as PowerModule).beta;
                        totalOmega += (m as PowerModule).omega;
                    }
                }

                UpdateModule(i);
            }
        }
        float power = Mathf.Sqrt(squarePower);
        velocity = gamma * power / (float)totalLoad;
        pushForce = alpha * power;
        UpdateText();
    }

    void RemoveModule(int damagedNumber)
    {
        Module m = ModuleInfo.instance.moduleList[damagedNumber];
        if (m.type == ModuleType.None)
            return;
        totalElecRestore -= m.elecRestore;
        if (currentElecRestore > totalElecRestore)
            currentElecRestore = totalElecRestore;
        //totalMaxHp -= m.maxHp;
        totalLoad -= m.load;
        body.mass = totalLoad;

        if (curPlan[damagedNumber])
        {
            if (m.computingContribution >= 0)
                totalComputingProduction -= m.computingContribution;
            else
                totalComputingUsing -= (-m.computingContribution);
            totalElecContribution -= m.elecContribution;
            if (m.type == ModuleType.Power)
            {
                squarePower -= (m as PowerModule).beta;
                totalOmega -= (m as PowerModule).omega;
                float power = Mathf.Sqrt(squarePower);
                velocity = gamma * power / (float)totalLoad;
                pushForce = alpha * power;
            }
        }
        Destroy(m.obj);
        workingModuleCount--;
        if (m.type == ModuleType.Core)
        {
            gameOverText.SetActive(true);
            gameOver = true;
        }
        m.type = ModuleType.None;
        UpdateText();
    }*/
}
