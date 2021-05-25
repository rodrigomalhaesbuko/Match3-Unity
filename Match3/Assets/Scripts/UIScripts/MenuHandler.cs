using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public Text highScorePoints;
    // Start is called before the first frame update
    void Start()
    {
        // Stop Lose audio 
        AudioManager.instance.Stop("YouLose");
        // Play BGM 
        AudioManager.instance.Play("BackgroundMusic");
        
        // get highScore from player prefs 
        int highScore = PlayerPrefs.GetInt("HighScore");
        
        //update highScore on menu screen 
        highScorePoints.text = highScore.ToString();
    }
}
