using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuEventHandlers : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
