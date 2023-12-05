using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;

public class ShopMenuHandler : MonoBehaviour
{
    [Header("Objects")]
    public List<Car> cars = new();

    [Header("Text References")]
    public TMP_Text carNameText;
    public TMP_Text carSpeedText, carAccelerationText, carHandlingText, carPriceText;
    public Transform display1, display2, display3, display4;

    
    [Header("References")]
    public GameObject shopUI;
    public Transform[] dsCams;
    public GameObject arrowLeft, arrowRight;

    [SerializeField] private int carSelected = 0;
    private Camera playerCam;

    private GameObject car1, car2, car3, car4;

    private void Start()
    {   
        playerCam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleShopArrows();

        HandleText();
    }

    //update methods
    private void HandleText()
    {
        carNameText.text = cars[carSelected].name;
        carSpeedText.text = cars[carSelected].baseSpeed.ToString();
        carAccelerationText.text = cars[carSelected].acceleration.ToString();
        carHandlingText.text = cars[carSelected].handling.ToString();
        carPriceText.text = cars[carSelected].price.ToString() + "$";
    }
    public void HandleDisplayCars()
    {
        int carsPack = carSelected / 4;

        if(carSelected % 4 == 0)
        {
            Destroy(car1);
            Destroy(car2);
            Destroy(car3);
            Destroy(car4);
            
            car1 = Instantiate(cars[carsPack * 4].carModel, display1.position, display1.rotation);
            car2 = Instantiate(cars[carsPack * 4 + 1].carModel, display2.position, display2.rotation);
            car3 = Instantiate(cars[carsPack * 4 + 2].carModel, display3.position, display3.rotation);
            car4 = Instantiate(cars[carsPack * 4 + 3].carModel, display4.position, display4.rotation);

            car1.transform.SetParent(display1.transform);
            car2.transform.SetParent(display2.transform);
            car3.transform.SetParent(display3.transform);
            car4.transform.SetParent(display4.transform);
        }
    }

    private void HandleShopArrows()
    {
        if (carSelected == 0)
        {
            arrowLeft.SetActive(false);
        }
        else
        {
            arrowLeft.SetActive(true);
        }

        if (carSelected == cars.Count - 1)
        {
            arrowRight.SetActive(false);
        }
        else
        {
            arrowRight.SetActive(true);
        }
    }

    //button methods
    public void ArrowLeftPress()
    {
        carSelected--;

        playerCam.transform.DOMove(dsCams[carSelected % 4].position, 1f);
        playerCam.transform.DORotate(dsCams[carSelected % 4].eulerAngles, 1f);

        HandleDisplayCars();
    }

    public void ArrowRightPress()
    {
        carSelected++;

        playerCam.transform.DOMove(dsCams[carSelected % 4].position, 1f);
        playerCam.transform.DORotate(dsCams[carSelected % 4].eulerAngles, 1f);

        HandleDisplayCars();
    }


    //dotween animation handling
    public void ShopMenuStart()
    {
        carSelected = 0;
        shopUI.SetActive(true);
    }
}
