using UnityEngine;
using UnityEngine.UI;

public class TextARB:MonoBehaviour
{
    Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }
    private void Start()
    {   
        string str  = text.text;
       text.text =   ArabicSupport.ArabicFixer.FixTextForUI(text, str);
    }
}