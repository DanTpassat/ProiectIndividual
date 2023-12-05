using System.Collections.Generic;
using UnityEngine;

public class CollectiblesSpawner : MonoBehaviour
{   
    [Header("Object Pool Settings")]
    public int maxCollectiblesActive;
    public int initialPoolSize;
    public int distanceBetweenCollectibles;

    public List<GameObject> collectibles = new();

    public List<GameObject> objectPool;
    public int collectiblesActive = 0;

    private CarController player;

    // Called before the first frame update
    private void Start()
    {
        InitializeObjectPool();

        player = FindObjectOfType<CarController>();
    }

    private void Update()
    {
        SpawnObjects();
        DespawnObjects();
    }

    private void InitializeObjectPool()
    {
        objectPool = new List<GameObject>();
        
        GameObject obj = null;

        for (int i = 0; i < initialPoolSize; i++)
        {   
            int rand = Random.Range(0, collectibles.Count);
            
            obj = Instantiate(collectibles[rand], Vector3.zero, Quaternion.identity);

            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    private void SpawnObjects()
    {
        if (collectiblesActive < maxCollectiblesActive)
        {
            float randomX = Random.Range(-17.5f, -2.5f);
            float randomZ = Random.Range(100f, 300f);

            Vector3 objSpawnPosition = new Vector3(randomX, 1f, player.transform.position.z + randomZ);
            bool canSpawn = true;

            int i = Random.Range(0, objectPool.Count);


            if (!objectPool[i].activeSelf)
            {
                foreach (GameObject obj in objectPool)
                {
                    if (obj.activeSelf)
                    {
                        if (Vector3.Distance(objSpawnPosition, obj.transform.position) <= distanceBetweenCollectibles)
                        {
                            canSpawn = false;
                            break;
                        }
                    }
                }

                if (canSpawn)
                {
                    GameObject obj = objectPool[i];

                    obj.transform.position = objSpawnPosition;
                    obj.SetActive(true);
                    collectiblesActive++;
                }
            }
        }
    }

    private void DespawnObjects()
    {
        foreach (GameObject obj in objectPool)
        {
            if (obj.activeSelf && obj.transform.position.z + 25f < player.transform.position.z)
            {
                obj.SetActive(false);
                collectiblesActive--;
            }
        }
    }
}
