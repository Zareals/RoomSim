using UnityEngine;

public class RotateOnYAxis : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 90f; // Degrees per second
    [SerializeField] private bool rotateClockwise = true;
    [SerializeField] private bool randomizeInitialRotation = false;
    [SerializeField] private bool rotateOnlyWhenVisible = true;

    private void Start()
    {
        if (randomizeInitialRotation)
        {
            transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        }
    }

    private void Update()
    {
        if (rotateOnlyWhenVisible && !IsVisible()) return;

        float direction = rotateClockwise ? 1f : -1f;

        transform.Rotate(0, direction * rotationSpeed * Time.deltaTime, 0);
    }

    private bool IsVisible()
    {
        return GetComponent<Renderer>().isVisible;
    }
}