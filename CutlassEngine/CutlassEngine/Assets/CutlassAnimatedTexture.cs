﻿using Cutlass.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cutlass.Assets
{
    public class CutlassAnimatedTexture : CutlassTexture, ICutlassUpdateable
    {
        #region Fields

        private int _CurrentFrame = 0;

        private int _FrameLength = 1;

        private int _NumberOfFrames = 1;

        private int _FrameCounter = 0;

        #endregion Fields

        #region Properties

        public override Rectangle AreaToRender
        {
            get
            {
                if (_BaseTexture != null)
                    return new Rectangle((_BaseTexture.Width / _NumberOfFrames) * _CurrentFrame,
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
        public CutlassAnimatedTexture()
        { }

        /// <summary>
        /// Construct a new CutlassTexture.
        /// </summary>
        /// <param name="fileName">The asset file name.</param>
        public CutlassAnimatedTexture(string fileName, int numFrames = 1, int frameLength = 1)
            :base(fileName)
        {
            _NumberOfFrames = numFrames;
            _FrameLength = frameLength;
        }

        #endregion Initialization

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            _FrameCounter++;

            if (_FrameCounter == _FrameLength)
            {
                _FrameCounter = 0;
                _CurrentFrame = (_CurrentFrame + 1) % _NumberOfFrames;
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
