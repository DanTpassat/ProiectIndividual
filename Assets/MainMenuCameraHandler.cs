using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuCameraHandler : MonoBehaviour
{
    public float animDuration;

    [Header("References")]
    public Image transitionImage;
    public GameObject mainMenuUI;
    
    [Header("Camera Transforms")]
    public Transform mainMenuCam;
    public Transform garageCam, mapSelectCam, dsCam;

    
    private Camera playerCam;

    private void Start()
    {
        playerCam = FindObjectOfType<Camera>();
    }


    //buttons scripts
    public void StartPress()
    {
        playerCam.transform.DOMove(mapSelectCam.position, animDuration);
        playerCam.transform.DORotate(mapSelectCam.eulerAngles, animDuration);
    }
    
    public void ShopPress()
    {
        Sequence moveSequence = DOTween.Sequence();
        Sequence rotateSequence = DOTween.Sequence();

        moveSequence.Append(playerCam.transform.DOMove(new Vector3(-4.75f, 7f, -78.25f), animDuration / 2));
        rotateSequence.Append(playerCam.transform.DORotate(new Vector3(0f, 0f, 0f), animDuration / 2));

        moveSequence.Append(playerCam.transform.DOMove(new Vector3(0f, 22.25f, 38f), animDuration));
        rotateSequence.Append(playerCam.transform.DORotate(new Vector3(20f, 270f, 0f), animDuration));

        moveSequence.Append(playerCam.transform.DOMove(new Vector3(-30.5f, 1.5f, 44f), animDuration / 2));
        rotateSequence.Append(playerCam.transform.DORotate(new Vector3(0f, 260f, 0f), animDuration / 2));

        moveSequence.Append(playerCam.transform.DOMove(dsCam.position, animDuration));
        rotateSequence.Append(playerCam.transform.DORotate(dsCam.eulerAngles, animDuration));

        

        ShopMenuHandler shopMenu = FindObjectOfType<ShopMenuHandler>();

        shopMenu.HandleDisplayCars();

        moveSequence.OnComplete(shopMenu.ShopMenuStart);
    }

    public void GaragePress()
    {
        Sequence moveSequence = DOTween.Sequence();
        Sequence rotateSequence = DOTween.Sequence();

        moveSequence.Append(playerCam.transform.DOMove(new Vector3(-4.75f, 7f, -78.25f), animDuration / 2));
        rotateSequence.Append(playerCam.transform.DORotate(new Vector3(0f, 0f, 0f), animDuration / 2));

        moveSequence.Append(playerCam.transform.DOMove(new Vector3(-14.25f, 16f, 61.5f), animDuration));
        rotateSequence.Append(playerCam.transform.DORotate(new Vector3(20f, 90f, 0f), animDuration));

        moveSequence.Append(playerCam.transform.DOMove(new Vector3(8.25f, 1.5f, 63.25f), animDuration / 2));
        rotateSequence.Append(playerCam.transform.DORotate(new Vector3(0f, 90f, 0f), animDuration / 2));

        moveSequence.Append(playerCam.transform.DOMove(garageCam.position, animDuration));
        rotateSequence.Append(playerCam.transform.DORotate(garageCam.eulerAngles, animDuration));

        PlayerGarageHandler garageHandler = FindObjectOfType<PlayerGarageHandler>();

        moveSequence.OnComplete(garageHandler.GarageMenuStart);
    }

    public void BackPress()
    {
        Sequence backSequence = DOTween.Sequence();

        backSequence.Append(transitionImage.DOFade(1f, animDuration));
        backSequence.Append(playerCam.transform.DOMove(mainMenuCam.position, 0.1f));
        backSequence.Append(playerCam.transform.DORotate(mainMenuCam.eulerAngles, 0.1f));
        backSequence.Append(transitionImage.DOFade(0f, animDuration));

        backSequence.OnComplete(ActivateMainMenuButtons);
    }

    private void ActivateMainMenuButtons()
    {
        mainMenuUI.SetActive(true);
    }
}
