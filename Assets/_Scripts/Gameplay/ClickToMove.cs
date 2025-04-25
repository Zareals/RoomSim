using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float stoppingDistance = 0.2f;
    [SerializeField] private float randomMoveRadius = 10f;
    [SerializeField] private float wanderInterval = 5f;

    [Header("Animation")]
    [SerializeField] private string speedParameter = "Speed";
    [SerializeField] private float animationDampTime = 0.1f;

    [Header("Feedback")]
    [SerializeField] private ParticleSystem objectClickEffect;
    [SerializeField] private ParticleSystem groundClickEffect;
    [SerializeField] private AudioClip objectClickSound;
    [SerializeField] private AudioClip groundClickSound;

    private NavMeshAgent agent;
    private Camera mainCamera;
    private Animator animator;
    private bool isMovingTowardInteractable = false;
    private Coroutine wanderCoroutine;
    private Vector3 lastClickPoint;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        SetupAgent();
    }

    private void Start()
    {
        wanderCoroutine = StartCoroutine(WanderRoutine());
    }

    private void SetupAgent()
    {
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = true;
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimations();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                lastClickPoint = hit.point;
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                
                if (interactable != null)
                {
                    isMovingTowardInteractable = true;
                    MoveToTarget(hit.transform.position);
                    interactable.OnApproachStart();
                    PlayFeedback(hit.point, true);
                }
                else
                {
                    isMovingTowardInteractable = false;
                    MoveToTarget(hit.point);
                    PlayFeedback(hit.point, false);
                }
            }
        }
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(wanderInterval);
            
            if (!isMovingTowardInteractable && agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 randomPoint = GetRandomNavMeshPoint(transform.position);
                MoveToTarget(randomPoint);
                PlayFeedback(randomPoint, false);
            }
        }
    }

    private void MoveToTarget(Vector3 targetPosition)
    {
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);
        }
    }

    private Vector3 GetRandomNavMeshPoint(Vector3 aroundPoint)
    {
        Vector3 randomDirection = Random.insideUnitSphere * randomMoveRadius;
        randomDirection += aroundPoint;
        
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, randomMoveRadius, NavMesh.AllAreas);
        
        return hit.position;
    }

    private void PlayFeedback(Vector3 position, bool isObjectClick)
    {
        ParticleSystem effect = isObjectClick ? objectClickEffect : groundClickEffect;
        AudioClip sound = isObjectClick ? objectClickSound : groundClickSound;

        if (effect != null)
            Instantiate(effect, position, Quaternion.identity);

        if (sound != null)
            AudioSource.PlayClipAtPoint(sound, position);
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;
        
        float normalizedSpeed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat(speedParameter, normalizedSpeed, animationDampTime, Time.deltaTime);
    }

    public void OnReachedInteractable()
    {
        isMovingTowardInteractable = false;
    }
}