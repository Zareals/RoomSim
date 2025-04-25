using UnityEngine;
using DG.Tweening;

public class CameraTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private Ease easeType = Ease.InOutQuad;
    
    [Header("Camera Positions")]
    [SerializeField] private Transform[] cameraPositions;
    
    private Camera mainCamera;
    private int currentPositionIndex = 0;

    private void Awake()
    {
        mainCamera = Camera.main;
        
        // Initialize DOTween (if not already initialized elsewhere)
        DOTween.Init();
    }

    public void TransitionToNextPosition()
    {
        if (cameraPositions.Length == 0) return;
        
        // Get next position index (wraps around)
        currentPositionIndex = (currentPositionIndex + 1) % cameraPositions.Length;
        Transform target = cameraPositions[currentPositionIndex];
        
        // Create the transition sequence
        Sequence transitionSequence = DOTween.Sequence();
        
        // Move position
        transitionSequence.Append(mainCamera.transform.DOMove(target.position, transitionDuration)
            .SetEase(easeType));
            
        // Rotate to match target
        transitionSequence.Join(mainCamera.transform.DORotate(target.eulerAngles, transitionDuration)
            .SetEase(easeType));
            
        // Optional: Change field of view if the camera is perspective
        if (mainCamera.orthographic == false)
        {
            transitionSequence.Join(mainCamera.DOFieldOfView(target.GetComponent<Camera>().fieldOfView, transitionDuration)
                .SetEase(easeType));
        }
    }

    [ContextMenu("Transition To Position")]
    public void TransitionToPosition() => TransitionToPosition(1);
    public void TransitionToPosition(int index)
    {
        if (index < 0 || index >= cameraPositions.Length) return;
        
        currentPositionIndex = index;
        Transform target = cameraPositions[index];
        
        mainCamera.transform.DOMove(target.position, transitionDuration).SetEase(easeType);
        mainCamera.transform.DORotate(target.eulerAngles, transitionDuration).SetEase(easeType);
        
        if (mainCamera.orthographic == false)
        {
            mainCamera.DOFieldOfView(target.GetComponent<Camera>().fieldOfView, transitionDuration)
                .SetEase(easeType);
        }
    }

    // Call this to transition to a specific transform (added at runtime)
    public void TransitionToCustomPosition(Transform target, float customDuration = -1)
    {
        float duration = customDuration > 0 ? customDuration : transitionDuration;
        
        mainCamera.transform.DOMove(target.position, duration).SetEase(easeType);
        mainCamera.transform.DORotate(target.eulerAngles, duration).SetEase(easeType);
        
        if (mainCamera.orthographic == true)
        {
            mainCamera.DOFieldOfView(target.GetComponent<Camera>().fieldOfView, duration)
                .SetEase(easeType);
        }
    }
}