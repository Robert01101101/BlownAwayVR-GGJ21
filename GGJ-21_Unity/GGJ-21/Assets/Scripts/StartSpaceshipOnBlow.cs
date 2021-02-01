using System.Collections;
using System.Collections.Generic;
using Blowing;
using UnityEngine;

public class StartSpaceshipOnBlow : MonoBehaviour
{
    [SerializeField]
    private Animator animator = default;

    [SerializeField]
    private float delayBeforeTransition = 5;

    private bool usedUp;
    private Coroutine startGameCoroutine;

    public GameObject particles;

    private bool ready = false;

    private void Start()
    {
        StartCoroutine(StartDelay());
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(2);
        ready = true;
    }

    private IEnumerator StartGame()
    {
        animator.enabled = true;
        particles.SetActive(true);
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

        if (usedUp || !ready)
        {
            return;
        }

        usedUp = true;
        startGameCoroutine = StartCoroutine(StartGame());
    }
}
