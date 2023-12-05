using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MapSelectorHandler : MonoBehaviour
{
    [Header("References")]
    public TMP_Text mapNameText;
    public TMP_Text currentMapLevelText;
    public List<GameObject> mapObjects = new List<GameObject>();
    public GameObject arrowLeft, arrowRight;

    [HideInInspector] public int mapSelected = 0;

    private Camera playerCam;
    private PlayerHandler playerHandler;

    private void Awake()
    {
        arrowLeft.SetActive(false);
        arrowRight.SetActive(true);

        playerCam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if(mapSelected == 0)
        {
            arrowLeft.SetActive(false);
        }
        else
        {
            arrowLeft.SetActive(true);
        }

        if(mapSelected == mapObjects.Count - 1)
        {
            arrowRight.SetActive(false);
        }
        else
        {
            arrowRight.SetActive(true);
        }

        //text handling
        switch (mapSelected)
        {
            case 0:
                mapNameText.text = "Town";
                //currentMapLevelText.text = playerHandler.map1level.ToString();
                break;
            case 1:
                mapNameText.text = "Desert";
                //currentMapLevelText.text = playerHandler.map2level.ToString();
                break;
        }
    }

    //map selector buttons
    public void ArrowLeftPress()
    {
        mapObjects[mapSelected].SetActive(false);
        mapSelected--;
        mapObjects[mapSelected].SetActive(true);

        playerCam.transform.DOPunchRotation(new Vector3(0, 360, 0), 0.25f);
    }

    public void ArrowRightPress()
    {
        mapObjects[mapSelected].SetActive(false);
        mapSelected++;
        mapObjects[mapSelected].SetActive(true);

        playerCam.transform.DOPunchRotation(new Vector3(0, 360, 0), 0.25f);
    }

    public void PlayPress()
    {
        if(mapSelected == 0)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}
