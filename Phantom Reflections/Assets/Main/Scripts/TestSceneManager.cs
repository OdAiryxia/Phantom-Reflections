using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviour
{
    FlowerSystem flowerSys;

    void Awake()
    {
        flowerSys = FlowerManager.Instance.CreateFlowerSystem("TestScene", true);
        flowerSys.textSpeed = 0.05f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) | Input.GetMouseButtonDown(0))
        {
            // Continue the messages, stoping by [w] or [lr] keywords.
            flowerSys.Next();
        }
    }
}
