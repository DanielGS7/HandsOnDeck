﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using RenderTargetUsage = Microsoft.Xna.Framework.Graphics.RenderTargetUsage;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;
using System.Diagnostics;
using SharpDX.Direct3D9;
using System.Reflection.Metadata;
using HandsOnDeck.Classes.Object.Static;
using HandsOnDeck.Classes.UI;
using Microsoft.Xna.Framework.Input;
using HandsOnDeck.Classes.Object.Entity;

namespace HandsOnDeck.Classes.Managers
{

    public sealed class Renderer
    {
        internal SpriteBatch _spriteBatch;
        GraphicsDeviceManager graphics;

        private static Renderer renderer;
        private static object syncRoot = new object();

        Background background;
        Player player1 = new Player(new Vector2(Game1.ProgramWidth/2, Game1.ProgramHeight/2));
        EnemyBoat enemy1 = new EnemyBoat(new Vector2(Game1.ProgramWidth/3, Game1.ProgramHeight/3));
        KamikazeBoat enemy2= new KamikazeBoat(new Vector2(Game1.ProgramWidth/4, Game1.ProgramHeight/4));
        private Renderer() { }

        internal void Initialize(GraphicsDeviceManager _graphics)
        {
            PresentationParameters pp = _graphics.GraphicsDevice.PresentationParameters;
            Game1.RenderTarget = new RenderTarget2D(_graphics.GraphicsDevice, Game1.ProgramWidth, Game1.ProgramHeight, false,
            SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            graphics = _graphics;
            MapOverlay.GetInstance.Initialize();
            InputManager.GetInstance.Initialize();
        }

        public static Renderer GetInstance
        {
            get
            {
                if (renderer == null)
                {
                    lock (syncRoot)
                    {
                        if (renderer == null)
                            renderer = new Renderer();
                    }
                }
                return renderer;
            }
        }

        public void LoadContent(ContentManager content, SpriteBatch _spriteBatch)
        {
            this._spriteBatch = _spriteBatch;
            Background.GetInstance.LoadContent();
            MapOverlay.GetInstance.LoadContent();
            player1.LoadContent();
            enemy1.LoadContent();
            enemy2.LoadContent();
        }
        public void Update(GameTime gameTime)
        {
            Background.GetInstance.Update(gameTime);
            player1.Update(gameTime);
            enemy1.Update(gameTime, player1);
            enemy2.Update(gameTime, player1);
            InputManager.GetInstance.SetCurrentKeyboardState(Keyboard.GetState());
        }

        public void Draw()
        {
            GetInstance._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            //Alles dat getekend moet worden komt onder deze lijn
            Background.GetInstance.Draw();
            player1.Draw();
            enemy1.Draw();
            enemy2.Draw();
            MapOverlay.GetInstance.Draw();
            //Alles dat getekend moet worden komt boven deze lijn
            GetInstance._spriteBatch.End();
        }
    }
}