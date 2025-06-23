using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapsellPlusSDK;

public class VideoAd : MonoBehaviour
{
    [Header("Ad Settings")]
    public string zoneId = "6842f854298c6e6166a1267b";


    [Header("Skin Unlocking")]
    public SkinManager skinManager;
    private int skinIdToUnlock = -1;

    private string responseId;

    void Start()
    {
        InitializeAds();
    }

    void InitializeAds()
    {

        TapsellPlus.Initialize(
            "pobsigrqjgtketaqakdpecgqcrfieppmkfnbabfkdhdbbrsngjoeplognkdsbimrhfqsfr",
            adNetworkName =>
            {
            },
            error =>
            {
            }
        );
    }
    public void WatchAdForSkin(int skinID)
    {
        ShowAdWithCallback(() =>
        {
            skinManager.UnlockSkinByAd(skinID);
            skinManager.SelectSkin(skinID);
        });
    }
    public void RequestAndShowAd()
    {

        TapsellPlus.RequestRewardedVideoAd(zoneId,
            adModel =>
            {
                responseId = adModel.responseId;
                ShowAd(responseId);
            },
            error =>
            {
            }
        );
    }

    public void ShowAdWithCallback(System.Action onRewardCallback)
    {

        TapsellPlus.RequestRewardedVideoAd(zoneId,
            adModel =>
            {
                responseId = adModel.responseId;

                TapsellPlus.ShowRewardedVideoAd(responseId,
                    onOpen =>
                    {
                    },
                    onReward =>
                    {
                        onRewardCallback?.Invoke();
                    },
                    onClose =>
                    {
                    },
                    onError =>
                    {
                    }
                );
            },
            error =>
            {
            }
        );
    }

    private void ShowAd(string id)
    {

        TapsellPlus.ShowRewardedVideoAd(id,
            onOpen =>
            {
            },
            onReward =>
            {
            },
            onClose =>
            {
            },
            error =>
            {
            }
        );
    }
}
