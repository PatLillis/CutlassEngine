using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Cutlass.GameComponents
{
    public class Input : GameComponent
    {
        #region Fields

        public KeyboardState CurrentKeyboardState;
        public GamePadState CurrentGamePadState;
        public MouseState CurrentMouseState;

        public KeyboardState LastKeyboardState;
        public GamePadState LastGamePadState;
        public MouseState LastMouseState;

        private Point _LastMouseLocation;

        public Vector2 MouseMoved
        {
            get { return _MouseMoved; }
        }
        private Vector2 _MouseMoved;

        public bool GamePadWasConnected;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public Input(Game game)
            :base(game)
        {
            Enabled = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            LastGamePadState = CurrentGamePadState;
            LastMouseState = CurrentMouseState;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
            CurrentMouseState = Mouse.GetState();

            _MouseMoved = new Vector2(LastMouseState.X - CurrentMouseState.X, LastMouseState.Y - CurrentMouseState.Y);
            _LastMouseLocation = new Point(CurrentMouseState.X, CurrentMouseState.Y);

            // Keep track of whether a gamepad has ever been
            // connected, so we can detect if it is unplugged.
            if (CurrentGamePadState.IsConnected)
            {
                GamePadWasConnected = true;
            }
        }

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update.
        /// </summary>
        public bool IsNewKeyPress(Keys key)
        {
                return (CurrentKeyboardState.IsKeyDown(key) &&
                        LastKeyboardState.IsKeyUp(key));
        }

        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// </summary>
        public bool IsNewButtonPress(Buttons button)
        {
                return (CurrentGamePadState.IsButtonDown(button) &&
                        LastGamePadState.IsButtonUp(button));
        }

        /// <summary>
        /// Checks for a "menu select" input action.
        /// </summary>
        public bool MenuSelect
        {
            get
            {
                return IsNewKeyPress(Keys.Space) ||
                   IsNewKeyPress(Keys.Enter) ||
                   IsNewButtonPress(Buttons.A) ||
                   IsNewButtonPress(Buttons.Start);
            }
        }

        /// <summary>
        /// Checks for a "menu cancel" input action.
        /// </summary>
        public bool MenuCancel
        {
            get
            {
                return IsNewKeyPress(Keys.Escape) ||
                   IsNewButtonPress(Buttons.B) ||
                   IsNewButtonPress(Buttons.Back);
            }
        }

        /// <summary>
        /// Checks for a "menu up" input action.
        /// </summary>
        public bool MenuUp
        {
            get
            {
                return IsNewKeyPress(Keys.Up) ||
                       IsNewButtonPress(Buttons.DPadUp) ||
                       IsNewButtonPress(Buttons.LeftThumbstickUp);
            }
        }

        /// <summary>
        /// Checks for a "menu down" input action.
        /// </summary>
        public bool MenuDown
        {
            get
            {
                return IsNewKeyPress(Keys.Down) ||
                       IsNewButtonPress(Buttons.DPadDown) ||
                       IsNewButtonPress(Buttons.LeftThumbstickDown);
            }
        }


        /// <summary>
        /// Checks for a "menu right" input action.
        /// </summary>
        public bool MenuRight
        {
            get
            {
                return IsNewKeyPress(Keys.Right) ||
                       IsNewButtonPress(Buttons.DPadRight) ||
                       IsNewButtonPress(Buttons.LeftThumbstickRight);
            }
        }

        /// <summary>
        /// Checks for a "menu left" input action.
        /// </summary>
        public bool MenuLeft
        {
            get
            {
                return IsNewKeyPress(Keys.Left) ||
                       IsNewButtonPress(Buttons.DPadLeft) ||
                       IsNewButtonPress(Buttons.LeftThumbstickLeft);
            }
        }
        /// <summary>
        /// Checks for a "pause the game" input action.
        /// </summary>
        public bool PauseGame
        {
            get
            {
                return IsNewKeyPress(Keys.Escape) ||
                       IsNewButtonPress(Buttons.Back) ||
                       IsNewButtonPress(Buttons.Start);
            }
        }

        #endregion
    }
}
