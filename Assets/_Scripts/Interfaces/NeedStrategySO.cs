using UnityEngine;

public abstract class NeedStrategySO : ScriptableObject
{
    public abstract void UpdateNeed(NeedData needData, float deltaTime);
    public abstract void RestoreNeed(NeedData needData, float amount);
    public abstract float GetDecayRate();
}