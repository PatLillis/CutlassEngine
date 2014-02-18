﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cutlass.Assets;
using Cutlass.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace PirateyGame.Screens
{
    class ScrollMenuScreen : MenuScreen
    {
        public ScrollMenuScreen(string menuTitle)
            : base(menuTitle)
        { }

        public override void LoadContent()
        {
            base.LoadContent();

            //TextureManager.AddTexture(new CutlassTexture("Content/Textures/titleScrollMiddle"), "TitleScrollMiddle");
            //TextureManager.AddTexture(new CutlassTexture("Content/Textures/titleScrollEdge"), "TitleScrollEdge");
            //TextureManager.AddTexture(new CutlassTexture("Content/Textures/backgroundMenuCorner"), "MenuBackgroundCorner");
            //TextureManager.AddTexture(new CutlassTexture("Content/Textures/backgroundMenuVerticalEdge"), "MenuBackgroundVerticalEdge");
            //TextureManager.AddTexture(new CutlassTexture("Content/Textures/backgroundMenuHorizontalEdge"), "MenuBackgroundHorizontalEdge");
        }
    }
}
