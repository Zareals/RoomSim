using UnityEngine;

[CreateAssetMenu(fileName = "NewNeedData", menuName = "Needs System/Need Data")]
public class NeedData : ScriptableObject
{
    public string needName;
    public float maxValue = 100f;
    [SerializeField, Range(0, 100)] private float _currentValue = 100f;
    public NeedStrategySO strategy;
    
    public float CurrentValue
    {
        get => _currentValue;
        set => _currentValue = Mathf.Clamp(value, 0, maxValue);
    }

    [Header("UI")]
    public Color barColor = Color.green;
    public Sprite icon;
    
    [Header("Feedback")]
    public AudioClip restoreSound;
    public ParticleSystem restoreEffect;

    public void UpdateNeed(float deltaTime)
    {
        if (strategy != null)
        {
            strategy.UpdateNeed(this, deltaTime);
        }
    }

    public void Restore(float amount)
    {
        if (strategy != null)
        {
            strategy.RestoreNeed(this, amount);
        }
    }
}