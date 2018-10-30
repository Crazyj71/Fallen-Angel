using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour {
    private Rigidbody2D rb2d;
    public float fallDelay;
    public float activeDelay;
    private Color color = Color.white;
    private Color altColor = Color.black;
    private Vector3 position;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        position = transform.position;

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player")){
            StartCoroutine(Fall());
        }
        
    }

    IEnumerator Fall(){
        GetComponent<Renderer>().material.color = altColor;
        yield return new WaitForSeconds(fallDelay);
        rb2d.isKinematic = false;
        yield return new WaitForSeconds(activeDelay);
        transform.position = position;
        rb2d.velocity = rb2d.velocity * 0;
        rb2d.isKinematic = true;
        GetComponent<Renderer>().material.color = color;
    }

}
