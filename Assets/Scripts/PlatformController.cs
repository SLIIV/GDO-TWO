using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Transform endPos;
    public GameObject[] coinsSpawners;
    public GameObject[] obstacles;
    public GameObject[] secondObstacles;
    public GameObject[] movingObjectsSpawner;
    void Start()
    {
        WorldController.instance.OnPlatformMovement += TryDelAndAddPlatform;
    }


    private void TryDelAndAddPlatform()
    {
        if(transform.position.z < WorldController.instance.minZ)
        {
            WorldController.instance.WorldGenerator.CreatePlatform();
            Destroy(gameObject);
        }
        
    }
    

    private void OnDestroy()
    {

        if (WorldController.instance != null)
        {
            WorldController.instance.OnPlatformMovement -= TryDelAndAddPlatform;
        }
        
    }
}
