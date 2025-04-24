using UnityEngine;

[CreateAssetMenu(fileName = "AcceleratedDecayStrategy", menuName = "Needs System/Strategies/Accelerated Decay")]
public class AcceleratedDecayStrategy : NeedStrategySO
{
    [SerializeField] private float baseDecayRate = 0.3f;
    [SerializeField] private float accelerationFactor = 0.1f;
    
    public override void UpdateNeed(NeedData needData, float deltaTime)
    {
        float currentRate = baseDecayRate + (accelerationFactor * (1 - (needData.CurrentValue / needData.maxValue)));
        needData.CurrentValue -= currentRate * deltaTime;
    }
    
    public override void RestoreNeed(NeedData needData, float amount)
    {
        needData.CurrentValue += amount;
    }
    
    public override float GetDecayRate() => baseDecayRate;
}