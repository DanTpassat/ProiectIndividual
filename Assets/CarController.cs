using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CarController : MonoBehaviour
{
    [Header("Car Speeds")]
    public float baseSpeed;
    public float nitroSpeed, breakSpeed;

    [Header("Car Stats")]
    public float acceleration;
    public float handling, nitroTime, nitroCooldown;

    [Header("Sounds")]
    public AudioSource engineSound;
    public AudioSource skidSound;

    [Header("References")]
    public GameObject carModel;
    public GameObject playerHolder;
    public TextMeshProUGUI scoreText;
    public Slider nosSlider;
    public GameObject inGameUI;
    public GameObject[] wheels;

    [Header("Debug")]
    public float carCurrentSpeed;
    [SerializeField] private float carTargetSpeed;


    private Rigidbody carRb;
    private Camera playerCam;

    //break handling
    [HideInInspector] public bool breaking = false;

    //nitro handling
    [HideInInspector] public bool startedNitro = false;
    [HideInInspector] public float elapsedNitroTime;
    [HideInInspector] public float nitroCooldownCounter;
    private float baseAcceleration, nitroAcceleration;

    //camera shake handling
    private CameraShake camShake;

    //turning logic
    private bool turning = false;
    [HideInInspector] public float turnPower;
    [HideInInspector] public float turnTime;

    //crashing logic
    private bool crashInitiated = false;
    [HideInInspector] public bool crashed = false;

    private void Start()
    {
        //get the rigidbody of the player
        carRb = GetComponent<Rigidbody>();
        camShake = FindObjectOfType<CameraShake>();
        playerCam = FindObjectOfType<Camera>();
        
        //nitro handling variables
        elapsedNitroTime = nitroTime;
        nitroCooldownCounter = nitroCooldown;
        baseAcceleration = acceleration;
        nitroAcceleration = acceleration * 2f;

        //initialise base speed when starting car
        carTargetSpeed = baseSpeed;

        inGameUI.transform.localScale = new Vector3(2f, 2f, 2f);
        inGameUI.transform.DOScale(1f, 2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        //update player holder
        playerHolder.transform.position = new Vector3(-10f, -0.025f, transform.position.z + 230f);
        
        //score text update
        scoreText.text = Mathf.FloorToInt(transform.position.z).ToString();

        //update Car Current Speed
        carCurrentSpeed = carRb.velocity.magnitude;
        
        //update camera shake magnitude
        camShake.magnitude = carCurrentSpeed * 0.0003f;

        //car movement forward
        if (carCurrentSpeed < carTargetSpeed)
        {
            carRb.AddForce(Vector3.forward * acceleration * Time.deltaTime, ForceMode.Acceleration);
        }

        //car model visual rotation
        TurnHandling();

        //nitro handling
        NitroHandling();

        //break handling
        BreakHandling();

        //crash handling
        CrashHandling();

        //audio handling
        AudioHandler();
    }

    private void TurnHandling()
    {
        if (!turning)
        {
            carRb.MoveRotation(Quaternion.RotateTowards(carRb.rotation, Quaternion.identity, 5f * Time.deltaTime));

            carModel.transform.rotation = Quaternion.Lerp(carModel.transform.rotation, Quaternion.identity, Time.deltaTime * (carCurrentSpeed / 10));

            wheels[0].transform.rotation = Quaternion.Lerp(wheels[0].transform.rotation, Quaternion.identity, Time.deltaTime * (carCurrentSpeed / 10));
            wheels[1].transform.rotation = Quaternion.Lerp(wheels[1].transform.rotation, Quaternion.identity, Time.deltaTime * (carCurrentSpeed / 10));

            //camera rotation
            Quaternion camTargetRotation = Quaternion.Euler(10f, 0f, 0f);
            playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, camTargetRotation, Time.deltaTime * (carCurrentSpeed / 10));

            turnPower = 0f;

            carRb.constraints = RigidbodyConstraints.FreezePositionX;

            turnTime = 0f;
        }
        else
        {
            turnPower = Mathf.Lerp(turnPower, handling, Time.deltaTime * (carCurrentSpeed / 10f));

            carRb.constraints = RigidbodyConstraints.None;

            turnTime += Time.deltaTime;
        }


        //wheels rotation
        for (int i = 0; i < 4; i++)
        {
            wheels[i].transform.Rotate(carCurrentSpeed * Time.deltaTime * 50, 0, 0);
        }
    }

    private void NitroHandling()
    {
        if (startedNitro)
        {
            if (elapsedNitroTime > 0f)
            {
                carTargetSpeed = nitroSpeed;
                acceleration = nitroAcceleration;
                elapsedNitroTime = elapsedNitroTime - Time.deltaTime;
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, 110f, 4f * Time.deltaTime);
                nosSlider.value = Mathf.Lerp(0f, 1f, elapsedNitroTime / nitroTime);
            }
            else
            {
                carTargetSpeed = baseSpeed;
                acceleration = baseAcceleration;
                nitroCooldownCounter = nitroCooldownCounter - Time.deltaTime;
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, 80f, 4f * Time.deltaTime);
                nosSlider.value = Mathf.Lerp(1f, 0f, nitroCooldownCounter / nitroCooldown);

                if (nitroCooldownCounter <= 0f)
                {
                    startedNitro = false;
                    elapsedNitroTime = nitroTime;
                    nitroCooldownCounter = nitroCooldown;
                }
            }
        }
    }

    private void BreakHandling()
    {
        if (breaking)
        {
            if (startedNitro)
            {
                elapsedNitroTime = 0f;
            }
            
            if(carCurrentSpeed > breakSpeed)
            {
                carRb.AddForce(Vector3.back * acceleration * Time.deltaTime, ForceMode.Acceleration);
            }

            carTargetSpeed = breakSpeed;
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, 70f, 4f * Time.deltaTime);
        }
        else if(!breaking && !startedNitro)
        {
            carTargetSpeed = baseSpeed;
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, 80f, 4f * Time.deltaTime);
        }
    }

    private void CrashHandling()
    {
        if(transform.position.x > -1.25f || transform.position.x < -18.75f)
        {
            crashed = true;
        }
        
        if(crashed && !crashInitiated)
        {
            playerCam.DOShakePosition(0.5f, 1f);

            carRb.constraints = RigidbodyConstraints.None;

            Transform cameraTransform = playerCam.transform;
            cameraTransform.SetParent(null);

            inGameUI.transform.DOMove(new Vector3(0f, -1100f, 0f), 1.5f).SetEase(Ease.OutCirc);

            ChunkGenerator chunkGen = FindObjectOfType<ChunkGenerator>();
            chunkGen.enabled = false;

            AITrafficController controller = FindObjectOfType<AITrafficController>();
            controller.enabled = false;

            foreach (GameObject car in controller.carPool)
            {
                car.GetComponent<CarAI>().enabled = false;
                car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }

            engineSound.enabled = false;
            skidSound.enabled = false;

            CarController script = GetComponent<CarController>();
            script.enabled = false;

            crashInitiated = true;
        }
    }

    public void AudioHandler()
    {
        //engine sound
        engineSound.pitch = Mathf.Lerp(0.3f, 3f, Mathf.Abs(carCurrentSpeed) / nitroSpeed);
        
        //skindmarks sound
        if ((turnPower > handling / 2 && carCurrentSpeed > baseSpeed / 2 && turnTime > 0.5f) || (breaking && carCurrentSpeed > baseSpeed / 2))
        {
            skidSound.mute = false;
        }
        else
        {
            skidSound.mute = true;
        }
    }

    //touch and swipe control functions
    public void ArrowLeft()
    {
        //turn power
        //carRb.AddForce(handling * Vector3.left * Time.deltaTime * (carCurrentSpeed / 10), ForceMode.VelocityChange);
        carRb.velocity = new Vector3(-turnPower, carRb.velocity.y, carRb.velocity.z);

        //car model visual rotation
        Quaternion targetRotation = Quaternion.Euler(0f, -10f, -6f);
        carModel.transform.rotation = Quaternion.Lerp(carModel.transform.rotation, targetRotation, Time.deltaTime * (carCurrentSpeed / 10));

        //camera rotation
        Quaternion camTargetRotation = Quaternion.Euler(10f, -1f, 5f);
        playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, camTargetRotation, Time.deltaTime * (carCurrentSpeed / 10));


        //wheels visual rotation
        Quaternion wheelTargetRotation = Quaternion.Euler(0, -20f, 0f);
        wheels[0].transform.rotation = Quaternion.Lerp(wheels[0].transform.rotation, wheelTargetRotation, Time.deltaTime * (carCurrentSpeed / 10));
        wheels[1].transform.rotation = Quaternion.Lerp(wheels[1].transform.rotation, wheelTargetRotation, Time.deltaTime * (carCurrentSpeed / 10));

        turning = true;
    }

    public void ArrowRight()
    {
        //turn power
        //carRb.AddForce(handling * Vector3.right * Time.deltaTime * (carCurrentSpeed / 10), ForceMode.VelocityChange);
        carRb.velocity = new Vector3(turnPower, carRb.velocity.y, carRb.velocity.z);

        //car model visual rotation
        Quaternion targetRotation = Quaternion.Euler(0f, 10f, 6f);
        carModel.transform.rotation = Quaternion.Lerp(carModel.transform.rotation, targetRotation, Time.deltaTime * (carCurrentSpeed / 10));

        //camera rotation
        Quaternion camTargetRotation = Quaternion.Euler(10f, 1f, -5f);
        playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, camTargetRotation, Time.deltaTime * (carCurrentSpeed / 10));

        //wheels visual rotation
        Quaternion wheelTargetRotation = Quaternion.Euler(0, 20f, 0f);
        wheels[0].transform.rotation = Quaternion.Lerp(wheels[0].transform.rotation, wheelTargetRotation, Time.deltaTime * (carCurrentSpeed / 10));
        wheels[1].transform.rotation = Quaternion.Lerp(wheels[1].transform.rotation, wheelTargetRotation, Time.deltaTime * (carCurrentSpeed / 10));

        turning = true;
    }

    public void ArrowRelease()
    {
        turning = false;
    }
    
    public void Break(bool _breaking)
    {
        breaking = _breaking;
    }

    public void Nitro(bool _nitroActive)
    {
        startedNitro = _nitroActive;

        playerCam.DOShakePosition(0.5f, 0.5f);
    }

    //collision handling
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "CarAI")
        {
            crashed = true;
        }
    }
}