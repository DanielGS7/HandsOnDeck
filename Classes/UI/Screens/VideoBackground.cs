using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class VideoBackground
{
    private List<Texture2D> frames;
    private int currentFrameIndex;
    private float frameTime;
    private float timeSinceLastFrame;
    private GraphicsDevice graphicsDevice;

    public VideoBackground(GraphicsDevice graphicsDevice, ContentManager content, string textureBaseName, int frameCount, float fps)
    {
        this.graphicsDevice = graphicsDevice;
        frames = new List<Texture2D>();
        for (int i = 1; i <= frameCount; i++)
        {
            frames.Add(content.Load<Texture2D>($"{textureBaseName}{i:D3}"));
        }
        currentFrameIndex = 0;
        frameTime = 1f / fps;
        timeSinceLastFrame = 0f;
    }

    public void Update(GameTime gameTime)
    {
        timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (timeSinceLastFrame >= frameTime)
        {
            currentFrameIndex = (currentFrameIndex + 1) % frames.Count;
            timeSinceLastFrame -= frameTime;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        if (frames.Count > 0)
        {
            var sourceRectangle = CalculateSourceRectangle(destinationRectangle, frames[currentFrameIndex].Bounds);
            spriteBatch.Draw(frames[currentFrameIndex], destinationRectangle, sourceRectangle, Color.White);
        }
    }

    private Rectangle CalculateSourceRectangle(Rectangle destinationRectangle, Rectangle textureFrame)
    {
        float textureAspectRatio = (float)textureFrame.Width / textureFrame.Height;
        float screenAspectRatio = (float)destinationRectangle.Width / destinationRectangle.Height;

        int sourceWidth, sourceHeight;
        int sourceX = 0, sourceY = 0;

        if (textureAspectRatio > screenAspectRatio)
        {
            sourceHeight = textureFrame.Height;
            sourceWidth = (int)(sourceHeight * screenAspectRatio);
            sourceX = (textureFrame.Width - sourceWidth) / 2;
        }
        else
        {
            sourceWidth = textureFrame.Width;
            sourceHeight = (int)(sourceWidth / screenAspectRatio);
            sourceY = (textureFrame.Height - sourceHeight) / 2;
        }

        return new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
    }
}