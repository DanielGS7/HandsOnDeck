using Microsoft.Xna.Framework;
using System;

namespace HandsOnDeck2.Classes
{
    public class TimeModifier
    {
        private static TimeModifier instance;
        public static TimeModifier Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimeModifier();
                }
                return instance;
            }
        }

        public float TimeScale { get; set; } = 1f;

        public void Update(GameTime gameTime)
        {
            gameTime.ElapsedGameTime = TimeSpan.FromTicks((long)(gameTime.ElapsedGameTime.Ticks * TimeScale));
        }

        public void SlowDown(float scale)
        {
            TimeScale = MathHelper.Clamp(scale, 0f, 1f);
        }

        public void Stop()
        {
            TimeScale = 0f;
        }

        public void Reset()
        {
            TimeScale = 1f;
        }
    }
}
