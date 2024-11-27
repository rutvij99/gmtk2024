using DG.Tweening;
using UnityEngine;

public class SimulationUIHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup pauseMenu;
    private bool isVisible = false;
    public void TogglePause()
    {
        if (pauseMenu.gameObject.activeInHierarchy)
        {
            // pauseMenu.alpha = 1;
            pauseMenu.DOFade(0, 0.1f).OnComplete(() =>
            {
                pauseMenu.gameObject.SetActive(false);
            });
        }
        else
        {
            pauseMenu.gameObject.SetActive(true);
            pauseMenu.alpha = 0;
            pauseMenu.DOFade(1, 0.5f).OnComplete(() =>
            {
                
            });
        }
    }
}
