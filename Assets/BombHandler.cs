using TMPro;
using UnityEngine;

public class BombHandler : MonoBehaviour
{
    [Header("Stats")]
    public float bombTimer;

    [Header("References")]
    public TMP_Text bombTimerText;

    //variables
    public float remainingTime;

    private Rigidbody playerRb;
    private float playerSpeed;
    private CarController carController;

    // Start is called before the first frame update
    void Start()
    {
        bombTimerText.text = bombTimer.ToString();

        playerRb = FindObjectOfType<CarController>().GetComponent<Rigidbody>();
        carController = FindObjectOfType<CarController>();

        remainingTime = 60f;
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = playerRb.velocity.magnitude;

        ModifyBombTimer();

        bombTimerText.text = remainingTime.ToString("F2");

        //check for bomb timer
        if (remainingTime < 0f)
        {
            carController.crashed = true;
        }
    }

    private void ModifyBombTimer()
    {
        if(playerSpeed < carController.baseSpeed / 2)
        {
            remainingTime = remainingTime - Time.deltaTime * 2;
            bombTimerText.color = Color.red;
        }
        else if(playerSpeed > carController.baseSpeed && carController.startedNitro && carController.elapsedNitroTime > 0f)
        {
            remainingTime = remainingTime + Time.deltaTime;
            bombTimerText.color = Color.green;
        }
        else
        {
            remainingTime = remainingTime - Time.deltaTime;
            bombTimerText.color = Color.white;
        }

        if(remainingTime > bombTimer)
        {
            remainingTime = bombTimer;
        }
    }
}
