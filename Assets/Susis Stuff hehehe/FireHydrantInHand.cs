using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireHydrantInHand : MonoBehaviour
{
    [SerializeField] private int durability = 3; //max is 3
    private Collider hitBox;
    public Animator fhAnims;
    // Start is called before the first frame update
    void Awake()
    {

        hitBox = GetComponent<Collider>();
    }
    public void HitBox(bool active)
    {
        hitBox.enabled = active;
    }
    private void FFCheck(Collider other)
    {
        if (other.gameObject.CompareTag("Firefighter"))
        {
            Debug.Log("Hit Enemy");
            durability--;
            other.GetComponent<FireFighter>().ChangeHealth(-4);
            GetComponent<AudioSource>().Play();
            if (durability <= 0)
            {
                PlayerAttacks.instance.Break();
                PlayerAttacks.instance.hand.GetComponent<Animator>().Play("New State");
            }
            hitBox.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        FFCheck(other);
    }
    public void MeleeAnim()
    {
        fhAnims.Play("Bash");
    }
}
