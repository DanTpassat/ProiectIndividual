using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseMenuHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    
    CameraShake camShake;

    private void Start()
    {
        camShake = FindObjectOfType<CameraShake>();
    }
    public void PauseTime()
    {
        pauseMenu.transform.localScale = Vector3.zero;
        pauseMenu.transform.DOScale(1f, 0.5f).SetEase(Ease.InBounce).SetUpdate(UpdateType.Normal, true);

        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, 0.25f);
        camShake.enabled = false;
    }

    public void ResumeTime()
    {
        pauseMenu.transform.DOScale(0f, 0.75f).SetEase(Ease.OutBounce).SetUpdate(UpdateType.Normal, true);

        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.5f);
        camShake.enabled = true;
    }

    public void ReturnToMenu()
    {
        DOTween.KillAll();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
