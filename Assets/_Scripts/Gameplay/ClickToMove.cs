using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float stoppingDistance = 0.2f;

    [Header("Animation Parameters")]
    [SerializeField] private string speedParameter = "Speed";
    [SerializeField] private float animationDampTime = 0.1f;

    [Header("Visual Feedback")]
    [SerializeField] private ParticleSystem clickEffect;
    [SerializeField] private AudioClip moveSound;

    private NavMeshAgent agent;
    private Camera mainCamera;
    private Animator animator;
    private float currentSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();

        SetupAgent();
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
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
                {
                    agent.SetDestination(navHit.position);
                    PlayClickFeedback(navHit.position);
                }
            }
        }
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;


        float normalizedSpeed = agent.velocity.magnitude / agent.speed;
        
        animator.SetFloat(speedParameter, normalizedSpeed, animationDampTime, Time.deltaTime);
    }

    private void PlayClickFeedback(Vector3 position)
    {
        if (clickEffect != null)
            Instantiate(clickEffect, position, Quaternion.identity);

        if (moveSound != null)
            AudioSource.PlayClipAtPoint(moveSound, position);
    }
}