using UnityEngine;

public class OrthoCameraController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;
    public bool allowRotation = true;
    
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
    private float currentRotationY = 0f;
    private float desiredOrthoSize;
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
        
        desiredOrthoSize = orthoCamera.orthographicSize;
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
            desiredOrthoSize -= scrollInput * zoomSpeed;
            desiredOrthoSize = Mathf.Clamp(desiredOrthoSize, minOrthoSize, maxOrthoSize);
        }
        
        // Smooth zoom
        orthoCamera.orthographicSize = Mathf.Lerp(
            orthoCamera.orthographicSize, 
            desiredOrthoSize, 
            Time.deltaTime * zoomDamping
        );
    }
    
    void HandleRotation()
    {
        if (!allowRotation) return;
        
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            float rotationInput = Input.GetAxis("Mouse X");
            currentRotationY += rotationInput * rotationSpeed;
            
            // For orthographic, we typically only want to rotate around Y axis
            transform.rotation = Quaternion.Euler(90f, currentRotationY, 0f);
        }
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
            currentRotationY = 0f;
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        else
        {
            transform.position = cameraStartPosition;
            currentRotationY = 0f;
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
    
    // Call this from a UI button if needed
    public void ToggleRotation(bool enabled)
    {
        allowRotation = enabled;
    }
    
    // Call this from a UI button if needed
    public void TogglePanning(bool enabled)
    {
        allowPanning = enabled;
    }
}