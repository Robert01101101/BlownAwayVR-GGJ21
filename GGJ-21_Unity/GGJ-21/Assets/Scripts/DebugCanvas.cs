using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugCanvas : MonoBehaviour
{
    //Singleton instance
    public static DebugCanvas instance;

    private static TextMeshProUGUI canvasText;

    //Singleton pattern - only one instance that is accessible from anywhere though ExhaleInput.instance
    //from: https://riptutorial.com/unity3d/example/14518/a-simple-singleton-monobehaviour-in-unity-csharp
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else { Destroy(this); }
    }

    private void Start()
    {
        canvasText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public static void DebugLog(string message)
    {
        //canvasText.text = message;
    }
}
