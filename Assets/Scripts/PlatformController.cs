using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    /// <summary>
    /// Конечная позиция текущей платформы
    /// </summary>
    public Transform endPos;

    /// <summary>
    /// Массив с монетками
    /// </summary>
    public GameObject[] coinsSpawners;

    /// <summary>
    /// Массив с препядствиями
    /// </summary>
    public GameObject[] obstacles;

    /// <summary>
    /// Массив второстепенных препядствий
    /// </summary>
    public GameObject[] secondObstacles;

    /// <summary>
    /// Массив с движущимися объектами
    /// </summary>
    public GameObject[] movingObjectsSpawner;
    void Start()
    {
        WorldController.instance.OnPlatformMovement += TryDelAndAddPlatform;
    }

    /// <summary>
    /// Попытка удалить или добавить платформу
    /// </summary>
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
        //Отписываемся от объекта, если он уничтожен
        if (WorldController.instance != null)
        {
            WorldController.instance.OnPlatformMovement -= TryDelAndAddPlatform;
        }
        
    }
}
