using System.Collections;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class YandexFunctions : MonoBehaviour
{
    [SerializeField] private GameObject _yandexHolder;

    private bool _isRateRequesSend;
    private bool _isCurrencyCodeGet;
    private bool _isTextureRequesSend;

    public Texture2D CurrencyTexture;
    public bool IsTextureLoaded;

    public string CurrencyCode;

    [DllImport("__Internal")]
    private static extern void RateGame();

    [DllImport("__Internal")]
    private static extern void ShowAdvBetweenLevels();

    [DllImport("__Internal")]
    private static extern void ShowAdvBeforeMainMenu();

    [DllImport("__Internal")]
    private static extern void ShowDoubleAdv(int reward);

    [DllImport("__Internal")]
    private static extern void BuySpeedUp();

    [DllImport("__Internal")]
    private static extern void BuySlowDown();

    [DllImport("__Internal")]
    private static extern void BuySpeedUpX10();

    [DllImport("__Internal")]
    private static extern void BuySlowDownX10();

    [DllImport("__Internal")]
    private static extern void Buy1000FatCoins();

    [DllImport("__Internal")]
    private static extern void CheckUnprocessedPurchases();

    [DllImport("__Internal")]
    private static extern string SetFocus();

    [DllImport("__Internal")]
    private static extern string GetProductCost(string productID);

    [DllImport("__Internal")]
    private static extern string GetCurrencyCode(string productID);

    [DllImport("__Internal")]
    private static extern string GetPortalCurrencySpriteUrl(string productID);

    private void Awake()
    {
        DontDestroyOnLoad(_yandexHolder);
    }

    public void SetFocusOnGame()
    {
        SetFocus();
    }

    public void CheckPurchases()
    {
        CheckUnprocessedPurchases();
    }

    public void RateGameRequest()
    {
        if (_isRateRequesSend) return;

        RateGame();
        _isRateRequesSend = true;
    }

    public void ShowAdvertisingBetweenLevels()
    {
        ShowAdvBetweenLevels();
    }

    public void ShowAdvertisingBeforeMainMenu()
    {
        ShowAdvBeforeMainMenu();
    }

    public void ShowDoubleAdvertising(int reward)
    {
        ShowDoubleAdv(reward);
    }

    public void Buy(bool isSpeedUp)
    {
        if(isSpeedUp)
        {
            BuySpeedUp();
        }
        else
        {
            BuySlowDown();
        }
    }

    public void BuyX10(bool isSpeedUp)
    {
        if (isSpeedUp)
        {
            BuySpeedUpX10();
        }
        else
        {
            BuySlowDownX10();
        }
    }

    public void BuyFatCoins()
    {
        Buy1000FatCoins();
    }

    public string GetCost(string productID)
    {
        if (!_isTextureRequesSend)
        {
            _isTextureRequesSend = true;
            var url = "https:" + GetSpriteUrl(productID);
            StartCoroutine(GetTexture(url));
        }

        return GetProductCost(productID);
    }

    public string GetCode(string productID)
    {
        if (!_isCurrencyCodeGet)
        {
            CurrencyCode = GetCurrencyCode(productID);
            _isCurrencyCodeGet = true;
        }

        return CurrencyCode;
    }

    private string GetSpriteUrl(string productID)
    {
        return GetPortalCurrencySpriteUrl(productID);
    }

    IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        if (www.downloadHandler.isDone)
        {
            CurrencyTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            IsTextureLoaded = true;
        }
        else
        {
            _isTextureRequesSend = false;
        }
    }
}
