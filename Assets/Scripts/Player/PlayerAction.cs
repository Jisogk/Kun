using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour {

    public float speed = 2.0f;

    public Rigidbody2D m_Rigidbody2D; 
    private Animator m_Anim;
    
    // Use this for initialization
	void Awake ()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        //m_Anim = this.transform.Find("PlayerPic").GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

    public void Move(Vector2 direction)
    {
        //控制主角的移动
        //输入为二维向量
        m_Rigidbody2D.AddForce(speed * direction.normalized);
    }

    public void AnimControll( bool isMoving, float dir_x, float dir_y)
    {
        m_Anim.SetBool("isMoving", isMoving);
        m_Anim.SetFloat("X", dir_x);
        m_Anim.SetFloat("Y", dir_y);
    }
}
