using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> mods = new List<StatModifier>();

    private bool isModified = true;
    private float finalValue;
    
    public float getValue()
    {
        
        if (isModified)
        {
            finalValue = GetFinalValue();
            isModified = false;
        }
        return finalValue;
        //return GetFinalValue(); if just doing this, the finalValue will be calculate for multiple times 
        //so we create a boolean variable that makes the getvalue() function only calculate twice,
        //the first time is when just get the buff item and the second time is when remove.
    }

    public void AddModifier(float value, string source)
    {
        StatModifier modToAdd = new StatModifier(value, source);
        mods.Add(modToAdd);
        isModified = true;
    }

    public void RemoveModifier(string source)
    {
        mods.RemoveAll(mods => mods.source == source); //remove everything related to mods where mods.source = source
        isModified = true;
    }

    private float GetFinalValue()
    {
        float finalValue = baseValue;
        foreach (StatModifier mod in mods)
        {
            finalValue += mod.value;
        }
        return finalValue;
    }
    public void SetBaseValue(float value) => baseValue = value;
}

// the main idea is given a class StatModifier which contains value (stats of the object +3 damage -2 attackSpeed for example)
// and the source which is the name of the object. It was like source: +3 damage -2 speed (dragonslayerGreatSword: +5 Damage -3 Speed)
[Serializable]
public class StatModifier
{
    public float value;
    public string source;
    
    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
