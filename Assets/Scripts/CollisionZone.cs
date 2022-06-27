using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionZoneType
{
    Enemy,
    Thorns
}

public class CollisionZone : MonoBehaviour
{
    [SerializeField] private CollisionZoneType _collisionZoneType;
    public CollisionZoneType Type => _collisionZoneType;
}
