using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    IEnumerator DelayScene()
    {
        yield return new WaitForSeconds(18.0f);
        SceneManager.LoadScene(PlayerPrefs.GetString("nextScene"));
    }
	// Update is called once per frame
	void Update () {
        StartCoroutine(DelayScene());
	}
}
