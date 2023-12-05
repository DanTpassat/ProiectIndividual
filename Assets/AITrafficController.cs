using System.Collections.Generic;
using UnityEngine;

public class AITrafficController : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> carModels = new();
    public List<GameObject> carPool;

    [Header("Variables")]
    public int maxActiveCars;

    [Header("Stats")]
    [SerializeField] private int activeCars = 0;

    private Vector3 carSpawnPoint, carSpawnRotation;


    private void Start()
    {
        InitializeObjectPool();
    }

    private void Update()
    {
        //spawn Cars
        if (activeCars < maxActiveCars)
        {
            SpawnCar();
        }

        DeleteCar();
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


    private void SpawnCar()
    {
        int i = Random.Range(0, carPool.Count);

        if (!carPool[i].activeInHierarchy)
        {
            int lane = Random.Range(0, 4); //  ( 0 - first lane    1 - second lane ) - right lane    ( 2 - third lane  3 - fourth lane ) - left lane

            switch (lane)
            {
                case 0: //right lane
                    carSpawnRotation = Vector3.zero;
                    carSpawnPoint = new Vector3(-2.5f, 0f, player.transform.position.z + Random.Range(100f, 300f));
                    break;
                case 1: //right lane
                    carSpawnRotation = Vector3.zero;
                    carSpawnPoint = new Vector3(-7.5f, 0f, player.transform.position.z + Random.Range(100f, 300f));
                    break;
                case 2: //left lane
                    carSpawnRotation = new Vector3(0f, 180f, 0f);
                    carSpawnPoint = new Vector3(-12.5f, 0f, player.transform.position.z + Random.Range(100f, 300f));
                    break;
                case 3: //left lane
                    carSpawnRotation = new Vector3(0f, 180f, 0f);
                    carSpawnPoint = new Vector3(-17.5f, 0f, player.transform.position.z + Random.Range(100f, 300f));
                    break;
            }

            carPool[i].transform.position = carSpawnPoint;
            carPool[i].transform.eulerAngles = carSpawnRotation;

            // Check for collision with other cars
            bool canSpawnCar = false;
            if (!Physics.Raycast(carPool[i].transform.position + new Vector3(0f, 1f, 0f), carPool[i].transform.forward * -1, out RaycastHit hit, 25f))
            {
                canSpawnCar = true;
            }

            foreach (GameObject car in carPool)
            {
                if (car.activeInHierarchy)
                {
                    float distance = Vector3.Distance(car.transform.position, carSpawnPoint);
                    if (distance < 10f) // Adjust the value as per your car size
                    {
                        canSpawnCar = false;
                        break;
                    }
                }
            }

            if (canSpawnCar)
            {
                carPool[i].SetActive(true);
                carPool[i].GetComponent<CarAI>().GetCurrentLane();
                activeCars++;
            }
        }
    }


    private void DeleteCar()
    {
        foreach (GameObject car in carPool)
        {
            if (car.activeInHierarchy == true && car.transform.position.z + 50f < player.transform.position.z)
            {
                car.SetActive(false);
                activeCars--;
            }
        }
    }
}
