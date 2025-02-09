using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExorcismTrigger : ButtonBaseFunction
{
    [SerializeField] private ExorcismQuestion[] question;

    protected override void OnMouseDown()
    {
        if (!TestSceneManager.instance.buttonInteruption)
        {
            ExorcismManager.instance.SetQuestion(question);
        }

        base.OnMouseDown();
    }
}
