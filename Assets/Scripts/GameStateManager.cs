using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameModeText;
    [SerializeField] private TextMeshProUGUI gameStateText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button playModeButton;
    [SerializeField] private Button buildModeButton;
    [SerializeField] private GameObject buildModeUI;
    [SerializeField] private GameObject userLevel;
    
    private int _playerScore;
    
    public delegate void GameStateAction();
    public static event GameStateAction OnGameReset;
    public static event GameStateAction OnLevelClear;
    public static event GameStateAction OnBuildMode;
    public static event GameStateAction OnPlayMode;
    
    enum GameState {Build, Play, Win, GameOver}
    private GameState _gameState;

    
    private void Awake()
    {
        _gameState = GameState.Build;
        playAgainButton.gameObject.SetActive(false);
        EnterBuildMode();
    }

    private void OnEnable()
    {
        PlayerState.OnPlayerWin += PlayerWin;
        PlayerState.OnPlayerDeath += GameOver;
        CoinController.OnCoinTouch += IncreaseScore;
    }

    public void EnterBuildMode()
    {
        _gameState = GameState.Build;
        OnBuildMode?.Invoke();
        
        buildModeUI.SetActive(true);
        buildModeButton.enabled = false;
        playModeButton.enabled = true;
        
        gameModeText.text = "Mode: Build";
        
        Time.timeScale = 0;
        ResetGameState();
        
        Debug.Log("Build mode activated");
    }

    public void EnterPlayMode()
    {
        _gameState = GameState.Play;
        _playerScore = 0;
        OnPlayMode?.Invoke();
        
        buildModeUI.SetActive(false);
        buildModeButton.enabled = true;
        playModeButton.enabled = false;
        
        gameModeText.text = "Mode: Play";
        
        Time.timeScale = 1;
        
        Debug.Log("Play mode activated");
    }

    public void ClearLevel()
    {
        foreach (Transform childTransform in userLevel.transform)
        {
            Destroy(childTransform.gameObject);
        }
        
        // used to destroy pooled projectiles and clear player
        OnLevelClear?.Invoke();
    }

    private void PlayerWin()
    {
        if (_gameState != GameState.Win)
        {
            _gameState = GameState.Win;
            gameStateText.text = "You win!";
            Time.timeScale = 0;
            playAgainButton.gameObject.SetActive(true);
        }
    }

    private void GameOver()
    {
        if (_gameState != GameState.GameOver)
        {
            _gameState = GameState.GameOver;
            gameStateText.text = "Game over :(";
            Time.timeScale = 0;
            playAgainButton.gameObject.SetActive(true);
        }
    }

    private void IncreaseScore()
    {
        _playerScore += 1;
        scoreText.text = "Score: " + _playerScore;
    }

    public void Retry()
    {
        if (_gameState != GameState.Play)
        {
            Time.timeScale = 1;
            _gameState = GameState.Play;
            gameModeText.text = "Mode: Play";
            
            ResetGameState();
            
            Debug.Log("Retrying. Good luck!");
        }
    }

    private void ResetGameState()
    {
        _playerScore = 0;
        gameStateText.text = "";
        scoreText.text = "Score: " + _playerScore;
        playAgainButton.gameObject.SetActive(false);

        OnGameReset?.Invoke();
    }

    private void OnDisable()
    {
        PlayerState.OnPlayerWin -= PlayerWin;
        PlayerState.OnPlayerDeath -= GameOver;
        CoinController.OnCoinTouch -= IncreaseScore;
    }
}
