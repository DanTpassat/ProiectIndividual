using UnityEngine;

public class PlayerGarageHandler : MonoBehaviour
{
    [Header("References")]
    public GameObject garageMenuUI;

    public void GarageMenuStart()
    {
        garageMenuUI.SetActive(true);
    }

}
