using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour {

    private float movement;
    public float speed = 0.7f;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < 415)
        {
            movement = speed * Time.deltaTime;
            this.transform.Translate(0, movement, 0);
        }
    }


}
