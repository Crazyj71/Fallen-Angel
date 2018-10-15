using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour {
    private Rigidbody2D rb2d;
    public float fallDelay;
    public float activeDelay;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player")){
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall(){
        yield return new WaitForSeconds(fallDelay);
        rb2d.isKinematic = false;
        yield return new WaitForSeconds(activeDelay);
        rb2d.gameObject.SetActive(false);
        yield return 0;
    }
}
