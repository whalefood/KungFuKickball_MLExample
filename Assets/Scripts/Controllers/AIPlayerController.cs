using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AIPlayerController : PlayerController
{
    public bool AttackAction;

    public bool JumpAction;

    public int MovementAction;


    public override void ReadHardware()
    {
        this.AttackButton.Pressed = AttackAction;
        this.JumpButton.Pressed = JumpAction;

        this.StickValueX = MovementAction;
    }
}
