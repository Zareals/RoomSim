using UnityEngine;

public class OrthoCameraController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;
    public bool allowRotation = true;
    [Range(-180f, 180f)] public float minRotationY = -180f;
    [Range(-180f, 180f)] public float maxRotationY = 180f;
    public bool snapToAngles = false;
    [Range(1f, 90f)] public float snapAngleInterval = 45f;
    public float rotationDamping = 5f;
    
    [Header("Initial Rotation")]
    public Vector3 initialRotation = new Vector3(32f, -135f, 0f);
    
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minOrthoSize = 2f;
    public float maxOrthoSize = 20f;
    public float zoomDamping = 5f;
    
    [Header("Pan Settings")]
    public float panSpeed = 0.1f;
    public bool allowPanning = true;
    public Vector2 panLimitsX = new Vector2(-10f, 10f);
    public Vector2 panLimitsZ = new Vector2(-10f, 10f);
    
    [Header("Target (Optional)")]
    public Transform target;
    public Vector3 targetOffset = Vector3.zero;
    
    private Camera orthoCamera;
    private float targetRotationY;
    private Vector3 dragOrigin;
    private Vector3 cameraStartPosition;
    
    void Start()
    {
        orthoCamera = GetComponent<Camera>();
        if (orthoCamera == null)
        {
            Debug.LogError("No Camera component found!");
            return;
        }
        
        if (!orthoCamera.orthographic)
        {
            Debug.LogWarning("Camera is not orthographic. Forcing orthographic mode.");
            orthoCamera.orthographic = true;
        }
        
        // Set initial rotation
        transform.rotation = Quaternion.Euler(initialRotation);
        targetRotationY = initialRotation.y;
        
        orthoCamera.orthographicSize = Mathf.Clamp(orthoCamera.orthographicSize, minOrthoSize, maxOrthoSize);
        cameraStartPosition = transform.position;
        
        if (target != null)
        {
            transform.position = target.position + targetOffset;
        }
    }
    
    void Update()
    {
        HandleZoom();
        HandleRotation();
        HandlePanning();
    }
    
    void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            float newSize = orthoCamera.orthographicSize - scrollInput * zoomSpeed;
            orthoCamera.orthographicSize = Mathf.Clamp(newSize, minOrthoSize, maxOrthoSize);
        }
    }
    
    void HandleRotation()
    {
        if (!allowRotation) return;
        
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            float rotationInput = Input.GetAxis("Mouse X");
            targetRotationY += rotationInput * rotationSpeed;
            
            // Apply rotation limits
            targetRotationY = Mathf.Clamp(targetRotationY, minRotationY, maxRotationY);
            
            // Apply snapping if enabled
            if (snapToAngles)
            {
                targetRotationY = Mathf.Round(targetRotationY / snapAngleInterval) * snapAngleInterval;
            }
        }
        
        // Smooth rotation while maintaining initial X and Z rotations
        float currentY = transform.eulerAngles.y;
        float newY = Mathf.LerpAngle(currentY, targetRotationY, Time.deltaTime * rotationDamping);
        transform.rotation = Quaternion.Euler(
            initialRotation.x, 
            newY, 
            initialRotation.z
        );
    }
    
    void HandlePanning()
    {
        if (!allowPanning) return;
        
        // Middle mouse button or Alt + Left mouse button
        if (Input.GetMouseButtonDown(2) || (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0)))
        {
            dragOrigin = GetMouseWorldPosition();
        }
        
        if (Input.GetMouseButton(2) || (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0)))
        {
            Vector3 difference = dragOrigin - GetMouseWorldPosition();
            transform.position += difference;
            
            // Apply pan limits if target is not set
            if (target == null)
            {
                Vector3 pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, panLimitsX.x, panLimitsX.y);
                pos.z = Mathf.Clamp(pos.z, panLimitsZ.x, panLimitsZ.y);
                transform.position = pos;
            }
        }
    }
    
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = orthoCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        plane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    
    public void ResetToTarget()
    {
        if (target != null)
        {
            transform.position = target.position + targetOffset;
        }
        else
        {
            transform.position = cameraStartPosition;
        }
        
        // Reset to initial rotation
        targetRotationY = initialRotation.y;
        transform.rotation = Quaternion.Euler(initialRotation);
    }
    
    public void ToggleRotation(bool enabled)
    {
        allowRotation = enabled;
    }
    
    public void TogglePanning(bool enabled)
    {
        allowPanning = enabled;
    }
    
    public void SetRotationLimits(float min, float max)
    {
        minRotationY = Mathf.Clamp(min, -180f, 180f);
        maxRotationY = Mathf.Clamp(max, -180f, 180f);
        targetRotationY = Mathf.Clamp(targetRotationY, minRotationY, maxRotationY);
    }
}