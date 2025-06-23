using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenShop()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void ShowAboutUs()
    {
        SceneManager.LoadScene("AboutUsScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
