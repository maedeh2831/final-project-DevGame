using UnityEngine;
using Bazaar.Poolakey.Data;

public class PurchaseButtonHandler : MonoBehaviour
{
    public ProductIdProvider productIdProvider;
    public PurchaseManager purchaseManager;
    public GameObject purchaseProgressingPanel;
    public HealthManager healthManager;


    private void OnEnable()
    {
        if (purchaseManager == null || !purchaseManager.gameObject.activeInHierarchy)
            purchaseManager = PurchaseManager.Instance;

        if (healthManager == null)
            healthManager = FindObjectOfType<HealthManager>();

        if (productIdProvider == null)
            productIdProvider = GetComponent<ProductIdProvider>();

    }


    public async void BuyProduct()
    {
        if (purchaseManager == null)
        {
            return;
        }

        if (productIdProvider == null || string.IsNullOrEmpty(productIdProvider.productId))
        {
            return;
        }

        string productId = productIdProvider.productId;

        if (purchaseProgressingPanel != null)
        {
            purchaseProgressingPanel.SetActive(true);
        }

        try
        {
            bool isInitialized = await purchaseManager.Init();
            if (!isInitialized)
            {
                return;
            }

            var purchaseResult = await purchaseManager.Purchase(productId);
            if (purchaseResult.status != Bazaar.Data.Status.Success)
            {
                return;
            }

            string purchaseToken = purchaseResult.data.purchaseToken;
            var consumeResult = await purchaseManager.Consume(purchaseToken);
            if (consumeResult.status != Bazaar.Data.Status.Success)
            {
                return;
            }


            if (healthManager != null)
            {
                switch (productId)
                {
                    case "Health_1":
                        healthManager.AddLives(1);
                        break;
                    case "Health_2":
                        healthManager.AddLives(5);
                        break;
                    case "Health_3":
                        healthManager.AddLives(10);
                        break;
                    case "Health_4":
                        healthManager.AddLives(15);
                        break;
                    case "Health_5":
                        healthManager.AddLives(20);
                        break;
                    case "Health_6":
                        healthManager.AddLives(30);
                        break;
                    default:
                        break;
                }
            }
        }
        finally
        {
            if (purchaseProgressingPanel != null)
            {
                purchaseProgressingPanel.SetActive(false);
            }
        }
    }
}
