using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Buff
{
    public StatType type;
    public float value;
}
public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity_Stats statsToModify; //chỉ nên được gọi khi player collide(nhặt được) items.

    [Header("Buff details")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration;
    [SerializeField] private bool canbeUse = true;
    
    [Header("Floating Movement")]
    [SerializeField] private float floatingSpeed;
    [SerializeField] private float floatingRange;
    private Vector3 startPos;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        startPos = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatingSpeed) * floatingRange;  //the candle will move up-down just like sin() graph
        transform.position = startPos + new Vector3(0,yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canbeUse) return;
        statsToModify = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCo(buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        canbeUse = false;
        sr.color = Color.clear;

        foreach (var buff in buffs)
        {
            statsToModify.GetStatByType(buff.type).AddModifier(buff.value,buffName);
        }
        
        yield return new WaitForSeconds(duration);
    
        foreach (var buff in buffs)
        {
            statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
        }
        
        Destroy(gameObject);
    }
}
