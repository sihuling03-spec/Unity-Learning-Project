using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[AddComponentMenu("Language/LanguageText")]
public class LanguageText : MonoBehaviour
{
    [HideInInspector] public string Language;
    [HideInInspector] public string File;
    [HideInInspector] public string Key;
    [HideInInspector] public string Value;
    public bool isToUpper = false;
     
    public LanguageService Localization;

    private Text _label;
    [HideInInspector] public Text label{ 
        get  { 
        if (_label == null)
            {
                _label = GetComponent<Text>();
            }
        return _label;
        }  
    }

    void Start()
    {
        Localization = LanguageService.Instance;

    
        if (isToUpper)
        {
            label.text = Localization.GetStringByKey(label,Key).ToUpper();
        }
        else
        {
            label.text = Localization.GetStringByKey(label, Key);
        }

        // TODO:EventCenter.Instance.OnLanguageChanged.AddListener(OnLanguageChange);
    }

    void OnLanguageChange()
    {
        if (isToUpper)
        {
            label.text = Localization.GetStringByKey(label,Key).ToUpper();
        }
        else
        {
            label.text = Localization.GetStringByKey(label,Key);
        }
    }

    private void OnDestroy()
    {
        // TODO:EventCenter.Instance.OnLanguageChanged.RemoveListener(OnLanguageChange);
    }
}
