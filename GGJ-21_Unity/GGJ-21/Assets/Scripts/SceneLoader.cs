using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//
// MUST BE ATTACHED TO PLAYERCTRL
//

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance = null;
    public OVRScreenFade screenFade;
    private string lastSceneName;

    //// BUILD INDEXES ////
    public readonly static int _BASE = 0;
    public readonly static int PAPERBOAT = 1;
    public readonly static int DESERTYACHT = 2;
    public readonly static int BALLOON = 3;
    public readonly static int ROCKET = 4;

    //Scenes
    [System.NonSerialized]
    public int currentSceneIdx;
    public string sceneToLoad;
    private bool sceneReloading = false;

    //booleans
    [System.NonSerialized]
    public bool paused;

    Transform vehicle;


    ///////////////////////////////////////////////////////////// AWAKE () //////////////////////////////////////////////////////////

    private void Awake()
    {
        if (instance == null) { instance = this; } else if (instance != this) { Destroy(gameObject); } //Singleton Pattern
    }

    void Start()
    {
        //Set up stuff for Pausing
        Time.timeScale = 1;
        paused = false;

        //Set curScene to be active Scene (_Base)
        currentSceneIdx = SceneManager.GetSceneByName(sceneToLoad).buildIndex;
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);

        //Listen for Scene Loads & Unloads
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        SceneManager.sceneUnloaded += OnSceneFinishedUnloading;
    }

    /////////////////////////////////////////////////////////////// OnSceneFinishedLoading () /////////////////////////////////////////
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentSceneIdx = scene.buildIndex;
        Debug.Log("Game Manager -> Scene Loaded: idx=" + currentSceneIdx + ", name=" + scene.name + ", loadMode=" + mode);

        //Position player & begin fade
        Teleport(GameObject.Find("SpawnPoint").transform.position, false);
        screenFade.FadeIn();

        //check for vehicle
        VehicleFlag vf = GameObject.FindObjectOfType<VehicleFlag>();
        if (vf != null)
        {
            vehicle = vf.transform;
            transform.parent = vehicle;
        }
    }

    public void Teleport(Vector3 target, bool fade=true)
    {
        if (fade)
        {
            StartCoroutine(TeleportFadeRoutine(target));
        } else
        {
            transform.position = target;
        }
    }

    IEnumerator TeleportFadeRoutine(Vector3 target)
    {
        screenFade.FadeOut();
        yield return new WaitForSeconds(2);
        transform.position = target;
        yield return new WaitForSeconds(.2f);
        screenFade.FadeIn();
    }

    /////////////////////////////////////////////////////////////// OnSceneFinishedUnloading () /////////////////////////////////////////
    void OnSceneFinishedUnloading(Scene scene)
    {
        if (sceneReloading)
        {
            sceneReloading = false;
            LoadScene(currentSceneIdx);
        }
    }



    /////////////////////////////////////////////////////////////////////////////////////////// LoadScene (newSceneToLoad) ####################
    public void LoadScene(int newSceneToLoad)
    {
        DetachPlayerFromVehicle();
        SceneManager.UnloadSceneAsync(currentSceneIdx);
        SceneManager.LoadScene(newSceneToLoad, LoadSceneMode.Additive);
    }

    private bool loadingNextScene = false;
    public void LoadNextScene()
    {
        if (!loadingNextScene)
        {
            DetachPlayerFromVehicle();
            loadingNextScene = true;
            StartCoroutine(LoadNextSceneRoutine());
        }
    }

    IEnumerator LoadNextSceneRoutine ()
    {
        screenFade.FadeOut();
        yield return new WaitForSeconds(2);
        SceneManager.UnloadSceneAsync(currentSceneIdx);
        currentSceneIdx++;
        SceneManager.LoadScene(currentSceneIdx, LoadSceneMode.Additive);
        loadingNextScene = false;
    }

    public void ReloadLevel()
    {
        DetachPlayerFromVehicle();
        sceneReloading = true;
        SceneManager.UnloadSceneAsync(currentSceneIdx);
        Debug.Log("GameManager -> Reload Level");
    }
    
    private void DetachPlayerFromVehicle()
    {
        if (vehicle != null)
        {
            transform.parent = null;
            Destroy(vehicle.gameObject);
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByBuildIndex(_BASE));
            vehicle = null;
        }
    }
}
