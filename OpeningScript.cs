using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScript : MonoBehaviour {

    public float time;
	// Use this for initialization
	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if(Input.anyKeyDown || time > 60f)
        {
            SceneManager.LoadScene("MainMenu");
        }
	}
}
