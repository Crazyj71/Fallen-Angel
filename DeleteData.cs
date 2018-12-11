using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteData : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void ClearGameData()
    {
        PlayerPrefs.DeleteAll();
    }
    // Update is called once per frame
    void Update () {
		
	}
}
