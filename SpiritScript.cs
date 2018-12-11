using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritScript : MonoBehaviour {
    private AudioSource idleSound;
    private GameObject Player;
    
    bool SoundPlaying;
	// Use this for initialization
	void Start () {
        idleSound = GetComponent<AudioSource>();
        Player = GameObject.Find("Player");
        Player.GetComponent<Rigidbody2D>();
        SoundPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x - Player.transform.position.x < 20f
                && transform.position.x - Player.transform.position.x > -20f && (transform.position.y - Player.transform.position.y < 20f
                && transform.position.y - Player.transform.position.y > -20f))
        {
            if (SoundPlaying == false)
            {
                idleSound.Play();
                SoundPlaying = true;
            }
        }
        else
        {
            idleSound.Pause();
            SoundPlaying = false;
        }



	}



}
