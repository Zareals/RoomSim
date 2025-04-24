using UnityEngine;

[CreateAssetMenu(fileName = "LinearDecayStrategy", menuName = "Needs System/Strategies/Linear Decay")]
public class LinearDecayStrategy : NeedStrategySO
{
    [SerializeField] private float decayRate = 0.5f;
    
    public override void UpdateNeed(NeedData needData, float deltaTime)
    {
        needData.CurrentValue -= decayRate * deltaTime;
    }
    
    public override void RestoreNeed(NeedData needData, float amount)
    {
        needData.CurrentValue += amount;
    }
    
    public override float GetDecayRate() => decayRate;
}