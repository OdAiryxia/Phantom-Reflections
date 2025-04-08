using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;

    public bool isMenu = false;

    public bool loadMenuOnStart;

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
        DontDestroyOnLoad(gameObject);

        if (loadMenuOnStart)
        {
            SceneManager.LoadSceneAsync((int)SceneIndexes.TitleScreen, LoadSceneMode.Additive);
            isMenu = true;
        }
    }

    //private void Start()
    //{
    //    if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Empty).isLoaded)
    //    {
    //        SceneManager.UnloadSceneAsync((int)SceneIndexes.Empty);
    //    }
    //}

    public void LoadMenu()
    {
        isMenu = true;

        SceneManager.UnloadSceneAsync((int)SceneIndexes.SampleScene);
        SceneManager.UnloadSceneAsync((int)SceneIndexes.UI);
        SceneManager.LoadSceneAsync((int)SceneIndexes.TitleScreen, LoadSceneMode.Additive);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.TitleScreen));
    }

    public void LoadScene()
    {
        isMenu = false;

        SceneManager.UnloadSceneAsync((int)SceneIndexes.TitleScreen);
        SceneManager.LoadSceneAsync((int)SceneIndexes.UI, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync((int)SceneIndexes.SampleScene, LoadSceneMode.Additive);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.SampleScene));
    }
}
