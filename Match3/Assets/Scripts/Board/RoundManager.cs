using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    // Rules 
    [Header("Game Rules")]
    public int roundDuration;
    
    [Tooltip("The base and first value of points that the player needs to achieve to win each round")] 
    public int baseGoal;
    
    [Tooltip("Time between rounds")] 
    public float nextRoundTime;
    
    // counter to make game progression
    private int _roundCount;
    
    // UI objects to change 
    [Header("UI elements")] 
    public Text timerText;
    public Text currentPointsText;
    public Text goalPointsText;
    public GameObject youLose;
    public GameObject nextRound;
    public GameObject cooldown;
    public GameObject shuffle;
    public Text endTotalPoints;
    public Text highScorePoints;
    public Text cooldownText;
    public Text roundCountText;
    [Tooltip("Color of timer text in last 30 seconds")] 
    public Color dangerTimeColor;
    [Tooltip("default timer text color")] 
    public Color defaultTimeColor;

    // timer variables
    private bool _timerIsRunning = false;
    private float _timeRemaining;
    
    // points 
    [HideInInspector] public int totalPoints;
    [HideInInspector] public int totalPointsToGoal;
    [HideInInspector] public bool gameEnded;
    
    // Board holder reference 
    private BoardHolder _boardHolder;
    
    // Start is called before the first frame update
    void Start()
    {
        // get board holder 
        _boardHolder = gameObject.GetComponent<BoardHolder>();
        
        // set variables to a fresh game 
        totalPoints = 0;
        totalPointsToGoal = 0;
        currentPointsText.text = totalPoints.ToString();
        goalPointsText.text = totalPointsToGoal.ToString();
        gameEnded = false;
        _roundCount = 0;
        
        // stop music from menu or from end game
        AudioManager.instance.Stop("BackgroundMusic");
        AudioManager.instance.Stop("YouLose");
        // start cooldown to begin the game 
        StartCoroutine(StartCooldown());
    }

    // add points in current score  
    public void AddPoints(int points)
    {
        totalPoints += points;
        currentPointsText.text = totalPoints.ToString();
        AudioManager.instance.Play("Clear");
        if (totalPoints >= totalPointsToGoal && !gameEnded)
        {
            StartCoroutine(RoundWin());
            StartRound();
        }
    }

    // called by board holder to start the round 
    public IEnumerator StartCooldown()
    {
        _boardHolder.paused = true;
        cooldown.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            // play countdown SFX
            AudioManager.instance.Play("Countdown");
            cooldownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        cooldown.SetActive(false);
        // play music from menu 
        AudioManager.instance.Play("BackgroundMusic");
        StartRound();
        // unpause game 
        _boardHolder.paused = false;
    }

    private void StartRound()
    {
        // make game progression 
        int progression = _roundCount * (int)Mathf.Log(totalPoints,2);
        totalPointsToGoal += baseGoal + progression;
        _roundCount++;
        
        // update values 
        goalPointsText.text = totalPointsToGoal.ToString();
        roundCountText.text = _roundCount.ToString();
        _timeRemaining = roundDuration;
        _timerIsRunning = true;

    }

    private void EndRound()
    {
        // stop bgm music 
        AudioManager.instance.Stop("BackgroundMusic");
        // play you lose sound
        AudioManager.instance.Play("YouLose");
        // pause the game 
        _boardHolder.paused = true;
        gameEnded = true;
        
        // appear you lose 
        youLose.SetActive(true);

        // save high score and go back to first scene 
        int lastHighScore = PlayerPrefs.GetInt("HighScore");
        if (totalPoints > lastHighScore)
        {
            PlayerPrefs.SetInt("HighScore", totalPoints);
            lastHighScore = totalPoints;
        }

        // update You Lose UI 
        endTotalPoints.text = totalPoints.ToString();
        highScorePoints.text = lastHighScore.ToString();
    }

    // called when completed a round 
    private IEnumerator RoundWin()
    {
        nextRound.SetActive(true);

        // play next round sound 
        AudioManager.instance.Play("NextRound");
        
        yield return new WaitForSeconds(nextRoundTime);
        
        nextRound.SetActive(false);
    }
    
    void Update()
    {
        if (_timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                DisplayTime(_timeRemaining);
            }
            else
            {
                _timeRemaining = 0;
                _timerIsRunning = false;
                EndRound();
                
            }
        }
    }
    
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        // activate red text on last 30 seconds 
        if (minutes == 0 && seconds <= 30)
        {
            timerText.color = dangerTimeColor;
        }
        else
        {
            timerText.color = defaultTimeColor;
        }
    }

    // called when board needs to shuffle 
    public void DisplayShufflingWarning()
    {
        StartCoroutine(ShuffleUI());
    }
    
    // show shuffle ui
    private IEnumerator ShuffleUI()
    {
        shuffle.SetActive(true);
        
        // stop timer when shuffling 
        _timerIsRunning = false;
        
        yield return new WaitForSeconds(2f);
        
        shuffle.SetActive(false);
        _timerIsRunning = true;
    }
    
    
}
