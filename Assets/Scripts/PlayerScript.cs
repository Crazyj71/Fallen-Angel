﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour {
    public float friction;
    public float runForce;
    public float jumpForce;
    public float maxRunSpeed = 20f;

    private Rigidbody2D rb2d;
    private Collider2D c2d;
    private bool grounded;
    private bool jump = false;
    // Use this for initialization

    void Movement()
    {
        //IF USER IS PRESSING ARROW KEYS
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) && rb2d.IsTouchingLayers(Physics2D.AllLayers))
        {
            Vector2 velocity = rb2d.velocity;
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal, 0f);
            float currentspeed = moveHorizontal * rb2d.velocity.x;
            if (currentspeed < maxRunSpeed) {
                rb2d.AddForce(movement * runForce);
            }
        }
        else if(rb2d.IsTouchingLayers(Physics2D.AllLayers) && !Input.GetKey(KeyCode.Space))
        {
            rb2d.velocity = friction*rb2d.velocity;
        }
    }

    void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(new Vector2(0f, jumpForce));
        jump = false;
    }

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        c2d = GetComponent<BoxCollider2D>();
	}


    // Update is called once per frame
    void Update () {
        transform.rotation = Quaternion.identity;
        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            jump = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BasicPlatform")){
            grounded = true;
        }
        Debug.Log("Touching the Ground");
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BasicPlatform")){
            grounded = false;
        }
    }

    void FixedUpdate()
    {
        
        Movement();

        if (jump)
        {
            Jump();
        }

    }
}
