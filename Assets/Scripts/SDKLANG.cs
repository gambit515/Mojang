using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

public class SDKLANG : MonoBehaviour
{
    public Text _textLanguage;
    public Text Languages;
    public Text deviceText;


    // Создание SINGLETON
    private static SDKLANG _instance;
    
    public static SDKLANG Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<SDKLANG>();

            return _instance;
        }
    }
    public static bool IsMobileDevice = false;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {

    }



    public void GettingLang(string lang)
    {
        Debug.Log("ВЫВОД JSON языка");
        print(lang);
        SetLang(lang);
    }

    private void SetLang(string _lang)
    {
        var json = JSON.Parse(_lang);
        string lang = json["browser"]["lang"];
        _textLanguage.text = lang;
        Languages.text = "Error";
        Debug.Log("Текущий язык"+lang);
        if (lang == "ru")
            Languages.text = "Русский";
        else if (lang == "en")
            Languages.text = "English";
    }

    public void GettingDevice(string _device)
    {
        deviceText.text = _device;
        switch(_device)
        {
            case "\"desktop\"":
                IsMobileDevice = false;
                break;
            case "\"mobile\"":
                IsMobileDevice = true;
                break;
            default:
                Debug.Log("Ошибка, неопощнанный тип устройства: " + _device);
                break;
        }
    }
}
