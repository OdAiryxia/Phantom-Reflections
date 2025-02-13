using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCluePickup : ButtonBaseFunction
{
    FlowerSystem flowerSys;

    [Header("彈出視窗")]
    [SerializeField] private bool isModalWindowTrigger = true;
    [SerializeField] private ModalWindowTemplate modalWindowTemplates;
    [Header("線索")]
    public ClueData clue;
    [Header("是否發生場景改變")]
    [SerializeField] private bool sceneChange = false;
    [SerializeField] private bool destoryItself = false;
    [SerializeField] private List<GameObject> imagesToShow;
    [Header("故事")]
    [SerializeField] private string story;

    bool isPicked = false;

    protected override void Start()
    {
        base.Start();

        flowerSys = FlowerManager.Instance.GetFlowerSystem("TestScene");

        foreach (var image in imagesToShow)
        {
            if (image != null)
            {
                image.SetActive(false);
            }
        }
    }

    protected override void OnMouseDown()
    {
        if (!TestSceneManager.instance.buttonInteruption)
        {
            if (isModalWindowTrigger)
            {
                ShowCurrentModal();
            }

            if (!isPicked)
            {
                if (clue != null)
                {
                    InventoryManager.instance.Add(clue);
                    isPicked = true;
                }

                if (sceneChange)
                {
                    foreach (var image in imagesToShow)
                    {
                        if (image != null)
                        {
                            image.SetActive(true);
                        }
                    }
                    if (destoryItself)
                    {
                        Destroy(gameObject);
                    }
                }
            }

            if (!string.IsNullOrEmpty(story) && !isModalWindowTrigger)
            {
                flowerSys.ReadTextFromResource(story);
            }
        }

        base.OnMouseDown();
    }

    void ShowCurrentModal()
    {
        ModalWindowTemplate currentTemplate = modalWindowTemplates;

        ModalWindowManager.instance.ShowVertical(
            currentTemplate.title,
            currentTemplate.image,
            currentTemplate.context,
            currentTemplate.confirmText, () =>
            {
                ModalWindowManager.instance.Close();

                if (!string.IsNullOrEmpty(story))
                {
                    flowerSys.ReadTextFromResource(story);
                }
            },
            currentTemplate.declineText, () =>
            {
                ModalWindowManager.instance.Close();
            },
            currentTemplate.alternateText, () =>
            {
                ModalWindowManager.instance.Close();
            }
        );
    }
}


