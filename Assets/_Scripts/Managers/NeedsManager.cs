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
            Debug.Log($"Updating {need.name}: {need.CurrentValue}");
        }
    }
    
    public void RestoreNeed(NeedData need, float amount)
    {
        need.Restore(amount);
        
        // Play feedback
        if (need.restoreSound != null)
        {
            AudioSource.PlayClipAtPoint(need.restoreSound, Camera.main.transform.position);
        }
        
        if (need.restoreEffect != null)
        {
            Instantiate(need.restoreEffect, transform.position, Quaternion.identity);
        }
    }
}