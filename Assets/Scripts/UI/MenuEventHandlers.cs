using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuEventHandlers : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene((int) Scenes.Level1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene((int) Scenes.Level1);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene((int) Scenes.MainMenu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
