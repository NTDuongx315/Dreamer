using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;
    
    [Header("On  Damage VFX")] 
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = .2f;
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;

    [Header("On Damage VFX")] 
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject critHitVfx;
    
    [Header("Element Colors")]
    [SerializeField] private Color chillVfx = Color.cyan;
    private Color originalHitVfxColor;
    
    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
        originalHitVfxColor = hitVfxColor;
    }

    public void PlayOnStatusVfx(float duration, ElementalType element)
    {
        if (element == ElementalType.Ice) StartCoroutine(PlayStatusVfxCo(duration, chillVfx));
    }

    private IEnumerator PlayStatusVfxCo(float duration, Color color)
    {
        float tickInterval = .2f;
        float Timer = 0;

        Color lightColor = color * 1.2f;
        Color darkColor = color * 0.8f;

        bool toggle = false;
        while (Timer < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;
            
            yield return new WaitForSeconds(tickInterval);
            Timer += tickInterval;
        }

        sr.color = Color.white;
    }

    public void CreateOnHitVfx(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;

        if (entity.facingDir == -1 && isCrit) vfx.transform.Rotate(0, 180, 0);
    }

    public void UpdateOnHitColor(ElementalType element)
    {
        switch (element)
        {
            case ElementalType.Ice:
                hitVfxColor = chillVfx;
                break;
            case ElementalType.None:
                hitVfxColor = originalHitVfxColor;
                break;
        }
    }

    public void PlayOnDamageVFx()
    {
        if(onDamageVfxCoroutine != null)
        {
            StopCoroutine(onDamageVfxCoroutine);
        }

        onDamageVfxCoroutine = StartCoroutine(OnDamageVFxCo());
    }

    private IEnumerator OnDamageVFxCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
    }
}
