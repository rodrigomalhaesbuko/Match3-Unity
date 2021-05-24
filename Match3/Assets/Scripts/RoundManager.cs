using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    // Rules 
    public int roundDuration;
    [Tooltip("Total of points that the player needs to achieve to win the each round")] 
    public int baseGoal;
    
    // UI objects to change 
    [Header("UI elements")] 
    public Text timerText;
    public Text currentPointsText;
    public Text goalPointsText;
    
    // timer variables
    private bool _timerIsRunning = false;
    private float _timeRemaining;
    
    // points 
    [HideInInspector] public int totalPoints;
    [HideInInspector] public int totalPointsToGoal;
    
    // Start is called before the first frame update
    void Start()
    {
        totalPoints = 0;
        totalPointsToGoal = 0;
        currentPointsText.text = totalPoints.ToString();
        goalPointsText.text = totalPointsToGoal.ToString();
        StartCoroutine(StartCooldown());
    }

    // add points in current score  
    public void AddPoints(int points)
    {
        totalPoints += points;
        currentPointsText.text = totalPoints.ToString();
        if (totalPoints >= totalPointsToGoal)
        {
            StartCoroutine(RoundWin());
        }
    }

    // called by board holder to start the round 
    public IEnumerator StartCooldown()
    {
        for (int i = 1; i < 4; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1f);
        }
        StartRound();
    }

    private void StartRound()
    {
        totalPointsToGoal += baseGoal;
        goalPointsText.text = totalPointsToGoal.ToString();
        _timeRemaining = roundDuration;
        _timerIsRunning = true;
    }

    private void EndRound()
    {
        Debug.Log("You Lost");
        // save high score and go back to first scene 
    }

    private IEnumerator RoundWin()
    {
        Debug.Log("Congratulations");
        _timerIsRunning = false;
        yield return new WaitForSeconds(1f);
        StartRound();
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
                Debug.Log("Time has run out!");
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
    }
}
