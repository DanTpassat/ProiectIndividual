using UnityEngine;

public class SkidMarkHandler : MonoBehaviour
{
    private TrailRenderer skidMark;
    private ParticleSystem smoke;
    float fadeOutSpeed;

    private CarController playerCar;

    private void Awake()
    {
        smoke = GetComponent<ParticleSystem>();
        skidMark = GetComponent<TrailRenderer>();
        playerCar = FindObjectOfType<CarController>();
        skidMark.emitting = false;
        skidMark.startWidth = 0.2f;
    }


    private void OnEnable()
    {
        skidMark.enabled = true;
    }
    private void OnDisable()
    {
        skidMark.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playerCar.crashed)
        {
            if ((playerCar.turnPower > playerCar.handling / 2 && playerCar.carCurrentSpeed > playerCar.baseSpeed / 2 && playerCar.turnTime > 0.5f) || (playerCar.breaking && playerCar.carCurrentSpeed > playerCar.baseSpeed / 2))
            {
                fadeOutSpeed = 0f;
                skidMark.materials[0].color = Color.black;
                skidMark.emitting = true;
            }
            else
            {
                skidMark.emitting = false;
            }
        }
        else
        {
            skidMark.emitting = false;
        }

        if (!skidMark.emitting)
        {
            fadeOutSpeed += Time.deltaTime / 2;
            Color m_color = Color.Lerp(Color.black, new Color(0f, 0f, 0f, 0f), fadeOutSpeed);
            skidMark.materials[0].color = m_color;
            if (fadeOutSpeed > 1)
            {
                skidMark.Clear();
            }
        }

        // smoke
        if (skidMark.emitting == true)
        {
            smoke.Play();
        }
        else 
        { 
            smoke.Stop(); 
        }

    }
}