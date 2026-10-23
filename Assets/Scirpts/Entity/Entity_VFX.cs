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
    
    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    public void CreateOnHitVfx(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;

        if (entity.facingDir == -1 && isCrit) vfx.transform.Rotate(0, 180, 0);
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
