using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CGManager : MonoBehaviour
{
    public static CGManager instance;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform CgParent;
    public Button closeButton;

    [SerializeField] private GameObject currentCg;

    [HideInInspector] public bool onCg = false;
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

        canvas.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);

        foreach (Transform child in CgParent)
        {
            child.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        closeButton.onClick.AddListener(CloseCG);
    }

    public void OpenCG(GameObject cg)
    {
        onCg = true;

        closeButton.gameObject.SetActive(true);

        currentCg = cg;
        currentCg.SetActive(true);
        TestSceneManager.instance.buttonInteruption = true;
    }

    public void CloseCG()
    {
        onCg = false;

        closeButton.gameObject.SetActive(false);

        currentCg.SetActive(false);
        TestSceneManager.instance.buttonInteruption = false;
    }
}
