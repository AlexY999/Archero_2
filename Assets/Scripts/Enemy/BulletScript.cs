using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    private float _speedOfBullet = 0;
    private float _rotationSpeed = 20;
    private float _damageValue = 1;
    private Vector3 _targetPosition;
    
    private void OnDisable()
    {
        transform.position = Vector3.up;
        _rigidbody.velocity = Vector3.zero;
    }

    public void SetProperties(float speed, Vector3 targetPosition, float bulletDamage)
    {
        _speedOfBullet = speed;
        _targetPosition = targetPosition;
        _damageValue = bulletDamage;
        
        StartCoroutine(nameof(MoveBullet));
    }

    private IEnumerator MoveBullet()
    {
        var lookDir = _targetPosition - transform.position;
        lookDir.y = 0;

        while (gameObject.activeSelf)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir),
                _rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * _speedOfBullet * Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<TriggerZone>(out var triggerZone))
        {
            switch (triggerZone.Type)
            {
                case TriggerZoneType.Walls:
                    gameObject.SetActive(false);
                    break;
                case TriggerZoneType.Player:
                    collision.gameObject.GetComponent<PlayerMovement>().Damage(_damageValue);
                    gameObject.SetActive(false);
                    break;
            }
        }
    }

}
