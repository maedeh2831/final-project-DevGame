using Bazaar.Data;
using Bazaar.Poolakey;
using Bazaar.Poolakey.Data;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    public static PurchaseManager Instance { get; private set; }

    [Header("Poolakey App Key")]
    [TextArea]
    [SerializeField] private string AppKey = "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwDmfmsx6yupAMazJrEhZlKyPn8mske2MVETvzPJrzMLXokOYyzuyKUSAX1+z/4mOD8e4O7hOU/ByGlEUp7DnYX+l7oGSgsan0rdVKmqVQ/0eNXOEAKGF9dpyHroSLg7YNmQjGuHB7kBcuPAwOnzoI7C0p8GlW0oWBNmtkIFuVMiMJd96mubriyCwxuHD3BPwJZZ3K6JryJ9LuasOq1qzkNCEYM3zsHVVR0/zukIZ5UCAwEAAQ==";

    private Payment _payment;
    private bool _isConnected = false;

    #region Singleton & Initialization

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async Task<bool> Init()
    {
        if (_isConnected && _payment != null) return true;

        try
        {

            var securityCheck = SecurityCheck.Enable(AppKey);
            var config = new PaymentConfiguration(securityCheck);
            _payment = new Payment(config);

            var result = await _payment.Connect();
            _isConnected = result.status == Status.Success;

            return _isConnected;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    #endregion

    #region Purchase

    public async Task<Result<PurchaseInfo>> Purchase(string productId)
    {
        if (!_isConnected)
        {
            var connected = await Init();
            if (!connected)
                return new Result<PurchaseInfo>(Status.Failure, "Not connected to Poolakey", null);
        }

        try
        {
            Debug.Log($"[Poolakey] Attempting to purchase: {productId}");
            var result = await _payment.Purchase(productId);

            if (result.status == Status.Success)
                Debug.Log("[Poolakey] Purchase successful.");
            else
                Debug.LogWarning($"[Poolakey] Purchase failed: {result.message}");

            return result;
        }
        catch (Exception ex)
        {
            return new Result<PurchaseInfo>(Status.Failure, ex.Message, null);
        }
    }

    #endregion

    #region Consume

    public async Task<Result<bool>> Consume(string purchaseToken)
    {
        if (!_isConnected)
        {
            var connected = await Init();
            if (!connected)
                return new Result<bool>(Status.Failure, "Not connected to Poolakey");
        }

        try
        {
            Debug.Log($"[Poolakey] Consuming token: {purchaseToken}");
            var result = await _payment.Consume(purchaseToken);

            if (result.status == Status.Success)
                Debug.Log("[Poolakey] Consume successful.");
            else
                Debug.LogWarning($"[Poolakey] Consume failed: {result.message}");

            return result;
        }
        catch (Exception ex)
        {
            return new Result<bool>(Status.Failure, ex.Message);
        }
    }

    #endregion

    #region Disconnect

    private void OnApplicationQuit()
    {
        if (_payment != null && _isConnected)
        {
            _payment.Disconnect();
            _isConnected = false;
        }
    }

    #endregion
}
