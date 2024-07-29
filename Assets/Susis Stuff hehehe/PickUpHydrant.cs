using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PickUpHydrant : SpriteBased
{
    private float life;
    private float lifespan = 6f;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        life += Time.deltaTime;
        if (life > lifespan)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.gameObject.GetComponent<PlayerAttacks>().hasHydrant)
            {
                other.gameObject.GetComponent<PlayerAttacks>().AddHydrant();
                Destroy(gameObject);
            }
        }
    }
}
