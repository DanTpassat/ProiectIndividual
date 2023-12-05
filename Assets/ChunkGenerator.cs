using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [Header("Control")]
    public int chunkPoolSize;
    public int maxChunksActivePerSide;

    [Header("References")]
    public Transform player;
    public List<GameObject> chunkPrefabsVegetation = new();
    public List<GameObject> chunkPrefabsBuildings = new();
    public GameObject road;
    public GameObject leftChunkSpawnpoint, rightChunkSpawnpoint, roadSpawnpoint;
    public List<GameObject> chunkPool;

    [Header("Debug")]
    [SerializeField] private int leftChunksActive = 0;
    [SerializeField] private int rightChunksActive = 0;

    private void Start()
    {
        InitializeObjectPool();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnLeftChunks();

        SpawnRightChunks();

        SpawnRoad();

        DeleteChunks();
    }


    private void InitializeObjectPool()
    {
        chunkPool = new List<GameObject>();

        GameObject obj = null;

        for (int i = 0; i < chunkPoolSize * 0.75f; i++)
        {
            int rand = Random.Range(0, chunkPrefabsVegetation.Count);

            obj = Instantiate(chunkPrefabsVegetation[rand], Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            chunkPool.Add(obj);
        }


        for(int i = Mathf.FloorToInt(chunkPoolSize * 0.75f); i <= chunkPoolSize; i++)
        {
            int rand = Random.Range(0, chunkPrefabsBuildings.Count);

            obj = Instantiate(chunkPrefabsBuildings[rand], Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            chunkPool.Add(obj);
        }
    }

    private void SpawnLeftChunks()
    {
        if (leftChunksActive < maxChunksActivePerSide)
        {
            int left = Random.Range(0, chunkPool.Count);

            if (!chunkPool[left].activeSelf)
            {
                chunkPool[left].transform.position = leftChunkSpawnpoint.transform.position;
                chunkPool[left].transform.rotation = leftChunkSpawnpoint.transform.rotation;
                chunkPool[left].SetActive(true);
                leftChunksActive++;

                leftChunkSpawnpoint.transform.position = new Vector3(leftChunkSpawnpoint.transform.position.x, 0f, leftChunkSpawnpoint.transform.position.z + 120f);
            }
        }
    }

    private void SpawnRightChunks()
    {
        if (rightChunksActive < maxChunksActivePerSide)
        {
            int right = Random.Range(0, chunkPool.Count);

            if (!chunkPool[right].activeSelf)
            {
                chunkPool[right].transform.position = rightChunkSpawnpoint.transform.position;
                chunkPool[right].transform.rotation = rightChunkSpawnpoint.transform.rotation;
                chunkPool[right].SetActive(true);
                rightChunksActive++;

                rightChunkSpawnpoint.transform.position = new Vector3(rightChunkSpawnpoint.transform.position.x, 0f, rightChunkSpawnpoint.transform.position.z + 120f);
            }
        }
    }

    private void SpawnRoad()
    {
        if (roadSpawnpoint.transform.position.z < player.position.z - 10f)
        {
            road.transform.position = roadSpawnpoint.transform.position;
            roadSpawnpoint.transform.position = new Vector3(0f, 0f, roadSpawnpoint.transform.position.z + 120f);
        }
    }

    private void DeleteChunks()
    {
        foreach (GameObject chunk in chunkPool)
        {
            if (chunk.activeInHierarchy)
            {
                if (player.position.z - 130f > chunk.transform.position.z && chunk.transform.position.x == rightChunkSpawnpoint.transform.position.x)
                {
                    chunk.SetActive(false);
                    rightChunksActive--;
                }

                if (player.position.z - 10f > chunk.transform.position.z && chunk.transform.position.x == leftChunkSpawnpoint.transform.position.x)
                {
                    chunk.SetActive(false);
                    leftChunksActive--;
                }
            }
        }
    }

}