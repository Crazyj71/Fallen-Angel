using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Text Hell, Earth, Heaven;
    public GameObject HellG, EarthG, HeavenG;
	// Use this for initialization
	void Start () {
        if (PlayerPrefs.GetInt("HellComplete") == 1)
        {
            EarthG.SetActive(true);
        }
        else EarthG.SetActive(false);
        if (PlayerPrefs.GetInt("EarthComplete") == 1)
        {
            HeavenG.SetActive(true);
        }
        else HeavenG.SetActive(false);

        Hell.text = "Highscore: " + PlayerPrefs.GetInt("Hell").ToString() + " \t\t Spirits: " + PlayerPrefs.GetInt("HellSpiritCount").ToString() + " of 3";
        Earth.text = "Highscore: " + PlayerPrefs.GetInt("Earth").ToString() + " \t\t Spirits: " + PlayerPrefs.GetInt("EarthSpiritCount").ToString() + " of 5";
        Heaven.text = "Highscore: " + PlayerPrefs.GetInt("Earth").ToString() + " \t\t Spirits: " + PlayerPrefs.GetInt("HeavenSpiritCount").ToString() + " of 5";
    }


  

    // Update is called once per frame
    void Update()
    {


        if (PlayerPrefs.GetInt("HellComplete") == 1)
        {
            EarthG.SetActive(true);
        }
        else EarthG.SetActive(false);
        if (PlayerPrefs.GetInt("EarthComplete") == 1)
        {
            HeavenG.SetActive(true);
        }
        else HeavenG.SetActive(false);

        Hell.text = "Highscore: " + PlayerPrefs.GetInt("Hell").ToString() + " \t\t Spirits: " + PlayerPrefs.GetInt("HellSpiritCount").ToString() + " of 3";
        Earth.text = "Highscore: " + PlayerPrefs.GetInt("Earth").ToString() + " \t\t Spirits: " + PlayerPrefs.GetInt("EarthSpiritCount").ToString() + " of 5";
        Heaven.text = "Highscore: " + PlayerPrefs.GetInt("Earth").ToString() + " \t\t Spirits: " + PlayerPrefs.GetInt("HeavenSpiritCount").ToString() + " of 5";
    }
}
