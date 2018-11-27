using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

  //public float speed;
  public float bulletSpeed;
  public float jumpheight;
  public float climbheight;
  public float slipspeed;
  //public float boostspeed;
  public GameObject bullet;
  public GameObject emptyModule;
  public GameObject weaponModule;
  public float gamma;
  public float alpha;

  private Rigidbody2D body;
  private CapsuleCollider2D capcollider;
  private bool toRight;
  private int onAirDirection;

  private List<GameObject> bulletList;

  private float distToGround;
  private LayerMask groundLayer;

  private float totalElecRestore = 0;
  private float currentElecRestore = 0;
  private int totalElecContribution = 0;
  private int totalComputingProduction = 0;
  private int totalComputingUsing = 0;
  private int totalMaxHp = 0;
  private int totalHp = 0;
  private int totalLoad = 0;
  private int workingModuleCount = 0;
  private float squarePower = 0;
  private float totalOmega = 0;
  private float velocity;
  private float pushForce;

  public GameObject playerPanel;
  private bool playerPanelActive;
  private GridLayoutGroup moduleLayout;

  private Text electricText;
  private Text computationText;
  private Text hpText;
  public GameObject gameOverText;
  private bool gameOver;

  private float timer;
  private float beginPushTime;

  private bool[] curPlan;
  private Vector3[] modulePostion;

	// Use this for initialization
	void Start () {
    gameOver = false;
    body = GetComponent<Rigidbody2D>();
    toRight = true;
    groundLayer = LayerMask.GetMask("Ground");
    onAirDirection = 0;
    bulletList = new List<GameObject>();
    capcollider = GetComponent<CapsuleCollider2D>();
    GameObject modulePanel = playerPanel.transform.Find("ModulePanel").gameObject;
    moduleLayout = modulePanel.GetComponent<GridLayoutGroup>();
    PlanManager.instance.currentPlanIndex = 0;
    modulePostion = new Vector3[ModuleInfo.TOT];
    modulePostion[0] = new Vector3(-1.5f, 0f, -1f);
    for(int i = 0; i < 3; i ++)
    {
      float yOffset = -i * 1.5f;
      for(int j = 0; j < 3; j ++)
      {
        float xOffset = j * 1.5f;
        modulePostion[i * 3 + j] = modulePostion[0] + new Vector3(xOffset, yOffset, 0f);
      }
    }

    for(int i = 0; i < ModuleInfo.TOT; i ++)
    {
      Module m = ModuleInfo.instance.moduleList[i];
      if (m.type != ModuleType.None)
      {
        totalHp += m.hp;
        totalMaxHp += m.maxHp;
        totalElecRestore += m.elecRestore;
        totalLoad += m.load;

        Image img = moduleLayout.transform.GetChild(i).gameObject.GetComponent<Image>();
        Sprite spr = Resources.Load<Sprite>("Images/" + ModuleInfo.instance.modImgMap[m.type]);
        img.sprite = spr;
        workingModuleCount++;

        GameObject moduleObj;
        if(m.type == ModuleType.Weapon)
        {
          //moduleObj = Instantiate(weaponModule, modulePostion[i], Quaternion.identity) as GameObject;
          moduleObj = Instantiate(weaponModule, transform) as GameObject;
        }
        else
        {
          //moduleObj = Instantiate(emptyModule, modulePostion[i], Quaternion.identity) as GameObject;
          moduleObj = Instantiate(emptyModule, transform) as GameObject;
        }
        //moduleObj.transform.parent = transform;
        moduleObj.transform.position = transform.TransformPoint(modulePostion[i]);
        m.obj = moduleObj;
        m.obj.GetComponent<ModuleObj>().index = i;
      }
    }
    currentElecRestore = totalElecRestore;
    body.mass = totalLoad;

    electricText = playerPanel.transform.Find("ElectricText").gameObject.GetComponent<Text>();
    computationText = playerPanel.transform.Find("ComputationText").gameObject.GetComponent<Text>();
    hpText = playerPanel.transform.Find("HpText").gameObject.GetComponent<Text>();

    ChangePlan();

    timer = 0;
  }

  void UpdateText()
  {
    electricText.text = string.Format("Electricity: {0}/{1} {2:+0;-#}/s", currentElecRestore, totalElecRestore, totalElecContribution);
    computationText.text = string.Format("Computation: {0}/{1}", totalComputingUsing, totalComputingProduction);
    float hpPercentage = totalMaxHp.Equals(0) ? 0 : (float)totalHp / (float)totalMaxHp;
    hpText.text = string.Format("HP: {0}/{1}({2:P})", totalHp, totalMaxHp, hpPercentage);
  }
	
	// Update is called once per frame
	void Update () {
    if (gameOver) return;
    //Debug.Log(moduleList[0].hp);
    timer += Time.deltaTime;
    if(timer > 1)
    {
      currentElecRestore += totalElecContribution;
      if (currentElecRestore > totalElecRestore)
      {
        currentElecRestore = totalElecRestore;
      }
      else if (currentElecRestore < 0)
        currentElecRestore = 0;
      UpdateText();
      timer = 0;
    }

    if(Input.GetKeyDown(KeyCode.N))
    {
      playerPanelActive = !playerPanelActive;
      playerPanel.SetActive(!playerPanelActive);
    }

    if(Input.GetKeyDown(KeyCode.Q))
    {
      if(PlanManager.instance.SwitchToPrevPlan())
      {
        ChangePlan();
      }
    }
    else if(Input.GetKeyDown(KeyCode.E))
    {
      if (PlanManager.instance.SwitchToNextPlan())
      {
        ChangePlan();
      }
    }

  }

  void FixedUpdate()
  {
    if (gameOver) return;
    if(currentElecRestore <= 0)
    {
      currentElecRestore = 0;
      return;
    }
    if(totalComputingUsing > totalComputingProduction)
    {
      return;
    }
    MyCheckMove();
    for(int i = 0; i < ModuleInfo.TOT; i ++)
    {
      if(curPlan[i] && ModuleInfo.instance.moduleList[i].type == ModuleType.Weapon)
      {
        CheckShoot(ModuleInfo.instance.moduleList[i] as WeaponModule);
      }
    }
    //Debug.Log(body.velocity);
  }

  void MyCheckMove()
  {
    bool isGroundedFlag = IsGrounded();
    bool isNearWallFlag = isNearWall();
    if(isGroundedFlag)
    {
      if(!body.velocity.y.Equals(0) && body.velocity.y < -50)
      {
        int damage = (int)((-50 - body.velocity.y) * 0.25);
        for(int i = 0; i < ModuleInfo.TOT; i ++)
        {
          Module m = ModuleInfo.instance.moduleList[i];
          if(m.type != ModuleType.None)
          {
            HurtModule(i, damage);
          }
        }
      }
    }

    float hori = Input.GetAxis("Horizontal");
    float vert = Input.GetAxis("Vertical");
    bool horiMoving = false;
    if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
    {
      horiMoving = true;
    }

    if (horiMoving && (hori > 0) ^ toRight)
    {
      TurnAround();
    }

    if(Input.GetKeyDown(KeyCode.LeftShift) || Time.time - beginPushTime <= 0.5)
    {
      if(Input.GetKeyDown(KeyCode.LeftShift))
      {
        if(currentElecRestore < totalOmega)
        {
          return;
        }
        currentElecRestore -= totalOmega;
        beginPushTime = Time.time;
      }
      Vector2 dir = new Vector2(hori, vert).normalized;
      body.AddForce(pushForce * dir);
      return;
    }

    if(!isGroundedFlag)
    {
      //Debug.Log("OnAir");
      if(isNearWallFlag && horiMoving && (toRight == (hori > 0)))
      {
        if (Input.GetKeyDown(KeyCode.Space))
        {
          //transform.position += new Vector3(0f, climbheight);
          body.MovePosition(body.position + new Vector2(0f, climbheight));
          //Debug.Log("Climb");
        }
        else
        {
          //transform.position -= new Vector3(0f, -slipspeed * Time.deltaTime);
          //Debug.Log("slipping");
          body.MovePosition(body.position + new Vector2(0f, -slipspeed * Time.deltaTime));
          body.velocity = new Vector2(body.velocity.x, 0);
        }
      }
      else
      {
        //Debug.Log("onair");
        if(!horiMoving)
          transform.position += new Vector3(onAirDirection * velocity * Time.deltaTime, 0f);
        else
        {
          if(hori > 0)
          {
            transform.position += new Vector3(velocity * Time.deltaTime, 0f);
            onAirDirection = 1;
          }
          else
          {
            transform.position -= new Vector3(velocity * Time.deltaTime, 0f);
            onAirDirection = -1;
          }
        }
        //not sure what to do yet
      }
      return;
    }
    //Debug.Log("Grounded");
    body.velocity = new Vector2(0f, 0f);

    if (Input.GetKeyDown(KeyCode.Space))
    {
      //Debug.Log("Hah?");
      if (isNearWallFlag && horiMoving && (toRight == (hori > 0)))
      {
        //transform.position += new Vector3(0f, climbheight);
        body.MovePosition(body.position + new Vector2(0f, climbheight));
      }
      else
      {
        body.velocity += new Vector2(0f, jumpheight);
        if (hori > 0)
        {
          onAirDirection = 1;
        }
        else if (hori < 0)
        {
          onAirDirection = -1;
        }
        else
          onAirDirection = 0;
      }
    }
    else if (horiMoving)
    {
      if (hori > 0)
      {
        onAirDirection = 1;
        body.MovePosition(body.position + new Vector2(velocity * Time.deltaTime, 0f));
      }
      else
      {
        onAirDirection = -1;
        body.MovePosition(body.position - new Vector2(velocity * Time.deltaTime, 0f));
      }
    }
    else
      onAirDirection = 0;
  }
  
  void TurnAround()
  {
    Vector3 originScale = transform.localScale;
    transform.localScale = new Vector3(-originScale.x, originScale.y, originScale.z);
    toRight = !toRight;
  }

  /*
  void CheckMove()
  {
    Vector2 bodypos = body.position;
    bool isGroundedFlag = IsGrounded();
    bool isNearWallFlag = isNearWall();
    //Debug.Log("isGroundedFlag:" + isGroundedFlag);
    //Debug.Log("isNearWallFlag:" + isNearWallFlag);
    //Debug.Log(Time.time);

    float hori = Input.GetAxis("Horizontal");
    float vert = Input.GetAxis("Vertical");

    if (!hori.Equals(0) && (hori > 0) ^ toRight)
    {
      Vector3 originScale = transform.localScale;
      transform.localScale = new Vector3(-originScale.x, originScale.y, originScale.z);
      toRight = !toRight;
    }

    if (Input.GetKey(KeyCode.LeftShift))
    {
      Vector2 newV = new Vector2(hori, vert);
      newV = newV.normalized * boostspeed;
      body.velocity = newV;
      return;
    }


    if (isNearWallFlag)
    {
      body.gravityScale = 0;
      //transform.position += new Vector3(0, -slipspeed, 0);
      //body.MovePosition(bodypos + new Vector2(0, -slipspeed));
      body.velocity = new Vector2(0, -slipspeed);
    }
    else
    {
      body.gravityScale = 3;
    }
    if (!isGroundedFlag && !isNearWallFlag) return;
    //Debug.Log("Grounded");

    
    if(Input.GetKey(KeyCode.A))
    {
      body.velocity = new Vector2(-speed, body.velocity.y);
      //if(!isNearWallFlag)
        //body.MovePosition(bodypos - new Vector2(speed * Time.deltaTime, 0));
    }

    if(Input.GetKey(KeyCode.D))
    {
      body.velocity = new Vector2(speed, body.velocity.y);
      //if (!isNearWallFlag)
        //body.MovePosition(bodypos + new Vector2(speed * Time.deltaTime, 0));
    }
    
    if(hori.Equals(0) && isGroundedFlag)
    {
      body.velocity = new Vector2(0, body.velocity.y);
      //body.MovePosition(bodypos);
    }
    
    if (Input.GetKeyDown(KeyCode.Space))
    {
      //Debug.Log("getSpace");
      //Debug.Log(isNearWallFlag);
      if (isNearWallFlag)
      {
        body.MovePosition(bodypos + new Vector2(0, climbheight));
      }
      //body.AddForce(new Vector2(0, upForce));
      else if (body.velocity.y.Equals(0))
      {
        body.velocity = new Vector2(body.velocity.x, jumpheight);
      }
    }
  }
  */

  void CheckShoot(WeaponModule wm)
  {
    Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector3 bulletpos = wm.obj.GetComponent<BoxCollider2D>().bounds.center;
    mousepos.z = bulletpos.z = -1;
    Vector3 direction = (mousepos - bulletpos).normalized;
    Transform gunTransform = wm.obj.transform.GetChild(0);
    float gunAngle = gunTransform.eulerAngles.z;
    if(!toRight)
    {
      if(gunAngle < 180)
      {
        gunAngle += 180;
      }
      else
      {
        gunAngle -= 180;
      }
    }
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    gunTransform.RotateAround(bulletpos, Vector3.back, gunAngle - angle);
    if (Input.GetMouseButton(0) && Time.time - wm.lastShootTime >= wm.shootingSpeed)
    {
      GameObject bul = Instantiate(bullet, bulletpos, Quaternion.identity) as GameObject;
      bul.name = "playerbullet";
      Bullet bulscript = bul.GetComponent<Bullet>();
      bulscript.shooter = gameObject;
      //bulletList.Add(bul);
      Rigidbody2D bulrig = bul.GetComponent<Rigidbody2D>();
      bulrig.velocity = direction * bulletSpeed;
      //Destroy(bul, 5);
      wm.lastShootTime = Time.time;
    }
  }

  private void OnCollisionStay2D(Collision2D collision)
  {

    if (collision.gameObject.name == "Block1(Clone)" || collision.gameObject.name == "Wall1(Clone)")
    {
      //Debug.Log(collision.transform.position);
    }
    
  }

  bool IsGrounded()
  {
    bool res = false;
    Bounds bd = capcollider.bounds;
    Vector2[] colliderpos = new Vector2[3];
    colliderpos[0] = bd.center;
    colliderpos[1] = new Vector2(bd.min.x, bd.center.y);
    colliderpos[2] = new Vector2(bd.max.x, bd.center.y);

    foreach(Vector2 pos in colliderpos)
    {
      RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, bd.extents.y + 0.05f, groundLayer);
      if (hit.collider != null)
      {
        res = true;
        break;
      }
    }
    return res;
  }

  bool isNearWall()
  {
    bool res = false;
    Bounds bd = capcollider.bounds;
    Vector2[] colliderpos = new Vector2[3];
    Vector2 direction;
    colliderpos[0] = bd.center;
    colliderpos[1] = new Vector2(bd.center.x, bd.min.y + 0.05f);
    colliderpos[2] = new Vector2(bd.center.x, bd.max.y - 0.05f);
    if (toRight)
    {
      direction = Vector2.right;
    }
    else
    {
      direction = Vector2.left;
    }

    foreach (Vector2 pos in colliderpos)
    {
      RaycastHit2D hit = Physics2D.Raycast(pos, direction, bd.extents.x + 0.05f, groundLayer);
      if (hit.collider != null)
      {
        res = true;
        break;
      }
    }
    //Debug.Log(res);
    return res;
  }

  /*
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.name == "testbullet")
    {
      if (workingModuleCount <= 0) return;
      System.Random rand = new System.Random();
      int damagedNumber = rand.Next(workingModuleCount);

      int k = 0;
      for (int i = 0; i < 9; i ++)
      {
        Module m = ModuleInfo.instance.moduleList[i];
        if(m != null && m.hp > 0)
        {
          if (k == damagedNumber)
          {
            m.hp -= 5;
            totalHp -= 5;
            if (m.hp <= 0)
            {
              m.hp = 0;
              RemoveModule(i);
            }
            UpdateModule(i);
            break;
          }
          else
            k++;
        }
      }
    }
  }
  */

  public void HurtModule(int damagedNumber, int damage)
  {
    Module m = ModuleInfo.instance.moduleList[damagedNumber];
    m.hp -= damage;
    totalHp -= damage;
    if (m.hp <= 0)
    {
      m.hp = 0;
      RemoveModule(damagedNumber);
    }
    UpdateModule(damagedNumber);
  }

  void UpdateModule(int damagedNumber)
  {
    Module m = ModuleInfo.instance.moduleList[damagedNumber];
    bool isOpen = curPlan[damagedNumber];
    Image img = moduleLayout.transform.GetChild(damagedNumber).gameObject.GetComponent<Image>();
    Color c;
    float hpPercent = (float)(m.hp) / (float)(m.maxHp);
    if (hpPercent > 0.66)
    {
      c = Color.green;
    }
    else if (hpPercent > 0.33)
    {
      c = Color.yellow;
    }
    else if (hpPercent > 0)
    {
      c = Color.red;
    }
    else
      c = Color.black;
    if (!isOpen)
    {
      float h, s, v;
      Color.RGBToHSV(c, out h, out s, out v);
      v = v / 2.0f;
      c = Color.HSVToRGB(h, s, v);
    }
    img.color = c;
  }

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
    if(m.type == ModuleType.Core)
    {
      gameOverText.SetActive(true);
      gameOver = true;
    }
    m.type = ModuleType.None;
    UpdateText();
  }
}
