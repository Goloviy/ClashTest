using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class SafeAreaDetector : MonoBehaviour
{
    public static SafeAnchor safeAnchor;

    private enum Devices
    {
        Normal,
        iPhoneX
    }

    private readonly Rect[] NSA_iPhoneX = new Rect[]
    {
        new Rect(0f, 102f / 2436f, 1f, 2202f / 2436f), // Portrait
        new Rect(132f / 2436f, 63f / 1125f, 2172f / 2436f, 1062f / 1125f) // Landscape
    };

    [SerializeField] private Devices simulator;

    private Rect _safeArea;

    void Awake()
    {
        _safeArea = GetSafeArea();
        RecalculateSafeAnchor();
        DontDestroyOnLoad(this);

#if UNITY_ANDROID && !UNITY_EDITOR
        Destroy(gameObject);
#endif
    }

    void LateUpdate()
    {
        if (_safeArea != GetSafeArea())
        {
            _safeArea = GetSafeArea();
            RecalculateSafeAnchor();
        }
    }

    private Rect GetSafeArea()
    {
#if UNITY_EDITOR
        if (simulator == Devices.iPhoneX)
        {
            Rect nsa = Screen.width < Screen.height ? NSA_iPhoneX[0] : NSA_iPhoneX[1];
            Rect safeArea = new Rect(Screen.width * nsa.x, Screen.height * nsa.y, Screen.width * nsa.width, Screen.height * nsa.height);
            return safeArea;
        }

        return Screen.safeArea;
#else
        return Screen.safeArea;
#endif
    }

    private void RecalculateSafeAnchor()
    {
        safeAnchor.anchorMin = _safeArea.position;
        safeAnchor.anchorMax = _safeArea.position + _safeArea.size;
        safeAnchor.anchorMin.x /= Screen.width;
        safeAnchor.anchorMin.y /= Screen.height;
        safeAnchor.anchorMax.x /= Screen.width;
        safeAnchor.anchorMax.y /= Screen.height;
    }
}