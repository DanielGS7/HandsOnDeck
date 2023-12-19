using Microsoft.Xna.Framework;
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

namespace HandsOnDeck.Classes { 

public sealed class Renderer
{
    internal SpriteBatch _spriteBatch;
    GraphicsDeviceManager graphics;

    private static Renderer renderer;
    private static object syncRoot = new object();
    
    Texture2D _boatTexture;
    private Rectangle _spriteselection;
    private int spriteWidth;
    private Renderer() { }

    internal void Initialize(GraphicsDeviceManager _graphics)
    {
        PresentationParameters pp = _graphics.GraphicsDevice.PresentationParameters;
            _spriteselection = new Rectangle(0, 0, 128, 128);
            Game1.RenderTarget = new RenderTarget2D(_graphics.GraphicsDevice, Game1.ProgramWidth, Game1.ProgramHeight, false,
    SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            graphics = _graphics;
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
            //temporary sprite initialization
            _boatTexture = content.Load<Texture2D>("boatsheet");
        }
        public void Update(GameTime gameTime)
        { }

    public void Draw()
    {
        Renderer.GetInstance._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
        _spriteBatch.Draw(_boatTexture, new Vector2(0, 0), Color.White);
        _spriteBatch.Draw(_boatTexture, new Vector2(0, 0), _spriteselection, Color.White);
        GetInstance._spriteBatch.End();
        spriteWidth += 128;
        if (spriteWidth >= 640)
        {
            spriteWidth = 0;
        }
        _spriteselection.X = spriteWidth;

        }
    }
}