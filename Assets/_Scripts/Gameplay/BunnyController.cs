using UnityEngine;

public class BunnyController : MonoBehaviour
{
    public static BunnyController instance;
    [SerializeField] private GameObject bunnyMesh;

    private void Awake()
    {
        instance = this;
    }
    
    public void PlayParticle(ParticleSystem particle)
    {
        Instantiate(particle, bunnyMesh.transform.position, Quaternion.identity, bunnyMesh.transform);
    }

}
