using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour {

  private event EventHandler<DoorEventArgs> TouchDoorHandler;
  private event EventHandler<DoorEventArgs> LeaveDoorHandler;
  //[HideInInspector]
  public GameManager.Direction direction;

    private void Awake()
    {
        //TouchDoorHandler += GameManager.instance.TouchDoor;
        //LeaveDoorHandler += GameManager.instance.LeaveDoor;
    }

    // Use this for initialization
    void Start()
    {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (TouchDoorHandler != null)
                TouchDoorHandler(this, new DoorEventArgs(direction));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (LeaveDoorHandler != null)
                LeaveDoorHandler(this, new DoorEventArgs(direction));
        }
    }
}
