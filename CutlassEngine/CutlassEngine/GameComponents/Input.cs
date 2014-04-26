using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Cutlass.GameComponents
{
    /// <summary>
    /// Handle input for the game
    /// </summary>
    public class Input : GameComponent
    {
        #region Properties

        /// <summary>Current keyboard state</summary>
        public KeyboardState CurrentKeyboardState;
        /// <summary>Previous keyboard state</summary>
        public KeyboardState LastKeyboardState;

        /// <summary>Current gamepad state</summary>
        public GamePadState CurrentGamePadState;
        /// <summary>Previous gamepad state</summary>
        public GamePadState LastGamePadState;

        /// <summary>Current mouse state</summary>
        public MouseState CurrentMouseState;
        /// <summary>Previous mouse state</summary>
        public MouseState LastMouseState;

        /// <summary>Previous mouse location</summary>
        private Point _LastMouseLocation;

        /// <summary>How far the mouse has moved.</summary>
        public Vector2 MouseMoved
        {
            get { return _MouseMoved; }
        }
        private Vector2 _MouseMoved;

        /// <summary>Was the gamepad connected last update?</summary>
        public bool GamePadWasConnected;

        public static int MenuEntryBuffer = 30;

        #endregion Properties

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
        /// Helper for checking if a key is still pressed
        /// </summary>
        public bool IsKeyStillPressed(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) &&
                LastKeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Helper for checking if a key was released
        /// </summary>
        public bool IsKeyReleased(Keys key)
        {
            return (CurrentKeyboardState.IsKeyUp(key) &&
                LastKeyboardState.IsKeyDown(key));
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
        /// Helper for checking if a button is still pressed
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsButtonStillPressed(Buttons button)
        {
            return (CurrentGamePadState.IsButtonDown(button) &&
                LastGamePadState.IsButtonDown(button));
        }

        /// <summary>
        /// Helper for checking if a button was released
        /// </summary>
        public bool IsButtonReleased(Buttons button)
        {
            return (CurrentGamePadState.IsButtonUp(button) &&
                LastGamePadState.IsButtonDown(button));
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
        /// Checks if a "menu right" input action is still happening.
        /// </summary>
        public bool MenuStillRight
        {
            get
            {
                return IsKeyStillPressed(Keys.Right) ||
                       IsButtonStillPressed(Buttons.DPadRight) ||
                       IsButtonStillPressed(Buttons.LeftThumbstickRight);
            }
        }

        /// <summary>
        /// Checks if menu right was released
        /// </summary>
        public bool MenuRightReleased
        {
            get
            {
                return IsKeyReleased(Keys.Right) ||
                       IsButtonReleased(Buttons.DPadRight) ||
                       IsButtonReleased(Buttons.LeftThumbstickRight);
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
        /// Checks if a "menu left" input action is still happening.
        /// </summary>
        public bool MenuStillLeft
        {
            get
            {
                return IsKeyStillPressed(Keys.Left) ||
                       IsButtonStillPressed(Buttons.DPadLeft) ||
                       IsButtonStillPressed(Buttons.LeftThumbstickLeft);
            }
        }

        /// <summary>
        /// Checks if menu left was released
        /// </summary>
        public bool MenuLeftReleased
        {
            get
            {
                return IsKeyReleased(Keys.Left) ||
                       IsButtonReleased(Buttons.DPadLeft) ||
                       IsButtonReleased(Buttons.LeftThumbstickLeft);
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
