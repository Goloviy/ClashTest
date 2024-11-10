using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField]
    private bool ignoreBottom = true;

    private RectTransform _rectTransform = null;

    void OnEnable()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Destroy(this);
        return;
#endif

        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        if (_rectTransform != null)
        {
            if (!ignoreBottom)
            {
                _rectTransform.anchorMin = SafeAreaDetector.safeAnchor.anchorMin;
            }

            _rectTransform.anchorMax = SafeAreaDetector.safeAnchor.anchorMax;
        }
    }
}