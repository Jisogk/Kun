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

  private float lastShootTime;
  private int hp;

	// Use this for initialization
	void Start () {
    player = GameObject.FindGameObjectWithTag("Player");
    lastShootTime = -10f;
    hp = maxHp;
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
