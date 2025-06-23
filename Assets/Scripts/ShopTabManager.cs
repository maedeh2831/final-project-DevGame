using UnityEngine;

public class ShopTabManager : MonoBehaviour
{
    public GameObject panelLives, panelSkins;

    public void ShowLives()
    {
        panelLives.SetActive(true);
        panelSkins.SetActive(false);
    }

    public void ShowSkins()
    {
        panelLives.SetActive(false);
        panelSkins.SetActive(true);
    }
}
