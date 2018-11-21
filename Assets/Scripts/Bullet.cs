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
    if(collision != null && collision.tag != "Bullet")
    {
      Transform shooterTransform = null;
      if (shooter != null)
        shooterTransform = shooter.transform;
      if (collision.gameObject != shooter
        && !(collision.tag == "ModuleObj" && collision.transform.parent == shooterTransform)
        && collision.tag != "Player")
      {
        //Debug.Log(collision.transform.parent);
        //Debug.Log(shooterTransform);
        Destroy(gameObject);
      }     
    }
  }
}
