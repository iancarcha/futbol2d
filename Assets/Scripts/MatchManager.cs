using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    [SerializeField] private GoalKeeper player1Goal;
    [SerializeField] private GoalKeeper player2Goal;

    [SerializeField] private FootballPlayer player1;
    [SerializeField] private FootballPlayer player2;
    [SerializeField] private Ball ball;

    [SerializeField] private TextMeshProUGUI player1ScoreText;
    [SerializeField] private TextMeshProUGUI player2ScoreText;
    [SerializeField] private TextMeshProUGUI goalText;
    private int player1Score;
    private int player2Score;

    private int secondsToPlay = 59;
    
    [SerializeField] private TextMeshProUGUI timer;

    [SerializeField] private AudioSource source;

    [SerializeField] private AudioClip goal;
    [SerializeField] private AudioClip referee;
    [SerializeField] private AudioClip publico;

    [SerializeField] private UIManager uiManager;
    

    private void Awake()
    {
        player1Goal.OnScore += ScoreP2;
        player2Goal.OnScore += ScoreP1;
        StartCoroutine(MatchTimer());
        EnablePlayers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    IEnumerator MatchTimer()
    {
        UpdateTimer();
        source.clip = referee;
        source.Play();
        PlayButton();

        while (secondsToPlay > 0)
        {
            yield return new WaitForSeconds(1);
            secondsToPlay--;
            UpdateTimer();
        }
        
        DisablePlayers();
        uiManager.DisplayResults(player1Score,player2Score);
        player1ScoreText.gameObject.SetActive(false);
        player2ScoreText.gameObject.SetActive(false);
        
    }

    IEnumerator ScoreGoal()
    {
        goalText.gameObject.SetActive(true);
        source.clip = goal;
        source.Play();
        LeanTween.scale(goalText.rectTransform, goalText.transform.localScale * 2f, source.clip.length).setEasePunch();
        DisablePlayers();
        yield return new WaitForSeconds(source.clip.length);
        goalText.gameObject.SetActive(false);
        StartOver();
        source.clip = referee;
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        EnablePlayers();
        PlayButton();
    }

    private void StartOver()
    {
        player1.transform.position = new Vector2(-1.5f, 0);
        player1.transform.rotation = Quaternion.Euler(0, 0, 0);
        player2.transform.position = new Vector2(1.5f, 0);
        player2.transform.rotation = Quaternion.Euler(0, 0, 180);
        ball.transform.parent = null;
        ball.EnableInteraction();
        ball.transform.position = Vector2.zero;
        ball.transform.rotation = Quaternion.identity;
    }

    private void ScoreP1()
    {
        player1Score++;
        UpdateScores();
        StartCoroutine(ScoreGoal());
        
    }

    private void ScoreP2()
    {
        player2Score++;
        UpdateScores();
        StartCoroutine(ScoreGoal());
    }

    private void UpdateTimer()
    {
        float minutes = Mathf.FloorToInt(secondsToPlay / 60);
        float seconds = Mathf.FloorToInt(secondsToPlay % 60);

        string secs = seconds.ToString();
        string mins = minutes.ToString();
        if (seconds < 10)
            secs = "0" + secs;
        if (minutes < 10)
            mins = "0" + mins;
        timer.text = mins + " : " + secs;
    }
    private void UpdateScores()
    {
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }
    private void DisablePlayers()
    {
        player1.DisablePlayer();
        player2.DisablePlayer();
    }
    
    private void EnablePlayers()
    {
        player1.EnablePlayer();
        player2.EnablePlayer();
    }
    
    private void PlayButton(){
        source.clip = publico;
        source.Play();
    }

 
   
}
