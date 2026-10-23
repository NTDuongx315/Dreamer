using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    
    public float getValue() {
        return baseValue;
    }
}
