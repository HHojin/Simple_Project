using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Walk,
        Run,
    };

    private NavMeshAgent agent;
    [SerializeField]
    private Vector3 targetPos;

    [SerializeField]
    private bool canMove = false;
    private bool isMoving = false;
    public float doubleClickSecond = 0.25f;
    private bool isOneClick = false;
    private double timer = 0;

    private Animator anim;
    //private Collider coll;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //coll = GetComponent<Collider>();
    }

    void Update()
    {
        FindDestination();

        if (isOneClick && ((Time.time - timer) > doubleClickSecond))
        {
            isOneClick = false;
        }

        if (canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isOneClick)
                {
                    //Debug.Log("One Click");
                    timer = Time.time;
                    isOneClick = true;

                    // 한번 클릭(walk)
                    anim.SetInteger("moveType", (int)State.Walk);
                    agent.speed = 7;
                }
                else if (isOneClick && ((Time.time - timer) < doubleClickSecond))
                {
                    //Debug.Log("Double Click");
                    isOneClick = false;

                    // 두번 클릭(run)
                    anim.SetInteger("moveType", (int)State.Run);
                    agent.speed = 14;
                }

                agent.destination = targetPos;
            }
        }

        if (DestinationArrived()) // 목적지에 도착했을 떄 Idle animation 실행
        {
            anim.SetInteger("moveType", (int)State.Idle);
            isMoving = false;
        }
    }

    private void FindDestination()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Location"))
            {
                //Debug.Log("Coll hit!");
                if (!isMoving)
                {
                    targetPos = new Vector3(hit.point.x, hit.point.y, 0);
                }

                canMove = true;
            }
            else
            {
                canMove = false;
            }
        }
    }

    private bool DestinationArrived()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        if(Physics.Raycast(this.transform.position + Vector3.up, Vector3.down, out hitInfo, 1))
        {
            //if(hitInfo.collider.gameObject.name
        }
    }
}