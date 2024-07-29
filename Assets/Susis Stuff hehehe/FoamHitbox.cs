using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FoamHitbox : SpriteBased
{
    private void Awake()
    {
        GetComponent<AudioSource>().Play();
        StartCoroutine("DieDelay");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hotspot"))
        {
            HitboxDelay();
            HeatGauge.instance.HotspotClearPlayer();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Firefighter"))
        {
            HitboxDelay();
            other.GetComponent<FireFighter>().ChangeHealth(-3);
        }
    }


    IEnumerator HitboxDelay()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Collider>().enabled = false;
    }

    IEnumerator DieDelay()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
