using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    /// <summary>
    /// Конечная точка фона
    /// </summary>
    public Transform endPos;
    void Start()
    {
        WorldController.instance.OnBackMovement += TryDelAndAddBackground;
    }

    /// <summary>
    /// Попытка удалить или добавить фон
    /// </summary>
    public void TryDelAndAddBackground()
    {
        if(transform.position.z < WorldController.instance.minZ * 2)
        {
            WorldController.instance.WorldGenerator.CreateBackGround();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (WorldController.instance != null)
        {
            WorldController.instance.OnBackMovement -= TryDelAndAddBackground;
        }
    }
}
