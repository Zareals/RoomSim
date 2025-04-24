using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private NeedData _affectedNeed;
    [SerializeField] private float _restoreAmount = 20f;
    
    private void OnMouseDown()
    {
        NeedsManager.Instance.RestoreNeed(_affectedNeed, _restoreAmount);
        Debug.Log("Interacted with " + gameObject.name);
        // Play object-specific animation here
    }
}
