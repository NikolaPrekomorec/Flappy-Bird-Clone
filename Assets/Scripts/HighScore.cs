using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HighScore : MonoBehaviour {

    Text highscore;

    private void OnEnable()
    {
        highscore = GetComponent<Text>();
        highscore.text = "Highscore: "+ PlayerPrefs.GetInt("HighScore").ToString();
    }

}
