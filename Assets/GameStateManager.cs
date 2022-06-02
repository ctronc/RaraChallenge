using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameModeText;
    [SerializeField] private TextMeshProUGUI gameStateText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int playerScore;

    private void Awake()
    {
        SubscribeToEvents();
        playerScore = 0;
    }

    private void SubscribeToEvents()
    {
        CoinController.onCoinTouch += IncreaseScore;
    }

    private void UnsubscribeFromEvents()
    {
        CoinController.onCoinTouch -= IncreaseScore;
    }

    private void IncreaseScore()
    {
        playerScore += 1;
        scoreText.text = "Score: " + playerScore;
    }

    private void StateReset()
    {
        playerScore = 0;
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
}
