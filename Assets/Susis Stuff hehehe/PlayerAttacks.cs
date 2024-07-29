using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class PlayerAttacks : Damageable
{
    public bool hasHydrant;
    [SerializeField] private bool canAttack = true;

    [SerializeField] GameObject fireHydrantPrefab;
    private GameObject currFireHydrant;
    public static PlayerAttacks instance;

    [SerializeField] private GameObject thrownPrefab;
    [SerializeField] private ParticleSystem fireVFX;
    public GameObject hand;
    private Vector3 handHome;
    public bool moving;
    public float swayMod, timer = 0;

    [SerializeField] private AudioSource snapSource, audioSpot;
    [SerializeField] private AudioClip fireShootSFX, snapSFX, throwSFX;

    private Coroutine healthRecharge;
    private void Awake()
    {
        handHome = hand.transform.localPosition;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        swayMod = (float)Math.Sin(timer / 10);
        
        
    }

    public void OnFireHydrant(InputAction.CallbackContext context)
    {
        if (hasHydrant && currFireHydrant != null)
        {
            if (context.started && canAttack)
            {
                Debug.Log("attackA");
                currFireHydrant.GetComponent<FireHydrantInHand>().HitBox(true);
                currFireHydrant.GetComponent<Animator>().Play("Bash");
                hand.GetComponent<Animator>().Play("Bash");
                StartCoroutine("HitBoxCooldown");
                StartCoroutine("MeleeCooldown");
            }
        }
    }
    
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (hasHydrant && currFireHydrant != null)
        {
            Debug.Log("thrown");
            Instantiate(thrownPrefab, transform.position, Quaternion.identity);
            Break();
            StartCoroutine("ThrowCooldown");
            audioSpot.clip = throwSFX;
            audioSpot.Play();
            HeatGauge.TickTemperature(-17, 0);
        }
    }

    public void OnFireAttack(InputAction.CallbackContext context)
    { 
        if (canAttack)
        {
            snapSource.clip = snapSFX;
            snapSource.Play();
            LayerMask lm = LayerMask.GetMask("Firefighter") ;
            RaycastHit hit;
            hand.GetComponent<Animator>().Play("HandFire");
            if(Physics.Raycast(transform.GetChild(0).gameObject.transform.position, 
                transform.forward, 
                out hit, 
                Mathf.Infinity,
                lm))
            {
                Debug.Log("Hit");
                hit.collider.GetComponent<FireFighter>().ChangeHealth(-1);
                Instantiate(fireVFX, hit.transform.position, Quaternion.identity);
                HeatGauge.TickTemperature(1, 0);
                audioSpot.clip = fireShootSFX;
                audioSpot.Play();
            }
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * Mathf.Infinity, Color.yellow, 2f, false);
            StartCoroutine("FireCooldown");
            
        }
    }

    public void Break()
    {
        Destroy(currFireHydrant);
        currFireHydrant = null;
        hasHydrant = false;
    }
    public void AddHydrant()
    {
        if(hasHydrant)
        {
            return;
        }
        hasHydrant = true;
        currFireHydrant = Instantiate(fireHydrantPrefab, this.transform);
    }

    

    public IEnumerator HitBoxCooldown()
    {
        yield return new WaitForSeconds(.5f);
        if (hasHydrant)
        {
            currFireHydrant.GetComponent<FireHydrantInHand>().HitBox(false);
        }
    }

    public IEnumerator MeleeCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
    public IEnumerator ThrowCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(.3f);
        canAttack = true;
    }
    public IEnumerator FireCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(.2f);
        canAttack = true;
    }
    public override void Die()
    {
        UIManager.instance.Status("Death");
    }

    public override void ChangeHealth(int d)
    {
        health += d;
        if (health <= 0)
        {
            Die();
        }
        if(healthRecharge != null)
        {
            StopCoroutine(healthRecharge);
            healthRecharge = StartCoroutine("HealthRecharge");
        }
        UIManager.instance.UpdateFace(health);
        Debug.Log("Health is " + health);
    }

    IEnumerator HealthRecharge()
    {
        yield return new WaitForSeconds(2f);
        health += 1;
        StartCoroutine(HealthRecharge());
        Debug.Log("healed");
    }
}
