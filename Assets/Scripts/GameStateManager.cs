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
        // Game starts in build mode
        playAgainButton.gameObject.SetActive(false);
        EnterBuildMode();
    }

    private void OnEnable()
    {
        // Listen to events from player and coins
        PlayerState.OnPlayerWin += PlayerWin;
        PlayerState.OnPlayerDeath += GameOver;
        CoinController.OnCoinTouch += IncreaseScore;
    }

    public void EnterBuildMode()
    {
        // Set the state to build mode
        _gameState = GameState.Build;
        OnBuildMode?.Invoke();
        
        buildModeUI.SetActive(true);
        buildModeButton.enabled = false;
        playModeButton.enabled = true;
        
        gameModeText.text = "Mode: Build";
        
        // Pauses the game
        Time.timeScale = 0;
        
        // Reset all entities to their initial state
        ResetGameState();
        
        Debug.Log("Build mode activated");
    }

    public void EnterPlayMode()
    {
        // Set the state to play mode
        _gameState = GameState.Play;
        OnPlayMode?.Invoke();
        
        _playerScore = 0;
        
        buildModeUI.SetActive(false);
        buildModeButton.enabled = true;
        playModeButton.enabled = false;
        
        gameModeText.text = "Mode: Play";
        
        // unpauses the game
        Time.timeScale = 1;
        
        Debug.Log("Play mode activated");
    }

    public void ClearLevel()
    {
        // For clear level button, destroys all GameObjects children of UserLevel
        foreach (Transform childTransform in userLevel.transform)
        {
            Destroy(childTransform.gameObject);
        }
        
        // Used to destroy pooled projectiles and hide player mesh
        OnLevelClear?.Invoke();
    }

    private void PlayerWin()
    {
        // Called on player win
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
        // Called when player dies
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
        // Increase score when player touches a coin
        _playerScore += 1;
        scoreText.text = "Score: " + _playerScore;
    }

    public void Retry()
    {
        // Method for retry button
        // Play again the same level after winning on losing.
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
