using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private Vector2 startPosition;
    
    private int _score;
    
    // private void OnEnable()
    // {
    //     playerMovement.OnCoinCollected += CollectCoin;
    //     playerMovement.OnDeath += PlayerDeath;
    //     playerMovement.OnWin += PlayerWin;
    // }
    //
    // private void OnDisable()
    // {
    //     playerMovement.OnCoinCollected -= CollectCoin;
    //     playerMovement.OnWin -= PlayerWin;
    // }

    private void Start()
    {
        StartGameplay();
    }

    private void StartGameplay()
    {
        ResetPlayerPosition();
        hudManager.SetGamePanel();
        playerMovement.enabled = true;
    }

    private void CollectCoin()
    {
        _score++;
        hudManager.ChangeScore(_score);
    }
    
    private void PlayerDeath()
    {
        hudManager.SetWinLosePanel(false, _score);
        playerMovement.enabled = false;
    }
    
    private void PlayerWin()
    {
        hudManager.SetWinLosePanel(true, _score);
        playerMovement.enabled = false;
    }

    private void ResetPlayerPosition()
    {
        playerMovement.transform.position = startPosition;
    }
}
