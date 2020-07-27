using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{

    /// <summary>
    /// Попытка удалить или добавить платформу
    /// </summary>
    public delegate void TryDelAndAddPlatform();

    /// <summary>
    /// Попытка удалить или добавить
    /// </summary>
    public delegate void TryDelAndAddBackground();

    /// <summary>
    /// Событие движения платформы
    /// </summary>
    public event TryDelAndAddPlatform OnPlatformMovement;

    /// <summary>
    /// Событие движения фона
    /// </summary>
    public event TryDelAndAddBackground OnBackMovement;


    /// <summary>
    /// Экземпляр генератора
    /// </summary>
    public WorldGenerator WorldGenerator;

    /// <summary>
    /// Инстанс контроллера
    /// </summary>
    public static WorldController instance;

    /// <summary>
    /// Минимальное расстояние, на которое может отдалиться платформа
    /// </summary>
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


    /// <summary>
    /// Двигает мир
    /// </summary>
    /// <param name="speed"></param>
    public void WorldMoving(float speed)
    {
        transform.position -= Vector3.forward * speed * Time.deltaTime;
        
    }


    /// <summary>
    /// Обрабатываем движение платформы
    /// </summary>
    /// <returns></returns>
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
