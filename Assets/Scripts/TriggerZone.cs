using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerZoneType
{
    Death,
    Coins,
    End,
}
public class TriggerZone : MonoBehaviour
{
    [SerializeField] private TriggerZoneType _triggerZoneType;
    public TriggerZoneType Type => _triggerZoneType;
}
