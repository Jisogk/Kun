using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

  public float speed;
  public float bulletSpeed;
  public float jumpheight;
  public float climbheight;
  public float slipspeed;
  public float boostspeed;
  public GameObject bullet;

  private Rigidbody2D body;
  private CapsuleCollider2D capcollider;
  private bool toRight;

  private List<GameObject> bulletList;

  private float distToGround;
  public LayerMask groundLayer;

  private Module[] moduleList;

  private int totalElecRestore = 0;
  private int currentElecRestore = 0;
  private int totalElecContribution = 0;
  private int totalComputingProduction = 0;
  private int totalComputingUsing = 0;
  private int totalMaxHp = 0;
  private int totalHp = 0;
  private int totalLoad = 0;
  private int workingModuleCount = 0;

  public GameObject playerPanel;
  private bool playerPanelActive;
  private GridLayoutGroup moduleLayout;
  private Dictionary<ModuleType, string> modImgMap;

  private Text ElectricText;
  private Text ComputationText;
  private Text HpText;

  private float timer;

	// Use this for initialization
	void Start () {
    body = GetComponent<Rigidbody2D>();
    toRight = true;
    bulletList = new List<GameObject>();
    capcollider = GetComponent<CapsuleCollider2D>();
    GameObject modulePanel = playerPanel.transform.Find("ModulePanel").gameObject;
    moduleLayout = modulePanel.GetComponent<GridLayoutGroup>();
    modImgMap = new Dictionary<ModuleType, string>();
    modImgMap.Add(ModuleType.Core, "001");
    modImgMap.Add(ModuleType.Power, "002");
    modImgMap.Add(ModuleType.Energy, "003");
    modImgMap.Add(ModuleType.Computation, "005");
    modImgMap.Add(ModuleType.Armor, "006");
    modImgMap.Add(ModuleType.Weapon, "007");

    moduleList = new Module[9];
    moduleList[0] = new Module(ModuleType.Weapon, 100, 100, 10, -5, 0, -5);
    moduleList[1] = new Module(ModuleType.Core, 100, 100, 10, 10, 100, 10);
    moduleList[2] = new Module(ModuleType.Armor, 100, 100, 10, 0);
    moduleList[3] = new Module(ModuleType.Energy, 100, 100, 10, 20, 100);
    moduleList[4] = null;
    moduleList[5] = new Module(ModuleType.Computation, 100, 100, 10, -5, 0, 10);
    moduleList[6] = new Module(ModuleType.Power, 100, 100, 10, -5, 0, -5, 100, 10);
    moduleList[7] = null;
    moduleList[8] = new Module(ModuleType.Power, 100, 100, 10, -5, 0, -5, 100, 10);

    for(int i = 0; i < 9; i ++)
    {
      Module m = moduleList[i];
      if(m != null)
      {
        totalHp += m.hp;
        totalMaxHp += m.maxHp;
        totalElecContribution += m.elecContribution;
        totalElecRestore += m.elecRestore;
        if (m.computingContribution > 0)
          totalComputingProduction += m.computingContribution;
        else
          totalComputingUsing += (-m.computingContribution);
        totalLoad += m.load;

        Image img = moduleLayout.transform.GetChild(i).gameObject.GetComponent<Image>();
        Sprite spr = Resources.Load<Sprite>("Images/" + modImgMap[m.type]);
        img.sprite = spr;

        Color c;
        float hpPercent = (float)(m.hp) / (float)(m.maxHp);
        if(hpPercent> 0.66)
        {
          c = Color.green;
        }
        else if (hpPercent > 0.33)
        {
          c = Color.yellow;
        }
        else
        {
          c = Color.red;
        }
        img.color = c;

        workingModuleCount++;
      }
    }

    ElectricText = playerPanel.transform.Find("ElectricText").gameObject.GetComponent<Text>();
    ComputationText = playerPanel.transform.Find("ComputationText").gameObject.GetComponent<Text>();
    HpText = playerPanel.transform.Find("HpText").gameObject.GetComponent<Text>();

    UpdateText();

    timer = 0;
  }

  void UpdateText()
  {
    ElectricText.text = string.Format("Electricity: {0}/{1} {2:+0;-#}/s", currentElecRestore, totalElecRestore, totalElecContribution);
    ComputationText.text = string.Format("Computation: {0}/{1}", totalComputingUsing, totalComputingProduction);
    float hpPercentage = totalMaxHp.Equals(0) ? 0 : (float)totalHp / (float)totalMaxHp;
    HpText.text = string.Format("HP: {0}/{1}({2:P})", totalHp, totalMaxHp, hpPercentage);
  }
	
	// Update is called once per frame
	void Update () {
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
	}

  void FixedUpdate()
  {
    CheckMove();
    CheckShoot();
    Debug.Log(body.velocity);
  }

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
      Debug.Log("getSpace");
      Debug.Log(isNearWallFlag);
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

  void CheckShoot()
  {
    if(Input.GetMouseButton(0))
    {
      Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousepos.z = 0;
      Vector3 direction = (mousepos - transform.position).normalized;

      GameObject bul = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
      Bullet bulscript = bul.GetComponent<Bullet>();
      bulscript.shooter = gameObject;
      //bulletList.Add(bul);
      Rigidbody2D bulrig = bul.GetComponent<Rigidbody2D>();
      bulrig.velocity = direction * bulletSpeed;
      //Destroy(bul, 5);
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
    return res;
  }

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
        Module m = moduleList[i];
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

  void UpdateModule(int damagedNumber)
  {
    Module m = moduleList[damagedNumber];
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
    img.color = c;
  }

  void RemoveModule(int damagedNumber)
  {
    Module m = moduleList[damagedNumber];
    if (m.computingContribution >= 0)
      totalComputingProduction -= m.computingContribution;
    else
      totalComputingUsing -= -m.computingContribution;
    totalElecContribution -= m.computingContribution;
    totalElecRestore -= m.elecRestore;
    if (currentElecRestore > totalElecRestore)
      currentElecRestore = totalElecRestore;
    //totalMaxHp -= m.maxHp;
    totalLoad -= m.load;
    workingModuleCount--;
    UpdateText();
  }
}
