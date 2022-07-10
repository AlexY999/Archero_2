using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerZoneType
{
    Player,
    Death,
    Coins,
    End,
    Walls,
    Enemy,
    Bullet,
}
public class TriggerZone : MonoBehaviour
{
    [SerializeField] private TriggerZoneType _triggerZoneType;
    
    public TriggerZoneType Type
    {
        get => _triggerZoneType;
        set => _triggerZoneType = value;
    }
}
