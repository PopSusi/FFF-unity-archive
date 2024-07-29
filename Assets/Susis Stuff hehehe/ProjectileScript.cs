using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Vector3 aim;
    private float life;
    private float lifespan = 3f;

    private void Start()
    {
        transform.GetChild(0).rotation = Quaternion.Euler(aim * -1);
    }
    void Update()
    {
        transform.position += aim * Time.deltaTime * 5;
        life += Time.deltaTime;
        if(life > lifespan)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("HitPlayer");
            other.gameObject.GetComponent<PlayerAttacks>().ChangeHealth(-1);
            Destroy(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
}
