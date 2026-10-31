using System;
using System.Collections;
using UnityEngine;

public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer sr;
    
    [Header("Buff details")] 
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

        StartCoroutine(BuffCo(buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        canbeUse = false;
        sr.color = Color.clear;
        Debug.Log("applied" + duration);
        
        yield return new WaitForSeconds(duration);

        Debug.Log("buff removed");
        
        Destroy(gameObject);
    }
}
