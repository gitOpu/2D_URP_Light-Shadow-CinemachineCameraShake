![](Doc/Cover.gif)

#  URP on 2D, Light, Glow, Shadow, HDR, Cinemachine Camera Shake on Fire

1. Create new 2D Project, by default URP and Post Processing facilities not integrated to your 2D project, that's why we need to add this to our project from Windows -> PackageManager
2. Right click on your project Create->Render->Universal Renderer Pipeline-> Universal Renderer Pipeline (Forward) Asset.  Create 2D Renderer Pipeline-> 2D Universal Renderer, add this to previously created URP Asset (by default its use 3D Renderer), also check HDR over the URP Asset. Go to your Edit->Project Setting->Graphics-> Scriptable Render Pipeline Settings, assign newly created one to it.
3. Go to your main camera, Check Post Processing, add Universal Additional Camera Data (Script)
4.a. Create New Game Object, Named it Volume, add Volume to it, select mode to Global.
b. Add new profile to Volume, Add Override->Bloom, Check Threshold, Intensity, Scatter, Tint, Dirt Texture and Dirt Intensity, change Intensity to 12, Tint Color Orange, other as you like.
4. Add anything to your Scene (2D or 3D Object), Create new Marital-> change Base Map and Emission Map HDR Color.

## 2D Randerer insted of 3D Randerer
After installation URP on your 2D project, you used to create an UniversalRenderPipelineAsset, when you create this asset files its automatically create a Renderer, which is useful for 3D project but not in 2D project, in the 2D project you have to also create a 2D Renderer from Create->Rendering->URP-> 2D Render. Now replace default Renderer by this brand new 2D Rander to your UniversalRenderPipelineAsset, Now Add this asset to your project graphics [Edit->Project Setting->Graphics->Scriptable RP Settings(select the asset)] 

![Details](https://answers.unity.com/questions/1719967/cant-use-urp-2d-lights.html?childToView=1806547#answer-1806547)



## Add Light
When you add URP to your project, its turns your games elements into black or magenta because its use Default Materials, To Fix this add 2D light from Asset->Light->2D Light->Global Light. Go to Edit->Renderer Pipeline->URP-> Upgrade Project Material to URP Material, it will change your Material to Lit Material.


## Cast Shadow
To cast shadow to you GO, add Shadow Caster 2D to that GO, adjust your light shadow casting option.

## Add Cinemamachine Camera
1. Add Cinemachinec Package from Package Manager
2. Unity Main Menu Create a Virtual Camera, it will automatically crate your camera as Virtual camera by adding CinemachjineBrain to you camera and a CM Virtual object to the project.
3. add you main game player object to CinemachineVirtulaCamera-> Follow.
---------------------------------------------------------------
For camera shake add Cinemachine Impulse Listener and Source to your Virtual machine


## Add Pixel Perfect Camera
For Pixel Art Game add pixel perfect camera both Main Camera and CM Virtual Camera(Self PPC)


## Movement
> Target: Player game object 
* Player can fire, move left and right

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private Rigidbody2D rb;
    ParticleSystem gun;
    Animator anim;
    public float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gun = GetComponentInChildren<ParticleSystem>();
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        //anim.SetFloat("x", x);

        transform.position += new Vector3(x * speed * Time.deltaTime, 0,0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
                gun.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            gun.Stop();
        }
    }
}

```

## ParticleCollision
> Target: Laser or gun fire particle. 
* Particle on collision enter instantiate explosion 
* Camera shake
* Add force to the obstacle 

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public CinemachineVirtualCamera cam;
    public GameObject explosionPrefab;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        GameObject explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);

        ParticleSystem p = explosion.GetComponent<ParticleSystem>();
        var pmain = p.main;

        cam.GetComponent<CinemachineImpulseSource>().GenerateImpulse();

        if (other.GetComponent<Rigidbody2D>() != null)
            other.GetComponent<Rigidbody2D>().AddForceAtPosition(collisionEvents[0].intersection * 10 - transform.position, collisionEvents[0].intersection + Vector3.up);

    }
}
```


## LightController
> Target : Explosion particle 
* GoTween instantiate particle prefab
* Remove this game object

```C#
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

```
