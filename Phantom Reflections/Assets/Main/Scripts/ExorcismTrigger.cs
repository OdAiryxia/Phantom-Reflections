using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExorcismTrigger : MonoBehaviour
{
    [SerializeField] private ExorcismQuestion[] question;

    public void SetQuestionToExorcismGame()
    {
        ExorcismGame.Instance.SetQuestions(question);
        ExorcismGame.Instance.ShowPanel();
    }
}
