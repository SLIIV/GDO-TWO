using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject[] freePlatforms;
    public GameObject[] obstaclesPlatforms;
    public GameObject[] startPlatforms;
    public GameObject[] movingObjects;
    public GameObject[] backgrounds;
    public Transform platformContainer;
    public Transform backGroundContainer;
    public Transform rightBackGroundContainer;
   

    private Transform lastPlatform = null;
    private Transform lastBackground = null;
    private Transform lastRightBackground = null;
    void Start()
    {
        
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    private void CreateFreePlatform()
    {
        Vector3 pos = (lastPlatform == null) ?
            platformContainer.position :
            lastPlatform.GetComponent<PlatformController>().endPos.position;
        int index = Random.Range(0, freePlatforms.Length);
        GameObject res = Instantiate(freePlatforms[index], pos, Quaternion.identity, platformContainer);
        lastPlatform = res.transform;
    }

    private void CreateObstaclesPlatform()
    {
        Vector3 pos = (lastPlatform == null) ?
            platformContainer.position :
            lastPlatform.GetComponent<PlatformController>().endPos.position;
        int index = Random.Range(0, obstaclesPlatforms.Length);
        GameObject res = Instantiate(obstaclesPlatforms[index], pos, Quaternion.identity, platformContainer);
        lastPlatform = res.transform;

    }
    private void CreateCoins()
    {
        for (int i = 0; i < lastPlatform.GetComponent<PlatformController>().coinsSpawners.Length; i++)
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
