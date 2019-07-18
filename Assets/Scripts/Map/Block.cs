using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

  public int hp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.tag == "Bullet")
    {
      hp -= 2;
      if(hp <= 0)
      {
        Destroy(gameObject);
      }
    }
  }
}
