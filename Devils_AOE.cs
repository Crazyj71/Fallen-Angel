using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devils_AOE : MonoBehaviour {
    public GameObject Player;
    public AudioSource StrikeSound;

	// Use this for initialization
	void Start () {
		
	}

    bool activate = false;
    IEnumerator Activate()
    {
        activate = true;
        transform.position = Player.transform.position + new Vector3(0, 2);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update () {
        if (activate == false) 
        StartCoroutine(Activate());
	}
}
