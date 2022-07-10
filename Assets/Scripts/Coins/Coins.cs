using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private float startHeight = 3f;
    [SerializeField] private Vector3 startRotation = new Vector3(90, 0, 0);
    [SerializeField] private float rotationEuler = 140f;

    void OnEnable()
    {
        var transformPosition = transform.position;
        transformPosition.y = startHeight;
        transform.position = transformPosition;
        StartCoroutine(nameof(ChangeRotation));
    }

    IEnumerator ChangeRotation()
    {
        transform.rotation = Quaternion.Euler(startRotation);
        
        while (true)
        {
            transform.Rotate(new Vector3(rotationEuler, 0, 0) * Time.deltaTime);
            
            yield return null;
        }
    }
}
