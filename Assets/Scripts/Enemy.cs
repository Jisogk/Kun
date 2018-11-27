using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

  private GameObject player;
  public GameObject bullet;
  public float sqrDetectDistance;
  public float shootingSpeed;
  public float bulletSpeed;
  public int maxHp;
  public float velocity;

  private float lastShootTime;
  private int hp;
  private bool toRight;
  private CapsuleCollider2D capcollider;
  private LayerMask groundLayer;
  private Rigidbody2D body;

	// Use this for initialization
	void Start () {
    player = GameObject.FindGameObjectWithTag("Player");
    lastShootTime = -10f;
    hp = maxHp;
    toRight = true;
    capcollider = GetComponent<CapsuleCollider2D>();
    groundLayer = LayerMask.GetMask("Ground");
    body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void FixedUpdate()
  {
    if((transform.position - player.transform.position).sqrMagnitude < sqrDetectDistance)
    {
      Shoot();
    }

    bool nearEdge;
    bool isGroundedFlag = IsGrounded(out nearEdge);
    bool isNearWallFlag = isNearWall();

    if (isGroundedFlag)
    {
      if(isNearWallFlag || nearEdge)
      {
        TurnAround();
      }
      body.MovePosition(body.position + new Vector2(velocity * Time.deltaTime, 0f));
    }
  }

  void Shoot()
  {
    if(Time.time - lastShootTime >= shootingSpeed)
    {
      Vector3 direction = (player.transform.position - transform.position).normalized;
      GameObject bul = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
      bul.name = "enemybullet";
      Bullet bulscript = bul.GetComponent<Bullet>();
      bulscript.shooter = gameObject;
      //bulletList.Add(bul);
      Rigidbody2D bulrig = bul.GetComponent<Rigidbody2D>();
      bulrig.velocity = direction * bulletSpeed;
      //Destroy(bul, 5);
      lastShootTime = Time.time;
    }
  }

  bool IsGrounded(out bool nearEdge)
  {
    bool res = false;
    nearEdge = true;
    Bounds bd = capcollider.bounds;
    Vector2[] colliderpos = new Vector2[3];
    colliderpos[0] = bd.center;
    colliderpos[1] = new Vector2(bd.min.x, bd.center.y);
    colliderpos[2] = new Vector2(bd.max.x, bd.center.y);

    int i = 0;
    int frontIndex = toRight ? 2 : 1;
    foreach (Vector2 pos in colliderpos)
    {
      RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, bd.extents.y + 0.05f, groundLayer);
      if (hit.collider != null)
      {
        res = true;
        if (i == frontIndex)
          nearEdge = false;
      }
      i++;
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

  void TurnAround()
  {
    Vector3 originScale = transform.localScale;
    transform.localScale = new Vector3(-originScale.x, originScale.y, originScale.z);
    toRight = !toRight;
    velocity = -velocity;
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.tag == "Bullet" && collision.name != "enemybullet")
    {
      hp -= 2;
      if(hp < 0)
      {
        Destroy(gameObject);
      }
    }
  }
}
