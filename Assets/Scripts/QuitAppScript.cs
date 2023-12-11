using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharlesEngine;

public class QuitAppScript : CEScript
{
    public override void Run()
    {
        Application.Quit();
    }
}
