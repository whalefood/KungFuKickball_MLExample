using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AIPlayerController : PlayerControllerBase
{

    public bool AttackAction;

    public bool JumpAction;

    public int MovementAction;

    public override void UpdateController()
    {
        this.AttackButton.Pressed = this.AttackAction;

        this.JumpButton.Pressed = this.JumpAction;

        this.StickValueX = this.MovementAction;
    }
}

