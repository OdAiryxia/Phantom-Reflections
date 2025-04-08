using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExorcismFailOverlay : MonoBehaviour
{
    public static ExorcismFailOverlay instance;

    [SerializeField] private List<Sprite> stage_one;
    [SerializeField] private List<Sprite> stage_two;
    [SerializeField] private List<Sprite> stage_three;

    [SerializeField] private Image stage_one_image;
    [SerializeField] private Image stage_two_image;
    [SerializeField] private Image stage_three_image;


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
    }
}
