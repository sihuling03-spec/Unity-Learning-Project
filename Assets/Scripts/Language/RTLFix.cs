using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RTLFix : MonoBehaviour
{ 
    private void Start()
    { 
        ChangeAlignment();
        // TODO:EventCenter.Instance.OnLanguageChanged.AddListener(ChangeAlignment);
    } 
     
    private void ChangeAlignment()
    {
        if (LanguageService.Instance.CurrentLanguage == (int)LanguageType.AR||(LanguageService.Instance.CurrentLanguage == (int)LanguageType.UR))
        {
            var text = GetComponent<Text>();
            if (text.alignment == TextAnchor.UpperLeft)
            {
                text.alignment = TextAnchor.UpperRight;
            }
            if (text.alignment == TextAnchor.MiddleLeft)
            {
                text.alignment = TextAnchor.MiddleRight; 
            }
            if (text.alignment == TextAnchor.LowerLeft)
            {
                text.alignment = TextAnchor.LowerRight;
            }
        }
        else
        {
            var text = GetComponent<Text>();
            if (text.alignment == TextAnchor.UpperRight)
            {
                text.alignment = TextAnchor.UpperLeft;
            }
            if (text.alignment == TextAnchor.MiddleRight)
            {
                text.alignment = TextAnchor.MiddleLeft;
            }
            if (text.alignment == TextAnchor.LowerRight)
            {
                text.alignment = TextAnchor.LowerLeft;
            }
        }
    }

    private void OnDestroy()
    {
        // TODO:EventCenter.Instance.OnLanguageChanged.RemoveListener(ChangeAlignment);
    }
}
