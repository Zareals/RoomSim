using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SerializableTransform
{
    public float[] position;
    public float[] rotation;
    
    public SerializableTransform(Vector3 pos, Quaternion rot)
    {
        position = new float[3];
        position[0] = pos.x;
        position[1] = pos.y;
        position[2] = pos.z;
        
        rotation = new float[4];
        rotation[0] = rot.x;
        rotation[1] = rot.y;
        rotation[2] = rot.z;
        rotation[3] = rot.w;
    }
    
    public Vector3 GetPosition()
    {
        return new Vector3(position[0], position[1], position[2]);
    }
    
    public Quaternion GetRotation()
    {
        return new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);
    }
}

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public SerializableTransform playerTransform;
    public float health;
    public float[] needs;
    public string lastSaveTime;
    
    public PlayerData()
    {
        playerName = "Player";
        playerTransform = new SerializableTransform(Vector3.zero, Quaternion.identity);
        health = 100f;
        needs = new float[4] { 100f, 100f, 100f, 100f };
        lastSaveTime = DateTime.Now.ToString();
    }
}

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    
    [Header("Settings")]
    [SerializeField] private float autoSaveInterval = 30f;
    [SerializeField] private string saveFileName = "playerData.dat";
    
    private float saveTimer;
    private string savePath;
    private PlayerData currentPlayerData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        savePath = Path.Combine(Application.persistentDataPath, saveFileName);
        saveTimer = autoSaveInterval;
        LoadData();
    }

    private void Update()
    {
        saveTimer -= Time.deltaTime;
        if (saveTimer <= 0f)
        {
            SaveData();
            saveTimer = autoSaveInterval;
        }
    }

    public void SaveData()
    {
        UpdatePlayerData();
        
        BinaryFormatter formatter = new BinaryFormatter();
        
        try
        {
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                formatter.Serialize(stream, currentPlayerData);
                Debug.Log($"Game saved at {savePath}");
                
                currentPlayerData.lastSaveTime = DateTime.Now.ToString();
                //OnSaveComplete?.Invoke();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    public void LoadData()
    {
        if (!File.Exists(savePath))
        {
            currentPlayerData = new PlayerData();
            Debug.Log("No save file found. Creating new data.");
            return;
        }
        
        BinaryFormatter formatter = new BinaryFormatter();
        
        try
        {
            using (FileStream stream = new FileStream(savePath, FileMode.Open))
            {
                currentPlayerData = (PlayerData)formatter.Deserialize(stream);
                Debug.Log($"Game loaded from {savePath}");
                
                ApplyPlayerData();
                //OnLoadComplete?.Invoke();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
            currentPlayerData = new PlayerData();
        }
    }

    private void UpdatePlayerData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            currentPlayerData.playerTransform = new SerializableTransform(
                player.transform.position,
                player.transform.rotation
            );
        }
        
        NeedsManager needsManager = FindObjectOfType<NeedsManager>();
        if (needsManager != null)
        {
            for (int i = 0; i < currentPlayerData.needs.Length; i++)
            {
                currentPlayerData.needs[i] = needsManager.GetNeedValue(i);
            }
        }
    }

    private void ApplyPlayerData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = currentPlayerData.playerTransform.GetPosition();
            player.transform.rotation = currentPlayerData.playerTransform.GetRotation();
        }
        
        NeedsManager needsManager = FindObjectOfType<NeedsManager>();
        if (needsManager != null)
        {
            needsManager.SetAllNeedValues(currentPlayerData.needs);
        }
    }

    public void ManualSave()
    {
        SaveData();
        saveTimer = autoSaveInterval;
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted");
            currentPlayerData = new PlayerData();
        }
    }
}