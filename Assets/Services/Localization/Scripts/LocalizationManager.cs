using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Defective.JSON;

public class LocalizationManager {

    public enum Language {
        EN,
        TH,
    }

    public delegate void CallbackWithLanguage(Language language);

    public CallbackWithLanguage OnLanguageChanged;

    private Dictionary<Language, Dictionary<string, string>> _pharseDict = new Dictionary<Language, Dictionary<string, string>>();

    private List<LocalizeTextUI> _localizeTextUIList = new List<LocalizeTextUI>();

    public void RegisterLocalizeTextUI(LocalizeTextUI item) {
        if(_localizeTextUIList.Contains(item))
            return;

        _localizeTextUIList.Add(item);
        item.ApplyLanguage(_currentLanguage);
    }

    public void UnRegisterLocalizeTextUI(LocalizeTextUI item) {
        if(!_localizeTextUIList.Contains(item))
            return;

        _localizeTextUIList.Remove(item);
    }

    private readonly string LANGUAGE_KEY = "LOCALIZATION_LANGUAGE";

    private readonly string LANGUAGE_JSON_PATH = "Data/languageLocalization";

    private const Language DEFAULT_LANGUAGE = Language.EN;

    private readonly Language []AVAILABLE_LANGUAGES = new Language[] {
        Language.EN,
        Language.TH
    };

    private Language _currentLanguage = DEFAULT_LANGUAGE;

    public void SetCurrentLanguage(Language language) {
        _currentLanguage = language;
        ApplyLocalization(language);
        Save(language);
    }

    public Language GetCurrentLanguage() {
        return _currentLanguage;
    }

    public string GetPharse(Language language, string key) {
        if(string.IsNullOrEmpty(key)) {
            Debug.LogError("key is null");
            return "";
        }

        if(!_pharseDict.ContainsKey(language)) {
            Debug.LogError("Language " + language.ToString() + " dict is null");
            return "";
        }

        if(!_pharseDict[language].ContainsKey(key)) {
            Debug.LogError("Key " + key + " is null");
            return "";
        }

        return _pharseDict[language][key];
    }

    private void ApplyLocalization(Language language) {
        for(int i = 0; i < _localizeTextUIList.Count; ++i) {
            if(_localizeTextUIList[i] == null) {
                _localizeTextUIList.RemoveAt(i);
                --i;
                continue;
            }

            _localizeTextUIList[i].ApplyLanguage(_currentLanguage);
        }

        OnLanguageChanged?.Invoke(_currentLanguage);
    }

    private void Save(Language language) {
        PlayerPrefs.SetString(LANGUAGE_KEY, language.ToString());
    }

    private Language Load() {
        return (Language) Enum.Parse(
            typeof(Language),
            PlayerPrefs.GetString(LANGUAGE_KEY, DEFAULT_LANGUAGE.ToString())
        );
    }

    private void Initialize() {
        // Dictionary<string, string> TH_DICT = new Dictionary<string, string>();
        // TH_DICT.Add("TEST", "ทดสอบจ้า");

        // Dictionary<string, string> EN_DICT = new Dictionary<string, string>();
        // EN_DICT.Add("TEST", "Test Ja");

        // _pharseDict.Clear();
        // _pharseDict.Add(Language.EN, EN_DICT);
        // _pharseDict.Add(Language.TH, TH_DICT);

        _pharseDict.Clear();

        for(int i = 0; i < AVAILABLE_LANGUAGES.Length; ++i) {
            _pharseDict.Add(AVAILABLE_LANGUAGES[i], new Dictionary<string, string>());
        }
        
        TextAsset jsonFile = Resources.Load<TextAsset>(LANGUAGE_JSON_PATH);
        JSONObject json = new JSONObject(jsonFile.text);

        foreach (JSONObject elem in json.list) {
            string key = elem["key"].stringValue;
            for(int i = 0; i < AVAILABLE_LANGUAGES.Length; ++i) {
                if(_pharseDict[AVAILABLE_LANGUAGES[i]].ContainsKey(key))
                    continue;

                _pharseDict[AVAILABLE_LANGUAGES[i]].Add(key, elem["value"][ AVAILABLE_LANGUAGES[i].ToString() ].stringValue);
            }
        }
    }

    #region SINGLETON
    private static LocalizationManager _instance = null;
    
    //The thread takes out a lock on a shared object, and then checks whether or not the instance has been created before creating the instance. 
    //This takes care of the memory barrier issue
    private static readonly object _padlock = new object();

    LocalizationManager() {        
        Initialize();
        
        SetCurrentLanguage(Load());
    }
        
        //Singleton Design Pattern
    public static LocalizationManager Instance {
        get {
            lock (_padlock) {
                if (_instance == null) {
                    _instance = new LocalizationManager();
                }
                return _instance;
            }
        }
    }
    #endregion
}
