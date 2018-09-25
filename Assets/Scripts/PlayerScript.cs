using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public float runSpeed;
    public float jumpForce;

    private Rigidbody2D rb2d;
    private Collider2D c2d;
    // Use this for initialization

    void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0);
        rb2d.AddForce(movement*runSpeed);
    }

    void Jump()
    {
        rb2d.AddForce(new Vector2(0, 1)*jumpForce);
    }

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        c2d = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Movement();
        if (Input.GetKeyDown(KeyCode.Space) && rb2d.IsTouchingLayers(Physics2D.AllLayers))
        {
            Jump();
        }

	}
}
