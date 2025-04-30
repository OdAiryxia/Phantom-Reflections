using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class ButtonCluePickup : ButtonBaseFunction
{
    FlowerSystem flowerSys;

    [Header("視窗")]
    [SerializeField] private bool 觸發彈出視窗;
    [SerializeField] private ModalWindowTemplate 彈出視窗;
    [Header("線索")]
    public ClueData 線索;
    bool isPicked = false;
    [Header("場景")]
    [SerializeField] private bool 觸發場景改變;
    [SerializeField] private bool 摧毀此物件;
    [SerializeField] private List<GameObject> 摧毀其他物件;
    [SerializeField] private List<GameObject> 顯示其他物件;
    [Header("故事")]
    [SerializeField] private string 故事;
    [SerializeField] private bool 再次播放;
    private bool storyPlayed = false;
    [Header("除靈")]
    [SerializeField] private bool 觸發除靈;
    [SerializeField] private ExorcismQuestion[] 除靈題目;
    [SerializeField] private UnityEvent events;

    protected override void Start()
    {
        base.Start();

        flowerSys = FlowerManager.Instance.GetFlowerSystem("FlowerSystem");

        foreach (var image in 顯示其他物件)
        {
            if (image != null)
            {
                image.SetActive(false);
            }
        }
    }

    protected override void OnMouseDown()
    {
        if (!ProgressManager.instance.buttonInteruption)
        {
            if (觸發彈出視窗)
            {
                ShowCurrentModal();
            }

            if (this.GetComponent<AudioSource>() != null)
            {
                AudioManager.instance.audioSource.resource = gameObject.GetComponent<AudioSource>().resource;
            }

            if (!isPicked)
            {
                if (線索 != null)
                {
                    InventoryManager.instance.Add(線索);
                    isPicked = true;
                }

                if (觸發場景改變)
                {
                    foreach (var image in 摧毀其他物件)
                    {
                        if (image != null)
                        {
                            image.SetActive(false);
                        }
                    }
                    foreach (var image in 顯示其他物件)
                    {
                        if (image != null)
                        {
                            image.SetActive(true);
                        }
                    }
                    if (摧毀此物件)
                    {
                        Destroy(gameObject);
                    }
                }
            }

            if (!string.IsNullOrEmpty(故事) && !觸發彈出視窗)
            {
                if (!storyPlayed || 再次播放)
                {
                    flowerSys.ReadTextFromResource(故事);
                    storyPlayed = true;
                }
            }

            if (觸發除靈)
            {
                ProgressManager.instance.setQuestionsAfterStory = true;
                ExorcismManager.instance.questions = 除靈題目;
                if (string.IsNullOrEmpty(故事) && !觸發彈出視窗)
                {
                    ExorcismManager.instance.SetQuestion();
                    if (events != null)
                    {
                        ExorcismManager.instance.events = events;
                    }
                }
            }
        }

        base.OnMouseDown();
    }

    void ShowCurrentModal()
    {
        ModalWindowTemplate currentTemplate = 彈出視窗;

        ModalWindowManager.instance.ShowVertical(
            currentTemplate.title,
            currentTemplate.image,
            currentTemplate.context,
            currentTemplate.confirmText, () =>
            {
                ModalWindowManager.instance.Close();

                if (AudioManager.instance.audioSource.resource != null)
                {
                    AudioManager.instance.Play();
                }

                if (!string.IsNullOrEmpty(故事) && (!storyPlayed || 再次播放))
                {
                    flowerSys.ReadTextFromResource(故事);
                    storyPlayed = true;
                }

                if (觸發除靈 && string.IsNullOrEmpty(故事))
                {
                    ExorcismManager.instance.SetQuestion();
                    if (events != null)
                    {
                        ExorcismManager.instance.events = events;
                    }
                }
            },
            currentTemplate.declineText, () =>
            {
                ModalWindowManager.instance.Close();

                if (AudioManager.instance.audioSource.resource != null)
                {
                    AudioManager.instance.Play();
                }

                if (!string.IsNullOrEmpty(故事) && (!storyPlayed || 再次播放))
                {
                    flowerSys.ReadTextFromResource(故事);
                    storyPlayed = true;
                }

                if (觸發除靈 && string.IsNullOrEmpty(故事))
                {
                    ExorcismManager.instance.SetQuestion();
                    if (events != null)
                    {
                        ExorcismManager.instance.events = events;
                    }
                }
            },
            currentTemplate.alternateText, () =>
            {
                ModalWindowManager.instance.Close();

                if (AudioManager.instance.audioSource.resource != null)
                {
                    AudioManager.instance.Play();
                }

                if (!string.IsNullOrEmpty(故事) && (!storyPlayed || 再次播放))
                {
                    flowerSys.ReadTextFromResource(故事);
                    storyPlayed = true;
                }

                if (觸發除靈 && string.IsNullOrEmpty(故事))
                {
                    ExorcismManager.instance.SetQuestion();
                    if (events != null)
                    {
                        ExorcismManager.instance.events = events;
                    }
                }
            }
        );
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // 畫出白色線條連接到顯示其他物件
        Gizmos.color = Color.white;
        if (顯示其他物件 != null)
        {
            foreach (var obj in 顯示其他物件)
            {
                if (obj != null)
                {
                    Gizmos.DrawLine(transform.position, obj.transform.position);
                    UnityEditor.Handles.Label(obj.transform.position + Vector3.up * 0.3f, obj.name);
                }
            }
        }

        // 畫出紅色線條連接到摧毀其他物件
        Gizmos.color = Color.red;
        if (摧毀其他物件 != null)
        {
            foreach (var obj in 摧毀其他物件)
            {
                if (obj != null)
                {
                    Gizmos.DrawLine(transform.position, obj.transform.position);
                    UnityEditor.Handles.Label(obj.transform.position + Vector3.up * 0.3f, obj.name);
                }
            }
        }
        // 顯示文字標籤
        Vector3 labelOffset = new Vector3(3f, 1f, 0); // 在物件旁邊右上方一點
        Vector3 labelPos = transform.position + labelOffset;

        string labelText = "";

        // 視窗
        labelText += $"彈出視窗：{(觸發彈出視窗 ? "是" : "否")}\n";

        // 線索
        labelText += $"線索蒐集：{(線索 != null ? "是" : "否")}\n";

        // 故事
        if (!string.IsNullOrEmpty(故事))
        {
            string storyLabel = 觸發彈出視窗 ? "彈出視窗後觸發故事" : "直接觸發故事";
            labelText += $"觸發故事：{storyLabel}（{故事}）\n";
        }
        else
        {
            labelText += "觸發故事：否\n";
        }

        // 除靈
        labelText += $"觸發除靈：{(觸發除靈 ? $"是（{除靈題目?.Length ?? 0}題）" : "否")}";

        labelText += $"\n摧毀自己：{(摧毀此物件 ? "是" : "否")}";
        Handles.Label(labelPos, labelText);
    }
#endif
}