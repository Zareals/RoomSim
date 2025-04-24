using UnityEngine;

namespace Cynithia.Managers
{
    public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private NeedsManager _needsManager;
    
    private void Awake()
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
    
    public NeedsManager NeedsManager => _needsManager;
}

}
