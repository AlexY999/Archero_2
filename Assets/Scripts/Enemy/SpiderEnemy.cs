using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : EnemyScript
{
    [Header("Move info")] 
    [SerializeField] private float speedMove;
    [Header("Move speed")] 
    [SerializeField] private float moveSpeed = 20;
    [Header("Rotation speed")] 
    [SerializeField] private float rotationSpeed = 20;
    [Header("Damage value")] 
    [SerializeField] private float damageValue = 4;
    [SerializeField] private float attackReload = 1.5f;

    private GameObject _attackObject;

    private new void Start()
    {
        base.Start();
        StartCoroutine(MoveEnemy());
        StartCoroutine(nameof(WaitForAttack));
    }

    private IEnumerator MoveEnemy()
    {
        while (true)
        {
            var lookDir = player.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(lookDir),rotationSpeed*Time.deltaTime);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            yield return null;
        }
    }
    
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<TriggerZone>(out var triggerZone))
        {
            switch (triggerZone.Type)
            {
                case TriggerZoneType.Player:
                    _attackObject = collision.gameObject;
                    break;
            }
        }
    }
    
    private IEnumerator WaitForAttack()
    {
        while (true)
        {
            if (_attackObject != null)
            {
                _attackObject.GetComponent<PlayerMovement>().Damage(damageValue);

                yield return new WaitForSeconds(attackReload);
                _attackObject = null;
            }

            yield return null;
        }
    }
}
