using HandsOnDeck2.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HandsOnDeck2.Classes.Rendering
{
    public abstract class Screen : IScreen
    {
        protected List<UIElement> uiElements;
        protected GraphicsDevice graphicsDevice;
        protected ContentManager content;
        public bool IsActive { get; set; }

        protected Screen(GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            uiElements = new List<UIElement>();
        }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var element in uiElements)
            {
                element.Update(gameTime, graphicsDevice.Viewport);
            }
        }

        public abstract void HandleInput();

        protected void AddButton(string text, Vector2 positionPercentage, Vector2 sizePercentage, float rotation, string texture, Action onClick)
        {
            uiElements.Add(new Button(content, text, positionPercentage, sizePercentage, rotation, texture, onClick));
        }

        protected void AddCheckbox(Vector2 positionPercentage, Vector2 sizePercentage, string label)
        {
            uiElements.Add(new Checkbox(content, positionPercentage, sizePercentage, label));
        }

        protected void AddVolumeSlider(Vector2 positionPercentage, Vector2 sizePercentage)
        {
            uiElements.Add(new VolumeSlider(content, positionPercentage, sizePercentage));
        }

        protected void AddTextElement(string text, Vector2 positionPercentage, float rotation, Color color, float textScale = 1f)
        {
            uiElements.Add(new TextElement(content, text, positionPercentage, rotation, color, textScale));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var element in uiElements)
            {
                element.Draw(spriteBatch);
            }
        }
    }
}