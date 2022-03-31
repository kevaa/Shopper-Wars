using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public GameObject customizationCanvas;
    public GameObject statsCanvas;
    [SerializeField] Toggle endGameToggle;

    public void PlayGame()
    {
        PlayerPrefs.SetInt("EndGameEarly", endGameToggle.isOn ? 1 : 0);
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
        PlayerPrefs.DeleteAll();
    }

}
