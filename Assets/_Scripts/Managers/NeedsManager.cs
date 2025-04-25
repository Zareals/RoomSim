using System.Collections.Generic;
using UnityEngine;

public class NeedsManager : MonoBehaviour
{
    public static NeedsManager Instance { get; private set; }
    [SerializeField] private List<NeedData> needs = new List<NeedData>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        foreach (var need in needs)
        {
            need.UpdateNeed(Time.deltaTime);
        }
    }
    
    public void RestoreNeed(NeedData need, float amount)
    {
        need.Restore(amount);
        
        if (need.restoreSound != null)
        {
            AudioSource.PlayClipAtPoint(need.restoreSound, Camera.main.transform.position);
        }
        
        if (need.restoreEffect != null)
        {
            Instantiate(need.restoreEffect, transform.position, Quaternion.identity);
        }
    }

    public float GetNeedValue(int needIndex)
    {
        if (needIndex >= 0 && needIndex < needs.Count)
        {
            return needs[needIndex].CurrentValue;
        }
        Debug.LogError($"Invalid need index: {needIndex}");
        return 0f;
    }

    public float GetNeedValue(NeedData need)
    {
        if (needs.Contains(need))
        {
            return need.CurrentValue;
        }
        Debug.LogError("Need not found in NeedsManager");
        return 0f;
    }

    public void RestoreNeed(int needIndex, float amount)
    {
        if (needIndex >= 0 && needIndex < needs.Count)
        {
            RestoreNeed(needs[needIndex], amount);
            return;
        }
        Debug.LogError($"Invalid need index: {needIndex}");
    }

    public int GetNeedCount()
    {
        return needs.Count;
    }

    public float[] GetAllNeedValues()
    {
        float[] values = new float[needs.Count];
        for (int i = 0; i < needs.Count; i++)
        {
            values[i] = needs[i].CurrentValue;
        }
        return values;
    }

    public void SetAllNeedValues(float[] values)
    {
        if (values == null || values.Length != needs.Count)
        {
            Debug.LogError("Invalid values array length");
            return;
        }

        for (int i = 0; i < needs.Count; i++)
        {
            needs[i].CurrentValue = Mathf.Clamp(values[i], 0, needs[i].maxValue);
        }
    }
}