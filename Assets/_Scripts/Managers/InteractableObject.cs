using DG.Tweening;
using UnityEngine;

public enum InteractableType{
    cat,
    food,
    bed,
    toy
}

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Need Settings")]
    [SerializeField] private NeedData _affectedNeed;
    [SerializeField] private float _restoreAmount = 20f;
    
    [Header("Animation")]
    [SerializeField] private Animator playerAnim;
    [SerializeField] private string interactTrigger = "Interact";
    
    [Header("Effects")]
    [SerializeField] private InteractableType interactableType;
    [SerializeField] private ParticleSystem interactionEffect;
    [SerializeField] private GameObject highlightEffect;
    [SerializeField] private AudioClip interactionSound;
    [SerializeField] private GameObject sleepingCanvas;
    [SerializeField] private float sleepDuration = 5f;
    [SerializeField] private AudioSource mainAudio;
    
    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 1.5f;
    
    private bool _wasClicked = false;
    private Transform _playerTransform;
    private Sequence _sleepSequence;

    private void Start()
    {
        _playerTransform = BunnyController.instance.transform;
        playerAnim = BunnyController.instance.gameObject.GetComponent<Animator>();
        if (highlightEffect != null)
            highlightEffect.SetActive(false);
            
        // Initialize sleep canvas to be hidden
        if (sleepingCanvas != null)
        {
            sleepingCanvas.transform.localScale = Vector3.zero;
            sleepingCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        if (_playerTransform == null || !_wasClicked) return;
        
        float distance = Vector3.Distance(transform.position, _playerTransform.position);
        if (distance <= interactionDistance)
        {
            PerformInteraction();
            _wasClicked = false;
        }
    }

    public void OnApproachStart()
    {
        if (_wasClicked && highlightEffect != null)
            highlightEffect.SetActive(true);
    }

    public void OnInteract()
    {
        // This will be called when player arrives at destination
    }

    private void OnMouseDown()
    {
        if (Camera.main == null) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                _wasClicked = true;
                OnApproachStart();
            }
        }
    }

    private void PerformInteraction()
    {
        NeedsManager.Instance.RestoreNeed(_affectedNeed, _restoreAmount);
        if (playerAnim != null)
            playerAnim.SetTrigger(interactTrigger);
        
        if (interactionEffect != null)
            Instantiate(interactionEffect, transform.position, Quaternion.identity);
        
        if (interactionSound != null)
            AudioSource.PlayClipAtPoint(interactionSound, transform.position);
        
        if (highlightEffect != null)
            highlightEffect.SetActive(false);
        
        if(interactableType == InteractableType.food)
        {
            Destroy(gameObject, .5f);
        }
        
        if(interactableType == InteractableType.bed)
        {
            ShowSleepCanvas();
        }
    }

    private void ShowSleepCanvas()
    {
        if (sleepingCanvas == null) return;
        
        if (_sleepSequence != null && _sleepSequence.IsActive())
        {
            _sleepSequence.Kill();
        }
        
        _sleepSequence = DOTween.Sequence();
        mainAudio.Pause();
        sleepingCanvas.SetActive(true);
        _sleepSequence.Append(sleepingCanvas.transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack));
            
        _sleepSequence.AppendInterval(sleepDuration);
        

        _sleepSequence.Append(sleepingCanvas.transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBack)
            .OnComplete(() => SleepSequenceComplete()
            ));
    }

    void SleepSequenceComplete()
    {
        mainAudio.Play();
        sleepingCanvas.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }

    private void OnDestroy()
    {
        if (_sleepSequence != null)
        {
            _sleepSequence.Kill();
        }
    }
}