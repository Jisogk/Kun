using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleObj : MonoBehaviour {

  [HideInInspector]
  public int index;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Bullet" && collision.name == "testbullet")
    {
      transform.parent.GetComponent<Player>().HurtModule(index);
    }
  }
}
