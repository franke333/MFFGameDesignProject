using CharlesEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndSceneDramaticScript : MonoBehaviour
{
    public SwitchToScene switchToScene;
    public UnityEvent onSceneSwitch;

    [Range(0, 20)]
    [Tooltip("Delay in seconds from calling Run method before switching to the next scene")]
    public float delayOfSwitch = 1f;

    public void Run()
    {
        StartCoroutine(SwitchScene());
    }

    private IEnumerator SwitchScene()
    {
        yield return Globals.Fade.In(FadeManager.FadeType.SceneSwitch);
        onSceneSwitch.Invoke();
        Debug.Log("Switching scene in " + delayOfSwitch + " seconds");
        yield return new WaitForSeconds(delayOfSwitch);
        switchToScene.Run();
    }
}