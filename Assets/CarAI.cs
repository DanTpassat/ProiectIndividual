using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public float acceleration;

    [Header("References")]
    public GameObject carModel;
    public List<GameObject> wheels = new List<GameObject>();

    private float currentSpeed;
    [SerializeField] private float targetSpeed;
    private Rigidbody carRb;

    private float interval;
    public int currentLane = 0;
    private AITrafficController trafficController;

    private void Start()
    {
        carRb = carModel.GetComponent<Rigidbody>();
        trafficController = FindObjectOfType<AITrafficController>(); 
        targetSpeed = speed;

        interval = Random.Range(5f, 20f);
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (carModel.activeSelf == true)
        {
            // Update current speed
            currentSpeed = carRb.velocity.magnitude;

            // Apply target speed based on local transform
            Vector3 localVelocity = carModel.transform.InverseTransformDirection(carRb.velocity);
            localVelocity.z = targetSpeed;
            //move car forward
            carRb.velocity = carModel.transform.TransformDirection(localVelocity);


            // Detect if a car is in front
            if (Physics.Raycast(carModel.transform.position + new Vector3(0f, 1f, 0f), carModel.transform.forward, out RaycastHit hit, 15f))
            {
                Debug.DrawLine(carModel.transform.position + new Vector3(0f, 1f, 0f), hit.point, Color.yellow);
                if (hit.collider.CompareTag("CarAI"))
                {
                    targetSpeed = hit.collider.GetComponent<CarAI>().targetSpeed;
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    targetSpeed = 0f;
                }
            }
            else
            {
                Debug.DrawLine(carModel.transform.position + new Vector3(0f, 1f, 0f), carModel.transform.position + new Vector3(0f, 1f, 0f) + carModel.transform.forward * 15f, Color.green);
                targetSpeed = speed;
            }

            // Wheels rotation
            for (int i = 0; i < wheels.Count; i++)
            {
                wheels[i].transform.Rotate(currentSpeed * Time.deltaTime * 50, 0, 0);
            }


            //change lane handling
            if (isActiveAndEnabled)
            {
                interval -= Time.deltaTime;
            }

            if (interval < 0f)
            {
                ChangeLane();
                interval = Random.Range(5f, 20f);
            }
        }
    }

    public void GetCurrentLane()
    {
        switch (transform.position.x)
        {
            case -2.5f:
                currentLane = 1;
                break;
            case -7.5f:
                currentLane = 2;
                break;
            case -12.5f:
                currentLane = 3;
                break;
            case -17.5f:
                currentLane = 4;
                break;
        }
    }

    private void ChangeLane()
    {
        bool possible = true;
        
        switch (transform.position.x)
        {
            case -2.5f:
                foreach (GameObject car in trafficController.carPool)
                {
                    if(car.GetComponent<CarAI>().currentLane == 2 && Vector3.Distance(transform.position, car.transform.position) < 15f)
                    {
                        possible = false;
                        break;
                    }
                }
                if(possible)
                {
                    currentLane = 2;
                    carRb.DOMoveX(-7.5f, 2f);
                }
                break;

            case -7.5f:
                foreach (GameObject car in trafficController.carPool)
                {
                    if (car.GetComponent<CarAI>().currentLane == 1 && Vector3.Distance(transform.position, car.transform.position) < 15f)
                    {
                        possible = false;
                        break;
                    }
                }
                if (possible)
                {
                    currentLane = 1;
                    carRb.DOMoveX(-2.5f, 2f);
                }
                break;

            case -12.5f:
                foreach (GameObject car in trafficController.carPool)
                {
                    if (car.GetComponent<CarAI>().currentLane == 4 && Vector3.Distance(transform.position, car.transform.position) < 15f)
                    {
                        possible = false;
                        break;
                    }
                }
                if (possible)
                {
                    carRb.DOMoveX(-17.5f, 2f);
                    currentLane = 4;
                }
                break;

            case -17.5f:
                foreach (GameObject car in trafficController.carPool)
                {
                    if (car.GetComponent<CarAI>().currentLane == 3 && Vector3.Distance(transform.position, car.transform.position) < 15f)
                    {
                        possible = false;
                        break;
                    }
                }
                if (possible)
                {
                    carRb.DOMoveX(-12.5f, 2f);
                    currentLane = 3;
                }
                break;

                /*
                case -2.5f:
                    if (!Physics.Raycast(carModel.transform.position + new Vector3(0f, 1f, 0f), Vector3.left, 5f))
                    {
                        carRb.DOMoveX(-7.5f, 2f);
                    }
                    break;
                case -7.5f:
                    if (!Physics.Raycast(carModel.transform.position + new Vector3(0f, 1f, 0f), Vector3.right, 5f))
                    {
                        carRb.DOMoveX(-2.5f, 2f);
                    }
                    break;
                case -12.5f:
                    if (!Physics.Raycast(carModel.transform.position + new Vector3(0f, 1f, 0f), Vector3.left, 5f))
                    {
                        carRb.DOMoveX(-17.5f, 2f);
                    }
                    break;
                case -17.5f:
                    if (!Physics.Raycast(carModel.transform.position + new Vector3(0f, 1f, 0f), Vector3.right, 5f))
                    {
                        carRb.DOMoveX(-12.5f, 2f);
                    }
                    break;

                */
        }
    }
}