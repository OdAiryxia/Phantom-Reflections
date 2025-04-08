using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGTrigger : ButtonBaseFunction
{
    [SerializeField] private GameObject cg;

    protected override void OnMouseDown()
    {
        if (!ProgressManager.instance.buttonInteruption)
        {
            if (cg != null)
            {
                CGManager.instance.OpenCG(cg);
            }
        }

        base.OnMouseDown();
    }
}
