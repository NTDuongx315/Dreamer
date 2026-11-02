using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destryDisplay = 1;
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
    

    private void Start()
    {
        ApplyRandomOffSet();
        ApplyRandomRotation();
        if (autoDestroy) Destroy(gameObject, destryDisplay);
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
