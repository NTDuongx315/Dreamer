using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1;
    
    //idea is get access to the sprite renderer in the animator so we can manage each sprite on each frame,
    //the speed of the fading animation.
    private SpriteRenderer sr;
    [Header("Fade effect")]
    [SerializeField] private bool canFade;
    [SerializeField] private float fadeSpeed = 1;
     
    [Space]
    [SerializeField] private bool randomOffSet = true;
    [SerializeField] private bool randomRotate = true;
    
    [Header("Random rotation")]
    [SerializeField] private float minRotation = 0;
    [SerializeField] private float maxRotation = 360;

    [Header("Random damage positions")] 
    [SerializeField] private float xMinOffSet = -.3f;
    [SerializeField] private float xMaxOffSet = .3f;
    [Space]
    [SerializeField] private float yMinOffSet = -.3f;
    [SerializeField] private float yMaxOffSet = .3f;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (canFade) StartCoroutine(FadeCo());
        
        ApplyRandomOffSet();
        ApplyRandomRotation();
        if (autoDestroy) Destroy(gameObject, destroyDelay);
    }

    private IEnumerator FadeCo()
    {
        Color targetColor = Color.white;
        //khi player bắt đầu dash, thì vòng while này sẽ chạy và những gì nó làm đó là 
        //với mỗi spriteframe sẽ mờ dần follow theo player
        while (targetColor.a > 0)
        {
            targetColor.a -= fadeSpeed * Time.deltaTime;
            sr.color = targetColor;
            yield return null;
        }
        sr.color = targetColor;
    }

    private void ApplyRandomOffSet()
    {
        if (!randomOffSet) return;
        float xOffset = Random.Range(xMinOffSet, xMaxOffSet);
        float yOffset = Random.Range(yMinOffSet, yMaxOffSet);
        
        transform.position = transform.position + new Vector3(xOffset, yOffset);
    }

    private void ApplyRandomRotation()
    {
        if(!randomRotate) return;
        float rotation = Random.Range(minRotation, maxRotation);
        transform.Rotate(0,0,rotation);
    }
}
