using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class FireFighter : SpriteBased
{
    public enum FireFighterType {Single, Burst, Spread, Axe};
    public FireFighterType myType;
    Animator anims;
    [SerializeField] GameObject projectile;
    int loopTime = 0;
    bool canAttack = true;
    [SerializeField] private AudioClip fireSFX;
    [SerializeField] private GameObject fireHydrant;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        
        anims = GetComponent<Animator>();
        StartCoroutine("ShootLoop");

    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }
    void Attack()
    {
        GetComponent<AudioSource>().clip = fireSFX;
        GetComponent<AudioSource>().Play();

        anims.Play("Attack");
        if (myType == FireFighterType.Single)
        {
            FireSingle(transform.forward);
        } else if (myType == FireFighterType.Spread)
        {
            Vector3 tempAngle = transform.forward;
            tempAngle.z -= .30f;
            for (int i = 0; i < 5; i++)
            {
                FireSingle(tempAngle);
                tempAngle.z += .15f;
            }
        } else if (myType == FireFighterType.Burst)
        {
            StartCoroutine("BlastLoop");
        } else if(myType == FireFighterType.Axe)
        {
            LayerMask lm = LayerMask.GetMask("Player");
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 2, lm))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    hit.collider.gameObject.GetComponent<PlayerAttacks>().ChangeHealth(-2);
                }
            }
        }
        canAttack = false;
        StartCoroutine("Cooldown");
    }
    void FireSingle(Vector3 angle)
    {
        GameObject myProj = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        myProj.GetComponent<ProjectileScript>().aim = angle;
        //Debug.Log("blamo");
    }

    IEnumerator ShootLoop()
    {
        Attack();
        float randInterval = UnityEngine.Random.Range(1.3f, 3f);
        yield return new WaitForSeconds(randInterval);
        StartCoroutine("ShootLoop");
    }

    IEnumerator BlastLoop()
    {
        FireSingle(transform.forward);
        loopTime++;
        yield return new WaitForSeconds(.1f);
        if (loopTime <= 2)
        {
            StartCoroutine("BlastLoop");
        } else
        {
            loopTime = 0;
        }
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(3f);
        canAttack= true;
    }
    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(3f);
        Attack();
    }

    public override void Die()
    {
        float tempRand = UnityEngine.Random.Range(0f, 1f);
        if(tempRand > .75f)
        {
            Instantiate(fireHydrant, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
