using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : ObstacleBase
{
    private enum States { Patrol, Block, Chase, Return }

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float chaseDuration = 1.5f;
    [SerializeField] private float rotationSpeed = 5f;
    private NavMeshAgent agent;
    private States currentState;
    private Vector3 basePos;


    [Header("Patrol Settings")]
    [SerializeField] private bool isPatroller = false;
    [SerializeField] private float patrolWaitTime = 1f;
    [SerializeField] private List<Transform> patrolPoints = new();
    private int currentPatrolIndex = 0;
    private bool isWaitingAtWaypoint = false;
    private Coroutine patrolWaitCoroutine;
    
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        basePos = transform.position;

        if (isPatroller && patrolPoints.Count > 0)
        {
            currentState = States.Patrol;
            MoveToPatrolPoint(0);
        }
        else
            currentState = States.Block;
    }

    private void Update()
    {
        // Player's Detection
        if (currentState != States.Chase && currentState != States.Return)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, BeeController.Instance.transform.position);

            if (distanceToPlayer <= detectionRadius)
                StartCoroutine(ChaseAndReturnRoutine());
        }

        if (currentState == States.Patrol)
            HandlePatrolUpdate();

        // Rotation
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            var moveDirection = new Vector3(agent.velocity.x, agent.velocity.y, 0f).normalized;
            var targetRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator ChaseAndReturnRoutine()
    {
        currentState = States.Chase;
        float timer = 0f;

        SoundManager.Instance.LaunchChaseMusic(chaseDuration);
        
        // CHASE
        while (timer < chaseDuration)
        {
            if (BeeController.Instance != null)
                agent.SetDestination(BeeController.Instance.transform.position);
            
            timer += Time.deltaTime;
            yield return null;
        }

        currentState = States.Return;
        agent.SetDestination(basePos);

        // RETURN
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            yield return null; 

        if (isPatroller)
            ResumePatrolFromNearestPoint();
        else
            currentState = States.Block;
    }

    protected override void OnCollisonReaction(BeeController bee)
    {
        base.OnCollisonReaction(bee);
        bee.HitFeedback();
        GameManager.Instance.RemoveAHeart();
    }

    #region Patrol Logic

    private void HandlePatrolUpdate()
    {
        if (patrolPoints.Count == 0 || isWaitingAtWaypoint) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            patrolWaitCoroutine = StartCoroutine(WaitAndMoveToNextPoint());
    }

    private IEnumerator WaitAndMoveToNextPoint()
    {
        isWaitingAtWaypoint = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(patrolWaitTime);

        agent.isStopped = false;
        isWaitingAtWaypoint = false;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        MoveToPatrolPoint(currentPatrolIndex);
    }

    private void MoveToPatrolPoint(int index)
    {
        if (patrolPoints.Count == 0 || patrolPoints[index] == null) return;
        agent.SetDestination(patrolPoints[index].position);
    }

    private void InterruptPatrolForChase()
    {
        if (patrolWaitCoroutine != null)
        {
            StopCoroutine(patrolWaitCoroutine);
            patrolWaitCoroutine = null;
        }
        isWaitingAtWaypoint = false;
        agent.isStopped = false; 
    }

    private void ResumePatrolFromNearestPoint()
    {
        if (patrolPoints.Count == 0) return;

        int nearestIndex = 0;
        float minDistance = float.MaxValue;

        for (int i = 0; i < patrolPoints.Count; i++)
        {
            if (patrolPoints[i] == null) continue;
            float dist = Vector3.Distance(transform.position, patrolPoints[i].position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearestIndex = i;
            }
        }

        currentPatrolIndex = nearestIndex;
        currentState = States.Patrol;
        MoveToPatrolPoint(currentPatrolIndex);
    }

    #endregion

    private void OnDrawGizmos()
    {
        // Draw Detection Range
        Gizmos.color = new Color(1f, 0f, 0f, 0.15f);
        Gizmos.DrawSphere(transform.position, detectionRadius);

        // Draw Patrol Path
        if (isPatroller && patrolPoints != null && patrolPoints.Count > 0)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < patrolPoints.Count; i++)
            {
                if (patrolPoints[i] == null) continue;

                Gizmos.DrawSphere(patrolPoints[i].position, 0.25f);

                int nextIndex = (i + 1) % patrolPoints.Count;
                if (patrolPoints[nextIndex] != null)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[nextIndex].position);
                }
            }
        }
    }
}
