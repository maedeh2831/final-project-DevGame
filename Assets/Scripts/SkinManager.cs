using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkinManager : MonoBehaviour
{
    public List<SkinItem> skinItems;
    public int playerScore = 0;
    public GameManager gameManager;
    public GameObject ballObject;

    private const string SelectedSkinKey = "SelectedSkin";
    private const string SkinUnlockPrefix = "SkinUnlocked_";

    void Start()
    {
        if (PlayerPrefs.GetInt(SkinUnlockPrefix + "0", 0) == 0)
        {
            PlayerPrefs.SetInt(SkinUnlockPrefix + "0", 1);
            PlayerPrefs.Save();
        }

        int playerScore = gameManager != null ? gameManager.GetScore() : 0;

        foreach (var item in skinItems)
        {
            bool isUnlocked = PlayerPrefs.GetInt(SkinUnlockPrefix + item.skinID, 0) == 1;

            if (!item.unlockedByAd && playerScore >= item.requiredScore && !isUnlocked)
            {
                isUnlocked = true;
                PlayerPrefs.SetInt(SkinUnlockPrefix + item.skinID, 1);
            }

            item.SetUnlocked(isUnlocked);
            item.RefreshButton(SelectSkin);
        }

        if (!PlayerPrefs.HasKey(SelectedSkinKey))
        {
            PlayerPrefs.SetInt(SelectedSkinKey, 0);
            PlayerPrefs.Save();
        }

        SelectSkin(PlayerPrefs.GetInt(SelectedSkinKey, 0));
    }

    public void SelectSkin(int skinID)
    {
        SkinItem selected = skinItems.Find(s => s.skinID == skinID);

        if (selected != null && selected.isUnlocked)
        {
            PlayerPrefs.SetInt(SelectedSkinKey, skinID);
            PlayerPrefs.Save();

            ApplySkin(skinID);

            foreach (var item in skinItems)
            {
                item.UpdateLockText();
                item.RefreshButton(SelectSkin);
            }
        }
        else
        {
            Debug.LogWarning($"Tried to select locked or missing skin {skinID}");
        }
    }

    private void ApplySkin(int skinID)
    {
        SkinItem skin = skinItems.Find(s => s.skinID == skinID);
        if (skin != null && ballObject != null)
        {
            SpriteRenderer sr = ballObject.GetComponent<SpriteRenderer>();
            if (sr != null && skin.skinSprite != null)
            {
                sr.sprite = skin.skinSprite;
            }
        }
    }

    public void UnlockSkinByAd(int skinID)
    {
        SkinItem item = skinItems.Find(i => i.skinID == skinID);
        if (item != null && !item.isUnlocked)
        {
            item.SetUnlocked(true);
            PlayerPrefs.SetInt(SkinUnlockPrefix + skinID, 1);
            PlayerPrefs.Save();
        }
    }
}
