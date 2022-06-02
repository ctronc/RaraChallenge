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
    
    enum GameStates {Build, Play, Win, GameOver}

    private GameStates _gameState;

    
    private void Awake()
    {
        SubscribeToEvents();
        _playerScore = 0; // has to be set when entering play mode, not here
        _gameState = GameStates.Build;
        playAgainButton.gameObject.SetActive(false);
        EnterBuildMode();
    }

    private void SubscribeToEvents()
    {
        PlayerState.OnPlayerWin += PlayerWin;
        PlayerState.OnPlayerDeath += GameOver;
        CoinController.OnCoinTouch += IncreaseScore;
    }

    private void UnsubscribeFromEvents()
    {
        PlayerState.OnPlayerWin -= PlayerWin;
        PlayerState.OnPlayerDeath -= GameOver;
        CoinController.OnCoinTouch -= IncreaseScore;
    }

    public void EnterBuildMode()
    {
        _gameState = GameStates.Build;
        
        buildModeUI.SetActive(true);
        buildModeButton.enabled = false;
        playModeButton.enabled = true;
        
        gameModeText.text = "Mode: Build";
        
        Time.timeScale = 0;
        ResetGameState();
        
        // Enable object placing

    }

    public void EnterPlayMode()
    {
        // Check if there's a player and a flag at least
        _gameState = GameStates.Play;
        
        buildModeUI.SetActive(false);
        buildModeButton.enabled = true;
        playModeButton.enabled = false;
        
        gameModeText.text = "Mode: Play";
        
        Time.timeScale = 1;
    }

    public void ClearLevel()
    {
        foreach (Transform childTransform in userLevel.transform)
        {
            Destroy(childTransform.gameObject);
        }
        
        // Destroy pooled projectiles
        OnLevelClear?.Invoke();
    }

    private void PlayerWin()
    {
        if (_gameState != GameStates.Win)
        {
            _gameState = GameStates.Win;
            gameStateText.text = "You win!";
            Time.timeScale = 0;
            playAgainButton.gameObject.SetActive(true);
        }
    }

    private void GameOver()
    {
        if (_gameState != GameStates.GameOver)
        {
            _gameState = GameStates.GameOver;
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

    public void PlayAgain()
    {
        if (_gameState != GameStates.Play)
        {
            Time.timeScale = 1;
            _gameState = GameStates.Play;
            gameModeText.text = "Mode: Play";
            
            ResetGameState();
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
        UnsubscribeFromEvents();
    }
}
