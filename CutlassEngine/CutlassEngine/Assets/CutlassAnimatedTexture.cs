using System;
using Microsoft.Xna.Framework;
using Cutlass.Interfaces;
using Cutlass.Utilities;
using Cutlass.GameComponents;

namespace Cutlass.Assets
{
    public class CutlassAnimatedTexture : CutlassTexture, ICutlassUpdateable
    {
        #region Fields

        private const int FramesPerSecond = 60;
        private const float MillisecondsPerFrame = 1000f / FramesPerSecond;

        public int CurrentFrame
        {
            get { return _CurrentFrame; }
        }
        private int _CurrentFrame = 0;

        private int _FrameLength = 1;

        private int _NumberOfFrames = 1;

        private int _FrameCounter = 0;

        private double _SubFrameCounter = 0;

        #endregion Fields

        #region Properties

        public override Rectangle AreaToRender
        {
            get
            {
                if (_BaseTexture != null)
                    return new Rectangle(((_BaseTexture.Width / _NumberOfFrames) * _CurrentFrame),
                        0,
                        _BaseTexture.Width / _NumberOfFrames,
                        _BaseTexture.Height);
                else
                    return Rectangle.Empty;
            }
        }

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Construct a new CutlassTexture.
        /// </summary>
        /// <param name="filename">The asset file name.</param>
        public CutlassAnimatedTexture(string filename, int numFrames = 1, int frameLength = 1)
            : base(filename)
        {
            _NumberOfFrames = numFrames;
            _FrameLength = frameLength;
        }

        #endregion Initialization

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            _SubFrameCounter += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_SubFrameCounter >= MillisecondsPerFrame)
            {
                _SubFrameCounter -= MillisecondsPerFrame;
                _FrameCounter++;

                if (_FrameCounter == _FrameLength)
                {
                    _FrameCounter = 0;
                    _CurrentFrame = (_CurrentFrame + 1) % _NumberOfFrames;
                }
            }
        }

        #endregion Update and Draw

        #region Public Methods

        public void Reset()
        {
            _CurrentFrame = 0;
            _FrameCounter = 0;
        }

        #endregion Public Methods
    }
}
