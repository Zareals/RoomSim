public interface INeedStrategy
{
    void UpdateNeed(NeedData needData, float deltaTime);
    void RestoreNeed(NeedData needData, float amount);
    float GetDecayRate();
}