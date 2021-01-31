using System.Collections;
using System.Collections.Generic;
using Blowing;
using UnityEngine;

namespace PaperboatIntroSceneScripts
{
    public class StartPaperPlaneGameOnBlown : MonoBehaviour
    {
        [SerializeField]
        private Animator animator = default;

        [SerializeField] 
        private float delayBeforeTransition = 5;
        
        private bool usedUp;
        private Coroutine startGameCoroutine;

        private IEnumerator StartGame()
        {
            animator.enabled = true;
            animator.Play(0);
            yield return new WaitForSeconds(delayBeforeTransition);
            
            SceneLoader.instance.LoadNextScene();
            yield return null;
        }

        void Update()
        {
            AffectedByBlow affectedByBlow = GetComponent<AffectedByBlow>();
            if (affectedByBlow == null)
            {
                return;
            }

            if (usedUp)
            {
                return;
            }
            
            usedUp = true;
            startGameCoroutine = StartCoroutine(StartGame());
        }
    }
}
