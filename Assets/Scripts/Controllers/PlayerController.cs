using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerController
{
    public class Button
    {
        public bool WasPressedThisFrame { get; private set; }

        private bool pressed;

        public bool Pressed
        {
            get
            {
                return pressed;
            }
            set
            {
                WasPressedThisFrame = value && !pressed;
                pressed = value;
            }
        }
    }

    public Button JumpButton = new Button();

    public Button AttackButton = new Button();

    public float StickValueX { get; set; }

    public virtual void ReadHardware() { }
}
