using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject startPage;
    public GameObject countdownPage;
    public GameObject gameOverPage;
    public Text scoreTxt;

    public static GameManager Instance;

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    enum PageState
    {
        None,
        Start,
        Countdown,
        GameOver,
    }

    int score = 0;
    public bool gameOver = true;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        CountdownText.OnCountdownFinnished += OnCountdownFinnished;
        TapController.OnScored += OnScored;
        TapController.OnDied += OnDied;
    }

    void OnDisable()
    {
        CountdownText.OnCountdownFinnished -= OnCountdownFinnished;
        TapController.OnScored -= OnScored;
        TapController.OnDied -= OnDied;
    }

    void OnCountdownFinnished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
    }

    void OnScored()
    {
        score++;
        scoreTxt.text = score.ToString();
    }

    void OnDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore",score);
        }
        SetPageState(PageState.GameOver);
    }

    private void Start()
    {
        SetPageState(PageState.Start);
        gameOver = true;
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                countdownPage.SetActive(false);
                gameOverPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                countdownPage.SetActive(false);
                gameOverPage.SetActive(false);
                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                countdownPage.SetActive(true);
                gameOverPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                countdownPage.SetActive(false);
                gameOverPage.SetActive(true);
                break;

        }
    }

    public void ConfirmGameOver()
    {
        //Replay button is hit
        OnGameOverConfirmed(); //event
        scoreTxt.text = "0";
        SetPageState(PageState.Start);
        gameOver = true;

    }

    public void StartGame()
    {
        //Play button is hit
        SetPageState(PageState.Countdown);
        gameOver = true;

    }




}
