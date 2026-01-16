/*****************************************************************************
*
* @brief : 
* @author : Lv Xiaoquan
* Compiler: Unity 2018.3.4f1 (64-bit）
* @date : 2019/9/27 18:03
* @version : ver 1.0
* @inparam : 
* @outparam : 
*
*****************************************************************************/
//Modify By Lv xiaoquan @2019/09/27 添加多语言支持
using SimpleJSON;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LanguageService
{
    private static LanguageService _instance;
    public static LanguageService Instance
    {
        get { return _instance ?? (_instance = new LanguageService()); }
    }
    //LocalizationConfig.json内的语言名称List
    public List<String> LanguageNames = new List<String>();
    //文件名list，目前只有Language
    public List<string> Files { get; set; }
    //文件名作为key，LanguageKeyValue为value的字典
    public Dictionary<string, Dictionary<string, string>> GetValueByFile { get; set; }
    //翻译文件的键值对
    public Dictionary<string, string> LanguageKeyValue { get; set; }

    public LanguageService()
    {
        LoadContent();
        //处理加入多语言后，覆盖安装问题
        if (!PlayerPrefs.HasKey("Language"))
        {
            GetSystemLanguage();
            PlayerPrefs.SetInt("Language", CurrentLanguage);
        }
        else
        {
            CurrentLanguage = PlayerPrefs.GetInt("Language", 0);
        }
    }
    private int _currentLanguage;
    private bool isFirstIn = true;
    public int CurrentLanguage
    {
        get
        {
            return _currentLanguage;
        }
        set
        {
            if (value != _currentLanguage || isFirstIn)
            {
                isFirstIn = false;
               _currentLanguage = value;
                PlayerPrefs.SetInt("Language", CurrentLanguage);
                //try
                //{
                    ReadJosnFiles();
                    // TODO:
                    // EventCenter.Instance.OnLanguageChanged?.Invoke();
                //}
                //catch (System.Exception ex) 
                //{
                //    Debug.LogError(ex);
                //}
            }
        }
    }

    public string GetLanguageName()
    {
        return (LanguageType)CurrentLanguage switch
        {
            //AR, HI, IN, TH, TU,
            LanguageType.EN => "en",
            LanguageType.DE => "de",
            LanguageType.FR => "fr",
            LanguageType.ES => "es",
            LanguageType.IT => "it",
            LanguageType.NL => "nl",
            LanguageType.DA => "da",
            LanguageType.PT => "pt",
            LanguageType.PL => "pl",
            LanguageType.SV => "sv",
            LanguageType.CS => "cs",
            LanguageType.HU => "hu",
            LanguageType.JA => "ja",
            LanguageType.FI => "fi",
            LanguageType.NO => "no",
            LanguageType.RU => "ru",
            LanguageType.KO => "ko",
            LanguageType.VI => "vi",
            LanguageType.AR => "ar",
            LanguageType.HI => "hi",
            LanguageType.ID => "id",
            LanguageType.TH => "th",
            LanguageType.TR => "tr",
            LanguageType.UR => "ur",
            _ => "en",
        };
    }
    private void GetSystemLanguage()
    {
        switch (GetSystemLanguageWithAndroid())
        {
            case "Arabic":
                CurrentLanguage = (int)LanguageType.AR; 
                break;
            case "Hindi":
                CurrentLanguage = (int)LanguageType.HI; 
                break;
            case "Urdu":
                CurrentLanguage = (int)LanguageType.UR; 
                break;
            default:
                CurrentLanguage = (int)LanguageType.EN;
                break;
        }

        //if ("Hindi" == GetSystemLanguageWithAndroid())
        //{
        //    CurrentLanguage = (int)LanguageType.HI;
        //}
        //else
        //{
        //    CurrentLanguage = (int)LanguageType.EN;
        //}
        // switch (Application.systemLanguage)
        // {
        //     case SystemLanguage.English:    CurrentLanguage = (int)LanguageType.EN; break;
        //     case SystemLanguage.German:     CurrentLanguage = (int)LanguageType.DE; break;
        //     case SystemLanguage.French:     CurrentLanguage = (int)LanguageType.FR; break;
        //     case SystemLanguage.Spanish:    CurrentLanguage = (int)LanguageType.ES; break;
        //     case SystemLanguage.Italian:    CurrentLanguage = (int)LanguageType.IT; break;
        //     case SystemLanguage.Dutch:      CurrentLanguage = (int)LanguageType.NL; break;
        //     case SystemLanguage.Danish:     CurrentLanguage = (int)LanguageType.DA; break;
        //     case SystemLanguage.Portuguese: CurrentLanguage = (int)LanguageType.PT; break;
        //     case SystemLanguage.Polish:     CurrentLanguage = (int)LanguageType.PL; break;
        //     case SystemLanguage.Swedish:    CurrentLanguage = (int)LanguageType.SV; break;
        //     case SystemLanguage.Czech:      CurrentLanguage = (int)LanguageType.CS; break;
        //     case SystemLanguage.Hungarian:  CurrentLanguage = (int)LanguageType.HU; break;
        //     case SystemLanguage.Japanese:   CurrentLanguage = (int)LanguageType.JA; break;
        //     case SystemLanguage.Finnish:    CurrentLanguage = (int)LanguageType.FI; break;
        //     case SystemLanguage.Norwegian:  CurrentLanguage = (int)LanguageType.NO; break;
        //     case SystemLanguage.Russian:    CurrentLanguage = (int)LanguageType.RU; break;
        //     case SystemLanguage.Korean:     CurrentLanguage = (int)LanguageType.KO; break;
        //     case SystemLanguage.Vietnamese: CurrentLanguage = (int)LanguageType.VI; break;
        //     case SystemLanguage.Arabic:     CurrentLanguage = (int)LanguageType.AR; break;
        //     case SystemLanguage.Indonesian: CurrentLanguage = (int)LanguageType.ID; break;
        //     case SystemLanguage.Thai:       CurrentLanguage = (int)LanguageType.TH; break;
        //     case SystemLanguage.Turkish:    CurrentLanguage = (int)LanguageType.TR; break;
        //     default:
        //     {
        //         if ("Hindi" == GetSystemLanguageWithAndroid())
        //         {
        //             CurrentLanguage = (int)LanguageType.HI;
        //         }
        //         else
        //         {
        //             CurrentLanguage = (int)LanguageType.EN;
        //         }
        //     }
        //     break;
        // }
    }

    //获取Unity SystemLanguage不包含的系统语言类型
    public static string GetSystemLanguageWithAndroid()
    {
        string systemLanguage = "Default";
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass localeClass = new AndroidJavaClass("java/util/Locale");
        AndroidJavaObject defaultLocale = localeClass.CallStatic<AndroidJavaObject>("getDefault");
        AndroidJavaObject usLocale = localeClass.GetStatic<AndroidJavaObject>("US");
        systemLanguage = defaultLocale.Call<string>("getDisplayLanguage", usLocale);
#endif
        return systemLanguage;
    }

    public bool IsCurrentArabic
    {
        get
        {
            return CurrentLanguage == 18;//(int)LanguageType.AR
        }
    }
    public void LoadContent()
    {
        TextAsset config = Resources.Load<TextAsset>("LocalizationConfig");
        var jsonArray = JSONNode.Parse(config.text);
        LoadAllLanguages((JSONClass)jsonArray);
    }

    void LoadAllLanguages(JSONClass jsonClass)
    {
        foreach (KeyValuePair<string, JSONNode> json in jsonClass)
        {
            LanguageNames.Add(json.Value.Value);
        }
    }

    void ReadJosnFiles()
    {
        LanguageKeyValue = new Dictionary<string, string>();
        GetValueByFile = new Dictionary<string, Dictionary<string, string>>();
        Files = new List<string>();

        var path = "Localization/" + LanguageNames[_currentLanguage] + "/";

        var resources = Resources.LoadAll<TextAsset>(path);
        if (!resources.Any())
        {
            Debug.LogError("Localization Files Not Found : " + LanguageNames[_currentLanguage]);
        }
        foreach (var resource in resources)    
        {
            ReadTextAsset(resource);
        }
    }

    // 将TextAsset内容读取到字典中
    void ReadTextAsset(TextAsset resource)
    {
        var jsonArray = JSONNode.Parse(resource.text);
        Files.Add(resource.name);
        GetValueByFile.Add(resource.name, new Dictionary<string, string>());
        foreach (KeyValuePair<string, JSONNode> json in (JSONClass)jsonArray)
        {
            GetValueByFile[resource.name].Add(json.Key, json.Value);
            if (LanguageKeyValue.ContainsKey(json.Key))
                Debug.LogWarning("Duplicate string : " + resource.name + " : " + json.Key);
            else
                LanguageKeyValue.Add(json.Key, json.Value);
        }
    }

    public string GetFromFile(string groupId, string key, string fallback)
    {
        if (!GetValueByFile.ContainsKey(groupId))
        {
            Debug.LogWarning("Localization File Not Found : " + groupId);
            return fallback;
        }
        var group = GetValueByFile[groupId];
        if (!group.ContainsKey(key))
        {
            Debug.LogWarning("Localization Key Not Found : " + key);
            return fallback;
        }
        return group[key];
    }
    //public string GetStringByKey(string key, params object[] formatArgs)
    //{
    //    try
    //    {
    //        return string.Format(LanguageKeyValue[key], formatArgs);
    //    }
    //    catch{
    //        Debug.Log(key);
    //        return "Not Found";
    //    }
    //}


    public string GetStringByKeyText(UnityEngine.UI.Text text, string key, string fallback = "Not Found")
    {
        if (!LanguageKeyValue.ContainsKey(key))
        {
            Debug.LogWarning(string.Format("Localization Key Not Found {0} : {1} ", LanguageNames[_currentLanguage], key));
        }
        else
        {
            fallback = LanguageKeyValue[key];
        }

        return ArabicSupport.ArabicFixer.FixTextForUI(text, fallback);
    }


    //selfuse
    public string GetStringByKey(UnityEngine.UI.Text text, string key, string fallback = "Not Found", params object[] formatArgs)
    {
        if (!LanguageKeyValue.ContainsKey(key))
        {
            Debug.LogWarning(string.Format("Localization Key Not Found {0} : {1} ", LanguageNames[_currentLanguage], key));
        } 
        else
        {
            fallback = LanguageKeyValue[key];
        }  

        fallback = string.Format(fallback, formatArgs);
        if (CurrentLanguage == (int)LanguageType.AR) 
        { 
            return ArabicSupport.ArabicFixer.FixTextForUI(text,fallback);
        }

        if ((CurrentLanguage == (int)LanguageType.UR))
        {
            
            return ArabicSupport.ArabicFixer.FixTextForUI(text,fallback);
        }

        return fallback;
    }
}
public enum LanguageType
{
    EN = 0, DE, FR, ES, IT, NL, DA, PT, PL, SV, CS, HU, JA, FI, NO, RU, KO, VI, AR, HI, ID, TH, TR,UR
}
