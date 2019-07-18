using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

  public GameObject player;
  private Transform follow;
  private Vector3 offset;

	// Use this for initialization
	void Start () {
    follow = player.GetComponent<Transform>();
    offset = new Vector3(0, 0, -10);
	}
	
	// Update is called once per frame
	void LateUpdate () {
    transform.position = follow.position + offset;
	}
}
