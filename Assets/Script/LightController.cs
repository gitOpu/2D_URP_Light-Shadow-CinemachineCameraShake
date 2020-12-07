using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class LightController : MonoBehaviour
{
    public Light2D explosionLight;
    public float explosionLightIntensity;

    void Start()
    {
        DOVirtual.Float(0, explosionLightIntensity, .05f, ChangeLight).OnComplete(() => DOVirtual.Float(explosionLightIntensity, 0, .1f, ChangeLight));
        StartCoroutine(DestroyGameObject());
    }

    void ChangeLight(float x)
    {
        explosionLight.intensity = x;
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
