using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //// This Script will be used to keep everything together. ////

    public static SceneLoader instance = null;
    public OVRScreenFade screenFade;
    private string lastSceneName;

    //// BUILD INDEXES ////
    public readonly static int _BASE = 0;
    public readonly static int PAPERBOAT_INTRO = 1;
    public readonly static int PAPERBOAT_GAME = 2;
    public readonly static int DESERTYACHT_INTRO = 3;
    public readonly static int DESERTYACHT_GAME = 4;
    public readonly static int BALLOON_INTRO = 5;
    public readonly static int BALLOON_GAME = 6;
    public readonly static int ROCKET_INTRO = 7;
    public readonly static int ROCKET_GAME = 8;

    //Scenes
    [System.NonSerialized]
    public int currentSceneIdx = PAPERBOAT_INTRO; //<--- Set first Scene to load
    private Scene curScene;
    private bool sceneReloading = false;

    //booleans
    public bool paused;


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
        curScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentSceneIdx, LoadSceneMode.Additive);

        //Listen for Scene Loads & Unloads
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        SceneManager.sceneUnloaded += OnSceneFinishedUnloading;
    }

    /////////////////////////////////////////////////////////////// OnSceneFinishedLoading () /////////////////////////////////////////
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentSceneIdx = scene.buildIndex;
        Debug.Log("Game Manager -> Scene Loaded: idx=" + currentSceneIdx + ", name=" + scene.name + ", loadMode=" + mode);

        Teleport(GameObject.Find("SpawnPoint").transform.position);
        screenFade.FadeIn();
    }

    private void Teleport(Vector3 target)
    {
        transform.position = target;
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
        SceneManager.UnloadSceneAsync(currentSceneIdx);
        SceneManager.LoadScene(newSceneToLoad, LoadSceneMode.Additive);
    }

    private bool loadingNextScene = false;
    public void LoadNextScene()
    {
        if (!loadingNextScene)
        {
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
        sceneReloading = true;
        SceneManager.UnloadSceneAsync(currentSceneIdx);
        Debug.Log("GameManager -> Reload Level");
    }
}
