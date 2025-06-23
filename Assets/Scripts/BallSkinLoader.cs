using UnityEngine;

public class BallSkinLoader : MonoBehaviour
{
    public Sprite[] skinSprites;

    void Start()
    {
        int selectedSkinID = PlayerPrefs.GetInt("SelectedSkin", 0);

        if (selectedSkinID < 0 || selectedSkinID >= skinSprites.Length)
        {
            selectedSkinID = 0;
        }

        if (TryGetComponent(out SpriteRenderer sr))
        {
            sr.sprite = skinSprites[selectedSkinID];
        }
        else
        {
            Debug.LogError("BallSkinLoader: SpriteRenderer .");
        }
    }
}
