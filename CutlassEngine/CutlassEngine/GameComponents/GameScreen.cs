using System;
using Cutlass.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;

namespace Cutlass.GameComponents
{
    /// <summary>
    /// Enum describes the screen transition state.
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    /// <summary>
    /// A screen is a single layer that has update and draw logic, and which
    /// can be combined with other layers to build up a complex menu system.
    /// For instance the main menu, the options menu, the "are you sure you
    /// want to quit" message box, and the main game itself are all implemented
    /// as screens.
    /// </summary>
    public abstract class GameScreen
    {
        #region Properties

        /// <summary>
        /// Normally when one screen is brought up over the top of another,
        /// the first screen will transition off to make room for the new
        /// one. This property indicates whether the screen is only a small
        /// popup, in which case screens underneath it do not need to bother
        /// transitioning off.
        /// </summary>
        public bool IsPopup
        {
            get { return _IsPopup; }
            protected set { _IsPopup = value; }
        }
        private bool _IsPopup = false;

        /// <summary>
        /// Indicates how long the screen takes to
        /// transition on when it is activated.
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return _TransitionOnTime; }
            protected set { _TransitionOnTime = value; }
        }
        private TimeSpan _TransitionOnTime = TimeSpan.Zero;

        /// <summary>
        /// Indicates how long the screen takes to
        /// transition off when it is deactivated.
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return _TransitionOffTime; }
            protected set { _TransitionOffTime = value; }
        }
        TimeSpan _TransitionOffTime = TimeSpan.Zero;

        /// <summary>
        /// Gets the current position of the screen transition, ranging
        /// from zero (fully active, no transition) to one (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionPosition
        {
            get { return _TransitionPosition; }
            protected set { _TransitionPosition = value; }
        }
        private float _TransitionPosition = 1;

        /// <summary>
        /// Gets the current alpha of the screen transition, ranging
        /// from 1 (fully active, no transition) to 0 (transitioned
        /// fully off to nothing).
        /// </summary>
        public byte TransitionAlpha
        {
            get { return (byte)(255 - TransitionPosition * 255); }
        }

        /// <summary>
        /// Gets the current screen transition state.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return _ScreenState; }
            protected set { _ScreenState = value; }
        }
        private ScreenState _ScreenState = ScreenState.TransitionOn;

        /// <summary>
        /// There are two possible reasons why a screen might be transitioning
        /// off. It could be temporarily going away to make room for another
        /// screen that is on top of it, or it could be going away for good.
        /// This property indicates whether the screen is exiting for real:
        /// if set, the screen will automatically remove itself as soon as the
        /// transition finishes.
        /// </summary>
        public bool IsExiting
        {
            get { return _IsExiting; }
            protected internal set { _IsExiting = value; }
        }
        private bool _IsExiting = false;


        /// <summary>
        /// Checks whether this screen is active and can respond to user input.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !_OtherScreenHasFocus &&
                       (_ScreenState == ScreenState.TransitionOn ||
                        _ScreenState == ScreenState.Active);
            }
        }

        private bool _OtherScreenHasFocus;

        #endregion

        #region Initialization

        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public virtual void LoadContent()
        { }

        /// <summary>
        /// Unload content for the screen.
        /// </summary>
        public virtual void UnloadContent()
        { }

        #endregion

        #region Update and Draw
        
        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            _OtherScreenHasFocus = otherScreenHasFocus;

            if (_IsExiting)
            {
                // If the screen is going away to die, it should transition off.
                _ScreenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, _TransitionOffTime, 1))
                {
                    // When the transition finishes, remove the screen.
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                if (UpdateTransition(gameTime, _TransitionOffTime, 1))
                {
                    // Still busy transitioning.
                    _ScreenState = ScreenState.TransitionOff;
                }
                else
                {
                    // Transition finished!
                    _ScreenState = ScreenState.Hidden;
                }
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if (UpdateTransition(gameTime, _TransitionOnTime, -1))
                {
                    // Still busy transitioning.
                    _ScreenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    _ScreenState = ScreenState.Active;
                }
            }
        }
        
        /// <summary>
        /// Helper for updating the screen transition position.
        /// </summary>
        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            // How much should we move by?
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            // Update the transition position.
            _TransitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (_TransitionPosition <= 0)) ||
                ((direction > 0) && (_TransitionPosition >= 1)))
            {
                _TransitionPosition = MathHelper.Clamp(_TransitionPosition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }

        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public virtual void HandleInput(Input input) { }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }

        /// <summary>
        /// This is called when the screen should draw after the UI has drawn.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PostUIDraw(GameTime gameTime) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                _IsExiting = true;
            }
        }

        #endregion
    }
}
