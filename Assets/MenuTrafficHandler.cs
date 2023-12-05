using System.Collections.Generic;
using UnityEngine;

public class MenuTrafficHandler : MonoBehaviour
{
    public List<GameObject> carModels = new();
    public Transform leftLaneSP, rightLaneSP;

    public List<GameObject> carPool;

    [Header("Variables")]
    public int maxActiveCars;
    public int trafficSpeed;

    [Header("Stats")]
    [SerializeField] private int activeCars = 0;

    private float timeBetweenSpawns = 0f;

    private void Start()
    {
        InitializeObjectPool();
        timeBetweenSpawns = Random.Range(1f, 5f);
    }

    private void Update()
    {
        timeBetweenSpawns -= Time.deltaTime;
        
        SpawnCars();
        MoveAndDeleteCars();
    }


    private void InitializeObjectPool()
    {
        carPool = new List<GameObject>();

        GameObject obj = null;

        for (int i = 0; i <= maxActiveCars * 2; i++)
        {
            int rand = Random.Range(0, carModels.Count);

            obj = Instantiate(carModels[rand], Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            carPool.Add(obj);
        }
    }

    private void SpawnCars()
    {
        if(activeCars < maxActiveCars && timeBetweenSpawns < 0f)
        {
            int rand = Random.Range(0, carPool.Count);

            if (!carPool[rand].activeSelf)
            {
                int lane = Random.Range(0, 2); // 0 - left    1 - right

                switch (lane)
                {
                    case 0:
                        carPool[rand].transform.SetPositionAndRotation(leftLaneSP.position, leftLaneSP.rotation);
                        carPool[rand].SetActive(true);
                        activeCars++;
                        timeBetweenSpawns = Random.Range(0.5f, 2f);
                        break;
                    case 1:
                        carPool[rand].transform.SetPositionAndRotation(rightLaneSP.position, rightLaneSP.rotation);
                        carPool[rand].SetActive(true);
                        activeCars++;
                        timeBetweenSpawns = Random.Range(0.5f, 2f);
                        break;
                }
            }
        }
    }
    
    private void MoveAndDeleteCars()
    {
        foreach (GameObject car in carPool)
        {
            if (car.activeSelf)
            {
                car.transform.Translate(Vector3.forward * trafficSpeed * Time.deltaTime);
            }

            if(car.activeSelf && car.transform.position.z < leftLaneSP.position.z || car.transform.position.z > rightLaneSP.position.z)
            {
                car.SetActive(false);
                activeCars--;
            }
        }
    }
}
