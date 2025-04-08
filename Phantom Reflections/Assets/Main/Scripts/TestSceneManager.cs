using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TestSceneManager : MonoBehaviour
{
    public static TestSceneManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DebugTester();
    }

    [Header("Debug")]
    [SerializeField] bool UI;
    [SerializeField] bool eventSystem;

    void DebugTester()
    {
        if (UI)
        {
            if (!SceneManager.GetSceneByBuildIndex((int)SceneIndexes.UI).isLoaded)
            {
                SceneManager.LoadSceneAsync((int)SceneIndexes.UI, LoadSceneMode.Additive);
            }
        }
        if (eventSystem)
        {
            GameObject obj = new GameObject("EventSystem");
            obj.AddComponent<EventSystem>();
            obj.AddComponent<StandaloneInputModule>();
        }
    }
}
