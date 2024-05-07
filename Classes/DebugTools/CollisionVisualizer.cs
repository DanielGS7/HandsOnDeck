using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HandsOnDeck.Classes.Collisions;
using System.Collections.Generic;

namespace HandsOnDeck.Classes.Managers
{
    public class CollisionVisualizer
    {
        private GraphicsDevice _graphicsDevice;
        private BasicEffect _basicEffect;
        private List<Hitbox> _hitboxes;

        public CollisionVisualizer(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _basicEffect = new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true
            };
            _hitboxes = new List<Hitbox>();
        }

        public void AddHitbox(Hitbox hitbox)
        {
            _hitboxes.Add(hitbox);
        }

        public void RemoveHitbox(Hitbox hitbox)
        {
            _hitboxes.Remove(hitbox);
        }

        public void Draw(Vector2 viewportPosition)
        {
            foreach (var hitbox in _hitboxes)
            {
                var color = hitbox.IsColliding ? Color.Red : Color.Green;
                DrawRectangle(new Rectangle(
                    hitbox.bounds.X - (int)viewportPosition.X,
                    hitbox.bounds.Y - (int)viewportPosition.Y,
                    hitbox.bounds.Width,
                    hitbox.bounds.Height), color * 0.5f);  
            }
        }

        private void DrawRectangle(Rectangle rectangle, Color color)
        {
            var vertices = new[]
            {
                new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Top, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Right, rectangle.Top, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Bottom, 0), color),
                new VertexPositionColor(new Vector3(rectangle.Right, rectangle.Bottom, 0), color)
            };

            var indices = new short[] { 0, 1, 2, 1, 3, 2 };

            _basicEffect.CurrentTechnique.Passes[0].Apply();
            _graphicsDevice.DrawUserIndexedPrimitives(
                PrimitiveType.TriangleList,
                vertices,
                0,
                4,
                indices,
                0,
                2);
        }
    }
}
