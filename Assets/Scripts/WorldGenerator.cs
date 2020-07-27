using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

    /// <summary>
    /// Массив с свободными платформами
    /// </summary>
    public GameObject[] freePlatforms;

    /// <summary>
    /// Массив с препятствиями
    /// </summary>
    public GameObject[] obstaclesPlatforms;

    
    //public GameObject[] startPlatforms;

    /// <summary>
    /// Движущиеся платформы
    /// </summary>
    public GameObject[] movingObjects;

    /// <summary>
    /// Фоны
    /// </summary>
    public GameObject[] backgrounds;

    /// <summary>
    /// Родительский объект платформ
    /// </summary>
    public Transform platformContainer;

    /// <summary>
    /// Родительский объект всех фонов
    /// </summary>
    public Transform backGroundContainer;

    /// <summary>
    /// Родительский объект всех фонов справа
    /// </summary>
    public Transform rightBackGroundContainer;
   
    /// <summary>
    /// Последняя сгенерированная платформа
    /// </summary>
    private Transform lastPlatform = null;

    /// <summary>
    /// Последний сгенерированный фон
    /// </summary>
    private Transform lastBackground = null;

    /// <summary>
    /// Последний сгенерированный фон справа
    /// </summary>
    private Transform lastRightBackground = null;
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Инициализация платформ и фонов
    /// </summary>
    private void Init()
    {
        CreateFreePlatform();
        CreateFreePlatform();
        CreateFreePlatform();

        for(int i = 0; i < 10; i++)
        {
            CreatePlatform();
            
        }
        for(int i = 0; i < 5; i++)
        {
            CreateBackGround();
        }
    }

    /// <summary>
    /// Создание платформы
    /// </summary>
    public void CreatePlatform()
    {
        int rand = Random.Range(0, 10);
        if (rand >= 2)
        {
            CreateObstaclesPlatform();
        }
        else CreateFreePlatform();
        CreateCoins();
        CreateObstacles();
        CreateMovingObjects();

    }

    /// <summary>
    /// Создание свободной платформы
    /// </summary>
    private void CreateFreePlatform()
    {
        Vector3 pos = (lastPlatform == null) ? // получаем позиию для спавна новой платформы
            platformContainer.position :
            lastPlatform.GetComponent<PlatformController>().endPos.position;
        int index = Random.Range(0, freePlatforms.Length);
        GameObject res = Instantiate(freePlatforms[index], pos, Quaternion.identity, platformContainer); //Создаём новую платформу
        lastPlatform = res.transform;
    }

    /// <summary>
    /// Создание платформы с препятствиями
    /// </summary>
    private void CreateObstaclesPlatform()
    {
        Vector3 pos = (lastPlatform == null) ?
            platformContainer.position :
            lastPlatform.GetComponent<PlatformController>().endPos.position;
        int index = Random.Range(0, obstaclesPlatforms.Length);
        GameObject res = Instantiate(obstaclesPlatforms[index], pos, Quaternion.identity, platformContainer);
        lastPlatform = res.transform;

    }

    /// <summary>
    /// Создание монеток
    /// </summary>
    private void CreateCoins()
    {
        for (int i = 0; i < lastPlatform.GetComponent<PlatformController>().coinsSpawners.Length; i++) //Проходимся по массиву монеток на
            //платформе и включаем их
        {
            int index = Random.Range(0, lastPlatform.GetComponent<PlatformController>().coinsSpawners.Length);
            int rand = Random.Range(0, 11);

            if (lastPlatform.GetComponent<PlatformController>().coinsSpawners.Length != 0)
            {
                if (rand < 6)
                {
                    lastPlatform.GetComponent<PlatformController>().coinsSpawners[index].SetActive(true);
                }
                else lastPlatform.GetComponent<PlatformController>().coinsSpawners[index].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Создаёт движущиеся объекты
    /// </summary>
    private void CreateMovingObjects()
    {
        int index = Random.Range(0, lastPlatform.GetComponent<PlatformController>().movingObjectsSpawner.Length);
        int rand = Random.Range(0, 100);

        if(lastPlatform.GetComponent<PlatformController>().movingObjectsSpawner.Length > 0)
        {
            for(int i = 0; i < lastPlatform.GetComponent<PlatformController>().movingObjectsSpawner.Length; i++)
            if(rand <= 90)
            {
                    lastPlatform.GetComponent<PlatformController>().movingObjectsSpawner[index].SetActive(true);
            }
        }
    }

    /// <summary>
    /// Устанавливает рандомное значение исходя из количества препятствий
    /// </summary>
    /// <param name="obstaclesCount">Количество препядствий на платформе</param>
    /// <returns></returns>
    private int SetRandRange(int obstaclesCount)
    {
        int rand = 0;
        if (obstaclesCount > 3)
        {
            rand = Random.Range(2, 5);
        }
        else if (obstaclesCount < 3)
        {
            rand = 1;
        }
        else if (obstaclesCount == 3)
             rand = Random.Range(1, 3);
        return rand;

    }

    /// <summary>
    /// Включает препядствия на платформе
    /// </summary>
    private void CreateObstacles()
    {
        int obstaclesCount = lastPlatform.GetComponent<PlatformController>().obstacles.Length;
        int secondObstaclesCount = lastPlatform.GetComponent<PlatformController>().secondObstacles.Length;

        for (int i = 0; i < SetRandRange(obstaclesCount); i++)
        {
            int index = Random.Range(0, obstaclesCount);
            if (lastPlatform.GetComponent<PlatformController>().obstacles.Length != 0)
            {
                 lastPlatform.GetComponent<PlatformController>().obstacles[index].SetActive(true);
            }
        }
        for (int i = 0; i < SetRandRange(secondObstaclesCount); i++)
        {
            int index = Random.Range(0, secondObstaclesCount);
            if (lastPlatform.GetComponent<PlatformController>().secondObstacles.Length != 0)
            {
                lastPlatform.GetComponent<PlatformController>().secondObstacles[index].SetActive(true);
            }
        }
    }


    /// <summary>
    /// Создаёт рандомный фон с каждой стороны
    /// </summary>
    public void CreateBackGround()
    {
        if (backgrounds.Length > 0)
        {
            Vector3 pos = (lastBackground == null) ? backGroundContainer.position :
                lastBackground.GetComponent<BackGroundController>().endPos.position;
            Vector3 posRight = (lastRightBackground == null) ? rightBackGroundContainer.position :
                lastRightBackground.GetComponent<BackGroundController>().endPos.position;
            int rand = Random.Range(0, backgrounds.Length);
            int randRight = Random.Range(0, backgrounds.Length);
            GameObject back = Instantiate(backgrounds[rand], pos, Quaternion.identity, backGroundContainer);
            GameObject backRight = Instantiate(backgrounds[randRight], posRight, Quaternion.identity, rightBackGroundContainer);
            lastBackground = back.transform;
            lastRightBackground = backRight.transform;

        }
       
    }
}
