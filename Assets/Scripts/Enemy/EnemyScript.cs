    using System;
    using System.Collections;
using UnityEngine;
    using UnityEngine.Serialization;

    public class EnemyScript : MonoBehaviour
{
    [Header("Health")] [SerializeField] private float hp = 100;
    [SerializeField] private GameObject healthBar;

    [Header("Attack force")] [SerializeField]
    private float attackForce = 20;

    private float _startHp = 100;
    private Animator _animator;
    
    public Transform player;

    // [Header("Move speed")] [SerializeField]
    // private float moveSpeed = 20;
    //
    // [Header("Rotation speed")] [SerializeField]
    // private float rotationSpeed = 20;
    //
    // void Update()
    // {
    //     var look_dir = player.position - transform.position;
    //     //look_dir.y = 0;
    //     transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(look_dir),rotationSpeed*Time.deltaTime);
    //     transform.position += transform.forward * moveSpeed * Time.deltaTime;
    // }
    
    public void DamageEnemy(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
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
}
