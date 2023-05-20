using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 0 - Index of Menu Scene in Scenes List (See File -> Build Settings)
            SceneManager.LoadScene(0);
            Cursor.visible = true;
        }
    }
}
