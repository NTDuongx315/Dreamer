using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    [Header("Image echo vfx")]
    [Range(.01f, .2f)]
    [SerializeField] private float imageEchoInterval =.1f;
    [SerializeField] private GameObject imageEchoPrefab;
    private Coroutine imageEchoCo;

    public void DoImageEchoEffect(float duration)
    {
        if (imageEchoCo != null) StopCoroutine(imageEchoCo);
        imageEchoCo = StartCoroutine(imageEchoEffectCo(duration));
    }

    //FadeCo is the function that make the spriteframe blur after time but this one is the function which will follow up 
    // the dash animation of the player and create the dư ảnh đằng sau player khi dash.
    private IEnumerator imageEchoEffectCo(float duration)
    {
        float timeTracker = 0;
        while (timeTracker < duration)
        {
            CreateImageEcho();
            
            yield return new WaitForSeconds(imageEchoInterval);
            timeTracker += imageEchoInterval;
        }
    }

    private void CreateImageEcho()
    {
        GameObject imageEcho = Instantiate(imageEchoPrefab, transform.position, transform.rotation);
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}
