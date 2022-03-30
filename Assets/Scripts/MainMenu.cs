using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject customizationCanvas;
    public GameObject statsCanvas;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleCustomization()
    {
        customizationCanvas.SetActive(!customizationCanvas.activeInHierarchy);
    }

    public void ToggleStatsBoard()
    {
        statsCanvas.SetActive(!statsCanvas.activeInHierarchy);
    }

    public void ClearStatsBoard()
    {
        StatsticsBoard.Instance.clearAllData();
    }
}
