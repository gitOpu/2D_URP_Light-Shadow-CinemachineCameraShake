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
