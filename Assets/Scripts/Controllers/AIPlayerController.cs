using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AIPlayerController : PlayerControllerBase
{
    public bool AttackAction;

    public bool JumpAction;

    public int MovementAction;


    public override void UpdateController()
    {
        this.AttackButton.Pressed = AttackAction;
        this.JumpButton.Pressed = JumpAction;

        this.StickValueX = MovementAction;
    }
}
