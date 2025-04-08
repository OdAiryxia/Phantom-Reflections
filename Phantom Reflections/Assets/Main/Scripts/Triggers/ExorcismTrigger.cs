using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExorcismTrigger : ButtonBaseFunction
{
    [SerializeField] private ExorcismQuestion[] question;

    protected override void OnMouseDown()
    {
        if (!ProgressManager.instance.buttonInteruption)
        {
            ExorcismManager.instance.questions = question;
            ExorcismManager.instance.SetQuestion();
        }

        base.OnMouseDown();
    }
}
