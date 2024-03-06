using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using Utilla;
using UnityEngine.UI;

[BepInPlugin("com.yourname.gorillatagwebmod", "Gorilla Tag Web Mod", "1.0.0")]
public class GorillaTagWebMod : BaseUnityPlugin
{
    private GameObject webViewObject;
    private WebView webViewComponent;
    private bool isWebViewActive = false;
    
    private ConfigEntry<string> urlConfig;

    void Awake()
    {
        urlConfig = Config.Bind("General", "URL", "https://example.com", "The URL to load in the web view");
    }

    void Start()
    {
        webViewObject = new GameObject("WebViewObject");
        webViewComponent = webViewObject.AddComponent<WebView>();

        // Set up the web view's properties
        webViewComponent.Init(new Vector2(800, 600), false);
        webViewComponent.Align = WebViewAlign.Full;
        webViewComponent.Position = new Vector2(0.5f, 0.5f);
        webViewComponent.Scale = new Vector2(0.5f, 0.5f);

        // Load the initial URL
        webViewComponent.LoadURL(urlConfig.Value);
        webViewComponent.Show(false);

        Utilla.Events.OnGorillaLocalGrab += ToggleWebView;
    }

    void Update()
    {
        if (isWebViewActive)
        {
            // Update the web view's position to match the player's hand
            Vector3 handPosition = PlayerManager.LocalPlayer.LeftHand.Position;
            webViewComponent.transform.position = handPosition;
        }
    }

    void ToggleWebView(GameObject obj)
    {
        if (obj == PlayerManager.LocalPlayer.gameObject)
        {
            isWebViewActive = !isWebViewActive;
            webViewComponent.Show(isWebViewActive);
        }
    }

    void OnDestroy()
    {
        Utilla.Events.OnGorillaLocalGrab -= ToggleWebView;
    }
}
