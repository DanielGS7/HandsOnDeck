﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HandsOnDeck2.Classes
{
    public class World
    {
        private Effect heightmapEffect;
        private Texture2D heightmapTexture;
        private GraphicsDevice graphicsDevice;
        private Camera camera;

        public World(GraphicsDevice graphicsDevice, ContentManager content, Camera camera)
        {
            this.graphicsDevice = graphicsDevice;
            this.camera = camera;

            heightmapEffect = content.Load<Effect>("Shaders/Heightmap");

            heightmapTexture = new Texture2D(graphicsDevice, 800, 800);

            GenerateHeightMap();
        }

        public void GenerateHeightMap()
        {
            heightmapEffect.Parameters["mapPos"].SetValue(new Vector2(0, 0));
            heightmapEffect.Parameters["res"].SetValue(new Vector2(heightmapTexture.Width, heightmapTexture.Height));
            heightmapEffect.Parameters["WorldViewProjection"].SetValue(camera.Transform);

            RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, heightmapTexture.Width, heightmapTexture.Height);
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.Transparent);

            using (SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice))
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, heightmapEffect);
                spriteBatch.Draw(heightmapTexture, new Rectangle(0, 0, heightmapTexture.Width, heightmapTexture.Height), Color.White);
                spriteBatch.End();
            }

            graphicsDevice.SetRenderTarget(null);
            heightmapTexture = (Texture2D)renderTarget;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(heightmapTexture, new Rectangle(0, 0, heightmapTexture.Width, heightmapTexture.Height), Color.White);
            spriteBatch.End();
        }
    }
}
