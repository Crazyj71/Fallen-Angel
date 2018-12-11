using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlammableScript : MonoBehaviour {
    private GameObject Flame;
    private Rigidbody2D rb2d;
    public AudioSource burn;

    // Use this for initialization
    void Start () {
        Flame = transform.Find("Flame").gameObject;
        Flame.SetActive(false);
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    IEnumerator FireHandler()
    {
        GetComponent<SpriteRenderer>().color = Color.grey;
        Flame.SetActive(true);
        burn.Play();
        Flame.transform.position = transform.position;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(1f);
        rb2d.gameObject.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Blast"))
        {
            StartCoroutine(FireHandler());
        }
    }


}
