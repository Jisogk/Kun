using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAttackScript : MonoBehaviour {

  public GameObject bullet;
  public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(1))
    {
      Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousepos.z = 0;
      Vector3 direction = (player.transform.position - mousepos).normalized;

      GameObject bul = Instantiate(bullet, mousepos, Quaternion.identity) as GameObject;
      bul.name = "testbullet";
      Bullet bulscript = bul.GetComponent<Bullet>();
      bulscript.shooter = gameObject;

      Rigidbody2D bulrig = bul.GetComponent<Rigidbody2D>();
      bulrig.velocity = direction * 50;
    }
	}
}
