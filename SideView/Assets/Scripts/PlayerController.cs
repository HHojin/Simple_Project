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

    private NavMeshAgent m_agent;
    private float m_agentSpeed;
    private float m_agentoffMeshLinkSpeed = 7.0f;
    [SerializeField]
    private Vector3 m_targetPos;
    private Vector3 m_tmpTargetPos;

    [SerializeField]
    private bool m_canMove = false;
    [SerializeField]
    private bool m_isMoving = false;
    public float m_doubleClickSecond = 0.25f;
    private bool m_isOneClick = false;
    private double m_timer = 0;

    private Animator m_animator;
    private Transform m_transform;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
        m_transform = GetComponent<Transform>();
    }

    void Update()
    {
        FindDestination();

        if (m_isOneClick && ((Time.time - m_timer) > m_doubleClickSecond))
        {
            m_isOneClick = false;
        }

        if (m_canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!m_isOneClick)
                {
                    //Debug.Log("One Click");
                    m_timer = Time.time;
                    m_isOneClick = true;

                    // walk
                    m_animator.SetInteger("moveType", (int)State.Walk);
                    m_agentSpeed = 7.0f;
                }
                else if (m_isOneClick && ((Time.time - m_timer) < m_doubleClickSecond))
                {
                    //Debug.Log("Double Click");
                    m_isOneClick = false;

                    // run
                    m_animator.SetInteger("moveType", (int)State.Run);
                    m_agentSpeed = 14.0f;
                }

                m_agent.destination = m_targetPos;
                m_isMoving = true;
            }
        }

        if (m_isMoving)
        {
            if (m_agent.isOnOffMeshLink)
            {
                m_agent.speed = m_agentoffMeshLinkSpeed;
            }
            else
            {
                m_agent.speed = m_agentSpeed;
            }

            if (DestinationArrived()) // 목적지에 도착했을 떄 Idle animation 실행
            {
                m_animator.SetInteger("moveType", (int)State.Idle);
                m_isMoving = false;
            }
        }
    }

    private void FindDestination()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            //if (hit.transform.CompareTag("Location"))
            if (hit.collider != null)
            {
                m_tmpTargetPos = new Vector3(hit.point.x, hit.point.y, 0); // screen 좌표상 위치에서
                Physics.Raycast(m_tmpTargetPos, Vector3.down, out RaycastHit _hit); // Vector3.down 방향

                m_targetPos = _hit.point;

                m_canMove = true;
            }
            else
            {
                m_canMove = false;
            }
        }

    }

    private bool DestinationArrived()
    {
        if (!m_agent.pathPending)
        {
            if (m_agent.remainingDistance <= m_agent.stoppingDistance)
            {
                if (!m_agent.hasPath || m_agent.velocity.sqrMagnitude == 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
}