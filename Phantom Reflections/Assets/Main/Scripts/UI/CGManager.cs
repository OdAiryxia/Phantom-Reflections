using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGManager : MonoBehaviour
{
    public static CGManager instance;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform CgParent;
    public Button closeButton;

    public GameObject currentCg;
    public List<Button> cgButtons;

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
        currentCg = cg;

        closeButton.gameObject.SetActive(true);

        cgButtons = new List<Button>();
        foreach (Button button in currentCg.GetComponentsInChildren<Button>())
        {
            cgButtons.Add(button);
        }

        currentCg.SetActive(true);
        TestSceneManager.instance.buttonInteruption = true;
    }

    public void CloseCG()
    {
        onCg = false;

        closeButton.gameObject.SetActive(false);

        currentCg.SetActive(false);
        TestSceneManager.instance.buttonInteruption = false;
        currentCg = null;
        cgButtons.Clear();
    }
}
