using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Interfaces;

namespace HandsOnDeck2.Classes
{
    public class AudioManager
    {
        private static AudioManager instance;
        public static AudioManager Instance => instance ??= new AudioManager();

        private Dictionary<string, Song> music;
        private Dictionary<string, SoundEffect> soundEffects;
        private Song currentSong;
        private Song nextSong;
        private float musicTransitionTime;
        private float currentMusicVolume;
        private float nextMusicVolume;

        public float MusicVolume { get; set; } = 1f;
        public float SfxVolume { get; set; } = 1f;
        public bool IsMusicEnabled { get; set; } = true;
        public bool IsSfxEnabled { get; set; } = true;

        public AudioListener Listener { get; set; }

        private AudioManager()
        {
            music = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
            Listener = new AudioListener();
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            //WIP
        }

        public void PlayMusicForState(GameStates state)
        {
            string trackName = GetMusicForState(state);
            PlayMusic(trackName);
        }

        private string GetMusicForState(GameStates state)
        {
            switch (state)
            {
                case GameStates.DefaultMenu:
                    return "?";
                case GameStates.DefaultPlay:
                    return "?";
                default:
                    return "?";
            }
        }

        public void PlayMusic(string trackName, float transitionTime = 1f)
        {
            if (!IsMusicEnabled || !music.ContainsKey(trackName)) return;

            if (currentSong == null)
            {
                currentSong = music[trackName];
                MediaPlayer.Play(currentSong);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = MusicVolume;
            }
            else if (music[trackName] != currentSong)
            {
                nextSong = music[trackName];
                musicTransitionTime = transitionTime;
                currentMusicVolume = MediaPlayer.Volume;
                nextMusicVolume = 0f;
            }
        }

        public void PlaySoundEffect(string sfxName, float volume = 1f, float pitch = 0f, float pan = 0f)
        {
            if (!IsSfxEnabled || !soundEffects.ContainsKey(sfxName)) return;

            soundEffects[sfxName].Play(volume * SfxVolume, pitch, pan);
        }

        public void Update(GameTime gameTime)
        {
            if (nextSong != null)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                currentMusicVolume = MathHelper.Max(currentMusicVolume - deltaTime / musicTransitionTime, 0f);
                nextMusicVolume = MathHelper.Min(nextMusicVolume + deltaTime / musicTransitionTime, MusicVolume);

                MediaPlayer.Volume = currentMusicVolume;

                if (currentMusicVolume == 0f)
                {
                    currentSong = nextSong;
                    nextSong = null;
                    MediaPlayer.Play(currentSong);
                    MediaPlayer.Volume = nextMusicVolume;
                }
            }
            else
            {
                MediaPlayer.Volume = MusicVolume;
            }
        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
            currentSong = null;
            nextSong = null;
        }
    }
}