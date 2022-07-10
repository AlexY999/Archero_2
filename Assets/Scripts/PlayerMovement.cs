using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    public Action OnCoinCollected;
    public Action OnDeath;
    public Action OnWin;
    public Action<float> OnDamage;
    
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 5.0F;
    [SerializeField] private float attackRadius = 3.0f;
    [SerializeField] private float attackReload = 1.0f;
    [SerializeField] private LayerMask enemyLayerMask;

    private bool _attack;
    
    private const string IsWalking = "isWalking";
    private const string Attack = "Attack";
    private const string DieTag = "Die";

    private const int MaxAnimAttack = 2;

    private void Awake()
    {
        StartCoroutine(nameof(WaitForAttack));
    }

    void FixedUpdate()
    {
        Vector3 moveDirection;
        
#if UNITY_EDITOR
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#endif
        moveDirection = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

        
        moveDirection *= speed;

        if (moveDirection.magnitude >= 0.1f)
        {
            controller.Move(moveDirection * (speed * Time.deltaTime));
        }
        
        if (variableJoystick.Horizontal != 0 || variableJoystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(controller.velocity);
            animator.SetBool(IsWalking, true);
        }
        else
        {
            animator.SetBool(IsWalking, false);
        }
    }
    
    public void Damage(float damageValue)
    {
        OnDamage?.Invoke(damageValue);
    }

    [ContextMenu("Attack Enemy")]
    private void AttackEnemy()
    {
        var numOfAttack = Random.Range(0, MaxAnimAttack);
        animator.SetTrigger($"{Attack}{numOfAttack}");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<TriggerZone>(out var triggerZone))
        {
            switch (triggerZone.Type)
            {
                case TriggerZoneType.Death:
                    animator.SetTrigger(DieTag);
                    Debug.Log("You are died!");
                    OnDeath?.Invoke();
                    break;
                case TriggerZoneType.Coins:
                    OnCoinCollected?.Invoke();
                    triggerZone.gameObject.SetActive(false);
                    Debug.Log("You are collect the coin");
                    break;
                case TriggerZoneType.End:
                    Debug.Log("You are complete the level");
                    OnWin?.Invoke();
                    break;
            }
        }
    }
    
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<TriggerZone>(out var triggerZone))
        {
            switch (triggerZone.Type)
            {
                case TriggerZoneType.Enemy:
                    _attack = true;
                    break;
            }
        }
    }

    private IEnumerator WaitForAttack()
    {
        while (true)
        {
            if (_attack)
            {
                AttackEnemy();
                Action(transform.position, attackRadius, 30);
                yield return new WaitForSeconds(attackReload);
                _attack = false;
            }

            yield return null;
        }
    }
    
    private void Action(Vector3 point, float radius, float damage, bool allTargets = true)
    {
        Collider[] colliders = Physics.OverlapSphere(point, radius);

        if(!allTargets)
        {
            GameObject obj = NearTarget(point, colliders);
            if(obj != null && obj.GetComponent<EnemyScript>())
            {
                obj.GetComponent<EnemyScript>().DamageEnemy(damage);
            }
            return;
        }

        foreach(var hit in colliders) 
        {
            if(hit.GetComponent<EnemyScript>())
            {
                hit.GetComponent<EnemyScript>().DamageEnemy(damage);
            }
        }
    }
	
    private GameObject NearTarget(Vector3 position, Collider[] array) 
    {
        Collider current = null;
        float dist = Mathf.Infinity;

        foreach(var coll in array)
        {
            float curDist = Vector3.Distance(position, coll.transform.position);

            if(curDist < dist)
            {
                current = coll;
                dist = curDist;
            }
        }

        return (current != null) ? current.gameObject : null;
    }
}