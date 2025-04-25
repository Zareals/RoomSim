using EasyTransition;
using UnityEngine;

public enum ObjectType
{
    Food,
    Water,
    bed
}

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private ObjectType _objectType;
    [SerializeField] private NeedData _affectedNeed;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private float _restoreAmount = 20f;
    [SerializeField] private ParticleSystem pEffect;
    [SerializeField] private TransitionSettings transition;
    [SerializeField] private TransitionManager transitionManager;
    
    private void OnMouseDown()
    {
        NeedsManager.Instance.RestoreNeed(_affectedNeed, _restoreAmount);
        playerAnim.SetTrigger("Jump");
        BunnyController.instance.PlayParticle(pEffect);
        Debug.Log("Interacted with " + gameObject.name);
        if (_objectType == ObjectType.bed)
        {
            transitionManager.Transition(transition,.5f);
        }
    }
}
