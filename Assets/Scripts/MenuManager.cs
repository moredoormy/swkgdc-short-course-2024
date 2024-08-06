using UnityEngine;
using UnityEngine.SceneManagement;

namespace SwkGDCShortCourse
{
    public class ManuManager : MonoBehaviour
    {
        public void LoadGameScene() => SceneManager.LoadScene("Game");

        public void ExitToMainMenu() => SceneManager.LoadScene("MainMenu");

        public void ExitGame() => Application.Quit();
    }
}
