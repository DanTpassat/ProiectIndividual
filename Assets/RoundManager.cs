using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    [Header("Settings")]
    public int levelDistance;


    [Header("Stats")]
    public float distanceTravelled = 0f;
    public int coinsCollected = 0, totalCoinsValue = 0, nearMissCount = 0;
    //public float wrongLaneTime = 0f;
    public float bombTimeRemaining = 0f;

    [Header("References")]
    public Slider gameProgressSlider;
    public GameObject gameSummaryUI;
    public GameObject retryButton, nextButton;
    public TMP_Text gameSummaryText;

    //public bool respawned = false;

    private bool roundWon = false;

    private CarController player;

    private void Start()
    {
        player = FindObjectOfType<CarController>();
    }

    private void Update()
    {
        gameProgressSlider.value = Mathf.Lerp(0f, 1f, player.transform.position.z / levelDistance);
        
        if (player.transform.position.z > levelDistance)
        {
            roundWon = true;
            HandleFinish();
        }

        if (player.crashed)
        {
            HandleFinish();
        }
    }

    private void HandleFinish()
    {
        if (roundWon)
        {
            retryButton.SetActive(false);
            nextButton.SetActive(true);
            gameSummaryText.text = "LEVEL CLEARED";
        }
        else
        {
            retryButton.SetActive(true);
            nextButton.SetActive(false);
            gameSummaryText.text = "GAME OVER";
        }

        gameSummaryUI.SetActive(true);
    }

    private void SaveProgress()
    {

    }

    //button methods
    public void Retry()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        SaveProgress();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        SaveProgress();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        SaveProgress();
    }
}
