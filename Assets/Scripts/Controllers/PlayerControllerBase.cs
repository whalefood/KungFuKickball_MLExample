using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class PlayerControllerBase
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

    /// <summary>
    /// Value from -1 to 1
    /// </summary>
    public float StickValueX { get; set; }

    public virtual void UpdateController() { }

}
