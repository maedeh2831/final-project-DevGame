using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinItem : MonoBehaviour
{
    [Header("Skin Info")]
    public int skinID;
    public int requiredScore = 0;
    public bool unlockedByAd = false;
    public bool isUnlocked = false;

    [Header("UI References")]
    public Button selectButton;
    public GameObject lockPanel;
    public Button adButton;
    public TextMeshProUGUI lockText;

    [Header("Dependencies")]
    public VideoAd videoAd;
    public GameManager gameManager;

    public Sprite skinSprite;

    void Start()
    {
        int currentScore = gameManager != null ? gameManager.GetScore() : 0;

        isUnlocked = PlayerPrefs.GetInt("SkinUnlocked_" + skinID, 0) == 1;

        if (!isUnlocked && !unlockedByAd && currentScore >= requiredScore)
        {
            isUnlocked = true;
            PlayerPrefs.SetInt("SkinUnlocked_" + skinID, 1);
            PlayerPrefs.Save();
        }

        SetUnlocked(isUnlocked);

        if (adButton != null && unlockedByAd && !isUnlocked)
        {
            adButton.onClick.RemoveAllListeners();
            adButton.onClick.AddListener(() =>
            {
                if (videoAd != null)
                {
                    videoAd.WatchAdForSkin(skinID);
                }
            });
        }
    }

    public void SetUnlocked(bool value)
    {
        isUnlocked = value;

        if (lockPanel != null)
            lockPanel.SetActive(!value);

        if (selectButton != null)
            selectButton.interactable = value;

        if (adButton != null)
            adButton.gameObject.SetActive(!value && unlockedByAd);

        UpdateLockText();
    }

    public void RefreshButton(System.Action<int> onSelectCallback)
    {
        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();

            if (isUnlocked)
            {
                selectButton.interactable = true;
                selectButton.onClick.AddListener(() =>
                {
                    onSelectCallback?.Invoke(skinID);
                });
            }
            else
            {
                selectButton.interactable = false;
            }
        }

        UpdateLockText();
    }

    public void UpdateLockText()
    {
        if (lockText == null) return;

        if (!isUnlocked && !unlockedByAd)
        {
            lockText.text = requiredScore + " Score";
        }
        else
        {
            lockText.text = PlayerPrefs.GetInt("SelectedSkin", 0) == skinID ? "Selected" : "Select";
        }
    }
}
