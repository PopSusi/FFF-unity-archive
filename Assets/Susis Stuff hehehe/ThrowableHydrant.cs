using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableHydrant : SpriteBased
{

    [SerializeField] GameObject foamBlast;

    private void Start()
    {
        base.Start();
        GetComponent<Rigidbody>().AddForce(PlayerController.instance.gameObject.transform.forward * 900);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(foamBlast, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
