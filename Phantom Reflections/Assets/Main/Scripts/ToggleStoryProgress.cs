using Flower;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleStoryProgress : MonoBehaviour
{
    private FlowerSystem flowerSys;

    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    private bool isAuto;

    void Start()
    {
        flowerSys = FlowerManager.Instance.GetFlowerSystem("FlowerSystem");
        flowerSys.processMode = FlowerSystem.ProcessModeType.Normal;
        isAuto = false;
    }

    public void Toggle()
    {
        if (isAuto)
        {
            flowerSys.processMode = FlowerSystem.ProcessModeType.Normal;
            text.color = Color.gray;
            isAuto = false;
        }
        else if (!isAuto)
        {
            flowerSys.processMode = FlowerSystem.ProcessModeType.Auto;
            text.color = Color.white;
            isAuto = true;
        }
    }
}
