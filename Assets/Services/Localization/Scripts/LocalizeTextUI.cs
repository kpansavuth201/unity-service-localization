using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LocalizeTextUI : MonoBehaviour
{
    public string Key;
    
    private Text _textLabel;
    
    void Awake() {
        _textLabel = this.gameObject.GetComponent<Text>();
    }

    void Start() {
        LocalizationManager.Instance.RegisterLocalizeTextUI(this);
    }

    public void ApplyLanguage(LocalizationManager.Language language) {
        if(string.IsNullOrEmpty(Key)) {
            Debug.LogError(this.gameObject.name + " language key is null");
            return;
        }

        string pharse = LocalizationManager.Instance.GetPharse(language, Key);

        _textLabel.text = pharse;
    }
    
    void OnDestroy() {
        LocalizationManager.Instance.UnRegisterLocalizeTextUI(this);
    }
 }
