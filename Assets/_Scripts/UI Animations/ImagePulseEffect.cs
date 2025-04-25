using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class ImagePulseEffect : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private float pulseScale = 1.2f;
    [SerializeField] private float pulseDuration = 0.5f;
    [SerializeField] private int pulseLoops = -1; // -1 for infinite
    [SerializeField] private Ease easeType = Ease.InOutSine;
    
    private Image targetImage;
    private Vector3 originalScale;
    private Sequence pulseSequence;

    private void Awake()
    {
        targetImage = GetComponent<Image>();
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        StartPulse();
    }

    private void OnDisable()
    {
        StopPulse();
    }

    public void StartPulse()
{
    pulseSequence?.Kill();
    
    pulseSequence = DOTween.Sequence();
    pulseSequence.Append(transform.DOScale(originalScale * pulseScale, pulseDuration / 2).SetEase(easeType));
    pulseSequence.Append(transform.DOScale(originalScale, pulseDuration / 2).SetEase(easeType));
    pulseSequence.SetLoops(pulseLoops);
}

    public void StopPulse()
    {
        pulseSequence?.Kill();
        transform.localScale = originalScale;
    }
}