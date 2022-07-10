using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : EnemyScript
{
    [Header("Move info")] 
    [SerializeField] private float speedMove;
    [SerializeField] private float distance;
    [SerializeField] private Transform bulletParentTransform;
    [SerializeField] private float speedOfBullet;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float rateOfFire = 2f;

    private const string bulletTag = "Bullet";
    
    private new void Start()
    {
        base.Start();
        StartCoroutine(nameof(WaitFire));
        StartCoroutine(MoveEnemy());
    }

    private IEnumerator MoveEnemy()
    {
        Vector3 startPosition = transform.position;
        Vector3 velocity = Vector3.right * speedMove;

        while (true)
        {
            if (transform.position.x > startPosition.x + distance)
                velocity.x = -speedMove;
            else if (transform.position.x < startPosition.x - distance)
                velocity.x = speedMove;

            transform.position += velocity * Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator WaitFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(rateOfFire);
            Fire();
        }
    }

    [ContextMenu("Fire")]
    private void Fire()
    {
        var obj = ObjectPool.SharedInstance.GetPooledObject(bulletTag, true, bulletParentTransform);
        Vector3 position = transform.position;
        obj.transform.position += position;
        obj.GetComponent<BulletScript>().SetProperties(speedOfBullet, player.position, bulletDamage);
    }
}
