using System;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine;
using TMPro;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string GAME_ID = "5276583"; //replace with your gameID from dashboard. note: will be different for each platform.

    private const string BANNER_PLACEMENT = "Banner_Android";
    private const string VIDEO_PLACEMENT = "Interstitial_Android";
    private const string REWARDED_VIDEO_PLACEMENT = "Rewarded_Android";

    [SerializeField] private BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;

    private bool testMode = true;
    private bool showBanner = false;

    public TextMeshProUGUI vidasTxt;
    public TextMeshProUGUI mensajes;
    private float numVidas;

    //utility wrappers for debuglog
    public delegate void DebugEvent(string msg);
    public static event DebugEvent OnDebugLog;

    private void Start() {
        numVidas = 0;
    }

    public void Initialize()
    {
        if (Advertisement.isSupported)
        {
            //DebugLog(Application.platform + " supported by Advertisement");
            DebugLog("Inicializamos los anuncios");
            mensajes.text = "Inicializamos los anuncios";
        }
        Advertisement.Initialize(GAME_ID, testMode, this);
    }

    public void ToggleBanner() 
    {
        showBanner = !showBanner;

        if (showBanner)
        {
            Advertisement.Banner.SetPosition(bannerPosition);
            Advertisement.Banner.Show(BANNER_PLACEMENT);
            DebugLog("Mostramos el banner");
            mensajes.text = "Mostramos el banner";
        }
        else
        {
            Advertisement.Banner.Hide(false);
            DebugLog("Escondemos el banner");
            mensajes.text = "Escondemos el banner";
        }
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(REWARDED_VIDEO_PLACEMENT, this);
        DebugLog("Cargamos el anuncio con recompensa");
        mensajes.text = "Cargamos el anuncio con recompensa";
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
        numVidas++;
        vidasTxt.text = "Vidas: " + numVidas;
        DebugLog("Enseñamos el anuncio con recompensa");
        mensajes.text = "Enseñamos el anuncio con recompensa";
    }

    public void LoadNonRewardedAd()
    {
        Advertisement.Load(VIDEO_PLACEMENT, this);
        DebugLog("Cargamos el anuncio sin recompensa");
        mensajes.text = "Cargamos el anuncio sin recompensa";
    }

    public void ShowNonRewardedAd()
    {
        Advertisement.Show(VIDEO_PLACEMENT, this);
        DebugLog("Enseñamos el anuncio sin recompensa");
        mensajes.text = "Enseñamos el anuncio sin recompensa";
    }

    #region Interface Implementations
    public void OnInitializationComplete()
    {
        DebugLog("Init Success");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        DebugLog($"Init Failed: [{error}]: {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        DebugLog($"Load Success: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        DebugLog($"Load Failed: [{error}:{placementId}] {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        DebugLog($"OnUnityAdsShowFailure: [{error}]: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        DebugLog($"OnUnityAdsShowStart: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        DebugLog($"OnUnityAdsShowClick: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        DebugLog($"OnUnityAdsShowComplete: [{showCompletionState}]: {placementId}");
    }
    #endregion

    public void OnGameIDFieldChanged(string newInput)
    {
        GAME_ID = newInput;
    }

    public void ToggleTestMode(bool isOn)
    {
        testMode = isOn;
    }

    //wrapper around debug.log to allow broadcasting log strings to the UI
    void DebugLog(string msg)
    {
        OnDebugLog?.Invoke(msg);
        Debug.Log(msg);
    }
}
