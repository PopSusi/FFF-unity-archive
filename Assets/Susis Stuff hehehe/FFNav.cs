using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class FFNav : MonoBehaviour
{
    private Vector3 target;

    [SerializeField] int playerRadius = 5;

    NavMeshAgent agent;
    Animator anim;
    // Start is called before the first frame update
    private void Awake()
    {
        HeatGauge.firefightersCount++;

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        int tempRand = Random.Range(0, 1);
        Reposition();
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.velocity.magnitude > 0.2f)
        {
            anim.SetBool("Moving", true);
        } else
        {
            anim.SetBool("Moving", false);
            StartCoroutine("RandomStandTime");
        }
    }
    public void Reposition()
    {
        Vector3 temp = Random.insideUnitCircle * playerRadius;
        temp.z = temp.y;
        temp.y = 1;
        target = PlayerController.instance.transform.position + temp;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, 3, NavMesh.AllAreas))
        {
            agent.destination = hit.position;
            Debug.DrawLine(transform.position, target, Color.green, 2f);
        }
        else
        {
            Reposition();
        }
        Debug.DrawLine(transform.position, target, Color.red, .3f);
    }

    IEnumerator RandomStandTime()
    {
        float tempRand = Random.Range(.3f, 1);
        yield return new WaitForSeconds(tempRand);
        Reposition();
    }
}
