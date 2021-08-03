using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KeyboardPlayerController : PlayerControllerBase
{
    public override void UpdateController()
    {
        this.AttackButton.Pressed = Input.GetKey(KeyCode.K);
        this.JumpButton.Pressed = Input.GetKey(KeyCode.J);

        this.StickValueX = 0;
        if (Input.GetKey(KeyCode.A))
        {
            StickValueX = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            StickValueX = 1;
        }

    }
}
