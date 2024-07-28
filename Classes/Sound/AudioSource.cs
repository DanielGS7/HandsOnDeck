using Microsoft.Xna.Framework;

namespace HandsOnDeck2.Classes
{
    public class AudioSource
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public float Loudness { get; set; }
        public string SoundEffectName { get; set; }

        public AudioSource(string soundEffectName, Vector2 position, float radius, float loudness)
        {
            SoundEffectName = soundEffectName;
            Position = position;
            Radius = radius;
            Loudness = loudness;
        }

        public void Play()
        {
            Vector2 direction = Position - AudioManager.Instance.Listener.Position;
            float distance = direction.Length();

            if (distance > Radius) return;

            float attenuation = 1f - (distance / Radius);
            float volume = Loudness * attenuation;
            float pan = MathHelper.Clamp(direction.X / (Radius * 0.5f), -1f, 1f);

            AudioManager.Instance.PlaySoundEffect(SoundEffectName, volume, 0f, pan);
        }
    }
}