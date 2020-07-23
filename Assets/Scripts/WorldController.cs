using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{

    public delegate void TryDelAndAddPlatform();
    public delegate void TryDelAndAddBackground();
    public event TryDelAndAddPlatform OnPlatformMovement;
    public event TryDelAndAddBackground OnBackMovement;

    public WorldGenerator WorldGenerator;
    public static WorldController instance;
    public float minZ = -20f;
    


    private void Awake()
    {
        if(WorldController.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        WorldController.instance = this;
    }

    private void OnDestroy()
    {
        WorldController.instance = null;
    }

    void Start()
    {
        StartCoroutine(OnPlatformMovementCoroutine());
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    public void WorldMoving(float speed)
    {
        transform.position -= Vector3.forward * speed * Time.deltaTime;
        
    }

    IEnumerator OnPlatformMovementCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            if (OnPlatformMovement != null && OnBackMovement != null)
            {
                OnPlatformMovement();
                OnBackMovement();
            }
        }
    }
}
