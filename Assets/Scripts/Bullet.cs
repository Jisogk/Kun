using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

  public GameObject shooter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    Destroy(gameObject, 5);
	}

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision != null)
    {
      //Debug.Log(collision.gameObject.name);
      if(collision != shooter.GetComponent<CapsuleCollider2D>())
        Destroy(gameObject);
    }
  }
}
