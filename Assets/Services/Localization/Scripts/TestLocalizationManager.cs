using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLocalizationManager : MonoBehaviour
{
    public LocalizationManager.Language Language;
    
    void Start()
    {
        LocalizationManager.Instance.SetCurrentLanguage(Language);

        Debug.Log(
            LocalizationManager.Instance.GetPharse(
                LocalizationManager.Instance.GetCurrentLanguage(),
                "PICK_AVATAR"
            )
        );
    }
}
