using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ChangeLanguageController : MonoBehaviour
{
    public Button ButtonTH;
    public Button ButtonEN;

    void Start() {
        ButtonTH.onClick.AddListener(() => {
            LocalizationManager.Instance.SetCurrentLanguage(LocalizationManager.Language.TH);
        });
        ButtonEN.onClick.AddListener(() => {
            LocalizationManager.Instance.SetCurrentLanguage(LocalizationManager.Language.EN);
        });
    }
}
