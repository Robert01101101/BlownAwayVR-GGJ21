using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* [ExhaleInput Class]
 * Allows universal easy access to mic values. Use "isExhaling" and "strength".
 * Currently this is using the OVRLipSync approach. Code is also added for reading mic values manually, although those values aren't mapped or calibrized yet.
 */
public class ExhaleInput : MonoBehaviour
{
    //Singleton instance
    public static ExhaleInput instance;

    public static bool isExhaling;
    public static float strength;
    private static float minValue, maxValue;

    public OVRLipSyncContext oVRLipSyncContext;

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
        minValue = 0;
        maxValue = 1;

        //from https://stackoverflow.com/questions/60913254/microphone-permission-on-oculus-quest
        Debug.Log(Microphone.devices.Length);
        Debug.Log(Application.internetReachability.ToString());


        string micList = "";
        string firstMicName = "Built-in Microphone";
        foreach (var device in Microphone.devices)
        {
            micList += "Mic name: " + device + "\n";
            if (firstMicName == "Built-in Microphone") firstMicName = device;
        }
        DebugCanvas.DebugLog(micList);

        /*
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(firstMicName, true, 10, 44100);
        audioSource.Play();*/

        //_clipRecord = GetComponent<AudioSource>().clip;

    }

    private void Update()
    {
        strength = oVRLipSyncContext.GetMicVolume();
        isExhaling = strength > 0;

        DebugCanvas.DebugLog("isExhaling: " + isExhaling + "\n strength: " + strength);
    }

    public static void SetMin(float minVal)
    {

    }

    public static void SetMax(float minVal)
    {

    }



    //Not using for now, but this might allow us to read all sounds, as well as maybe access the headphone mic
    //from: https://forum.unity.com/threads/check-current-microphone-input-volume.133501/
    /*
    public static float MicLoudness;
    AudioClip _clipRecord;
    private string _device;

    //mic initialization
    void InitMic()
    {
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
    }

    void StopMicrophone()
    {
        Microphone.End(_device);
    }


    
    int _sampleWindow = 128;

    //get data from microphone into audioclip
    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }



    void Update()
    {
        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // pass the value to a static var so we can access it from anywhere
        MicLoudness = LevelMax();

        float db = 20 * Mathf.Log10(Mathf.Abs(MicLoudness));
        DebugCanvas.DebugLog(db.ToString());
    }

    bool _isInitialized;
    // start mic when scene starts
    void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

    //stop mic when loading a new level or quit application
    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }


    // make sure the mic gets started & stopped when application gets focused
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            //Debug.Log("Focus");

            if (!_isInitialized)
            {
                //Debug.Log("Init Mic");
                InitMic();
                _isInitialized = true;
            }
        }
        if (!focus)
        {
            //Debug.Log("Pause");
            StopMicrophone();
            //Debug.Log("Stop Mic");
            _isInitialized = false;

        }
    }*/

}
