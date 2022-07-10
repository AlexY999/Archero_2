using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Action OnEnemyDied;
    
    [Header("Health")] [SerializeField] private float hp = 100;
    [SerializeField] private GameObject healthBar;

    private float _startHp = 100;
    private Animator _animator;
    private string _coinTag = "Coin";
    
    public Transform player;

    public void DamageEnemy(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            OnDie();
            gameObject.SetActive(false);
            OnEnemyDied?.Invoke();
        }

        var transformLocalScale = healthBar.transform.localScale;
        transformLocalScale.x = hp / _startHp;
        healthBar.transform.localScale = transformLocalScale;
    }

    protected void Start()
    {
        _startHp = hp;
        _animator = GetComponent<Animator>();
    }

    private void OnDie()
    {
        ObjectPool.SharedInstance.GetPooledObject(_coinTag, true, transform.position, Quaternion.identity);
    }
}
