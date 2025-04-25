using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class ImageBounceEffect : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float moveDistance = 50f;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private int bounceLoops = -1; // -1 for infinite
    [SerializeField] private Ease easeType = Ease.InOutSine;
    
    private Image targetImage;
    private Vector3 originalPosition;
    private Sequence bounceSequence;

    private void Awake()
    {
        targetImage = GetComponent<Image>();
        originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        StartBounce();
    }

    private void OnDisable()
    {
        StopBounce();
    }

    public void StartBounce()
{
    bounceSequence?.Kill();
    
    bounceSequence = DOTween.Sequence();
    bounceSequence.Append(transform.DOLocalMoveY(originalPosition.y + moveDistance, moveDuration / 2).SetEase(easeType));
    bounceSequence.Append(transform.DOLocalMoveY(originalPosition.y, moveDuration / 2).SetEase(easeType));
    bounceSequence.SetLoops(bounceLoops);
}

    public void StopBounce()
    {
        bounceSequence?.Kill();
        transform.localPosition = originalPosition;
    }
}