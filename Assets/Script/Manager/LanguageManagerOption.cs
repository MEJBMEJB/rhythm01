using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.IO;

/*
 옵션 Scene에서 언어설정을 결정한다
 
 */
public class LanguageManagerOption : MonoBehaviour
{
    private static LanguageManagerOption _instance;
    public static LanguageManagerOption Instance
    {
        get => _instance;
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }
    void Start()
    {
        setLocal();
    }

    public void setLocal(int idx = -1)
    {
        OptionValueToJson data = new OptionValueToJson();
        //선택언어 결정
        string strValue = File.ReadAllText(data.jsonFilePath);
        data = JsonUtility.FromJson<OptionValueToJson>(strValue);

        Debug.Log($"setLocal idx = {idx}, data.language = {data.language}");

        if(idx == -1) 
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[data.language];
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[idx];
        }
    }

    public string getText(string tableName, string strKey)
    {
        string strReturnValue = "";
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        strReturnValue = LocalizationSettings.StringDatabase.GetLocalizedString(tableName, strKey, currentLanguage);
        Debug.Log($"ReturnValue = {strReturnValue}");
        return strReturnValue;
    }
}
