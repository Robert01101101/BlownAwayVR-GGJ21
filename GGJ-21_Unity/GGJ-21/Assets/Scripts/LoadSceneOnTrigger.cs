using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour
{
    [SerializeField]
    private string sceneName = default;

    private bool triggered;
    
    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
        {
            return;
        }
        
        SceneLoader.instance.LoadScene(SceneManager.GetSceneByName(sceneName).buildIndex);
        triggered = true;
    }
}
