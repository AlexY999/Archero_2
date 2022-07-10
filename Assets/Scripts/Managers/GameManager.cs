using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SceneIndex nextSceneName;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private float startHp = 100;
    [SerializeField] private List<EnemyScript> listOfEnemy = new List<EnemyScript>();
    [SerializeField] private TriggerZone endZone;
    
    private Vector3 _startPosition;
    private int _numOfEnemy;
    private int _score = 0;
    private float _hp;

    private void OnEnable()
    {
        _numOfEnemy = listOfEnemy.Count;
        _startPosition = playerMovement.transform.position;
        playerMovement.OnCoinCollected += CollectCoin;
        playerMovement.OnDeath += PlayerDeath;
        playerMovement.OnWin += PlayerWin;
        playerMovement.OnDamage += PlayerDamage;

        foreach (var enemy in listOfEnemy)
        {
            enemy.OnEnemyDied += EnemyDie;
        }
    }
    
    private void OnDisable()
    {
        playerMovement.OnCoinCollected -= CollectCoin;
        playerMovement.OnDeath -= PlayerDeath;
        playerMovement.OnWin -= PlayerWin;
        playerMovement.OnDamage -= PlayerDamage;
        
        foreach (var enemy in listOfEnemy)
        {
            enemy.OnEnemyDied -= EnemyDie;
        }
    }

    private void Start()
    {
        StartGameplay();
    }

    public void SetStartScore(int score)
    {
        _score = score;
    }
    
    private void StartGameplay()
    {
        ResetPlayerPosition();
        hudManager.SetGamePanel();
        playerMovement.enabled = true;
        _hp = startHp;
        hudManager.ChangeScore(_score);
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
        StopEnemy();
    }
    
    private void PlayerWin()
    {
        if (nextSceneName == SceneIndex.None)
        {
            hudManager.SetWinLosePanel(true, _score);
            playerMovement.enabled = false;
            StopEnemy();
        }
        else
        {
            hudManager.ChangeScene(nextSceneName.ToString(), _score);
        }
    }

    private void StopEnemy()
    {
        foreach (var enemy in listOfEnemy)
        {
            enemy.enabled = false;
        }
    }
    
    private void PlayerDamage(float damage)
    {
        if (_hp - damage > 0)
        {
            _hp -= damage;
        }
        else
        {
            _hp = 0;
            PlayerDeath();
        }
        
        hudManager.ChangeHPValue(_hp / startHp);
    }

    private void ResetPlayerPosition()
    {
        playerMovement.transform.position = _startPosition;
    }

    private void EnemyDie()
    {
        _numOfEnemy--;

        if (_numOfEnemy == 0)
        {
            endZone.Type = TriggerZoneType.End;
            endZone.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
    }
    
    private enum SceneIndex
    {
        Scene1,
        Scene2,
        FinalScene,
        None,
    }
}
