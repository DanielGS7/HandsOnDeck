using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using HandsOnDeck2.Enums;
using HandsOnDeck2.Interfaces;

namespace HandsOnDeck2.Classes.Sound
{
    public class AudioManager
    {
        private static AudioManager instance;
        public static AudioManager Instance => instance ??= new AudioManager();

        private Dictionary<GameState, List<Song>> stateMusicMap;
        private Dictionary<string, List<SoundEffect>> soundEffects;
        private Dictionary<string, SoundEffectInstance> loopingSounds;
        private Dictionary<GameState, int> currentSongIndexPerState;
        private Dictionary<GameState, TimeSpan> musicPositionPerState;
        private Song currentSong;
        private GameState currentGameState;
        private float musicTransitionTime;
        private float initialTransitionTime;
        private Random random;
        private bool isFadingOut = false;
        private GameState pendingGameState;

        public float MusicVolume { get { return GlobalInfo.MusicVolume; } set { GlobalInfo.MusicVolume = value; } }
        public float SfxVolume { get { return GlobalInfo.SfxVolume; } set { GlobalInfo.SfxVolume = value; } }
        public bool IsMusicEnabled { get { return GlobalInfo.IsMusicEnabled; } set { GlobalInfo.IsMusicEnabled = value; } }
        public bool IsSfxEnabled { get { return GlobalInfo.IsSfxEnabled; } set { GlobalInfo.IsSfxEnabled = value; } }

        public AudioListener Listener { get; set; }

        private AudioManager()
        {
            stateMusicMap = new Dictionary<GameState, List<Song>>();
            soundEffects = new Dictionary<string, List<SoundEffect>>();
            loopingSounds = new Dictionary<string, SoundEffectInstance>();
            currentSongIndexPerState = new Dictionary<GameState, int>();
            musicPositionPerState = new Dictionary<GameState, TimeSpan>();
            Listener = new AudioListener();
            random = new Random();
        }

        public void LoadContent(ContentManager content)
        {
            LoadMusic(content);
            LoadSoundEffects(content);
        }

        private void LoadMusic(ContentManager content)
        {
            stateMusicMap[GameState.DefaultMenu] = new List<Song> { content.Load<Song>("Music/Menu01") };
            stateMusicMap[GameState.PausedMenu] = new List<Song> { content.Load<Song>("Music/Menu01") };
            stateMusicMap[GameState.GameOverMenu] = new List<Song> { content.Load<Song>("Music/GameOver01") };
            stateMusicMap[GameState.Battle] = new List<Song> { content.Load<Song>("Music/Battle01") };
            stateMusicMap[GameState.DefaultPlay] = new List<Song>
            {
                content.Load<Song>("Music/Play01"),
                content.Load<Song>("Music/Play02"),
                content.Load<Song>("Music/Play03")
            };

            foreach (var state in stateMusicMap.Keys)
            {
                currentSongIndexPerState[state] = -1;
                musicPositionPerState[state] = TimeSpan.Zero;
            }
        }

        private void LoadSoundEffects(ContentManager content)
        {
            string[] sfxCategories = { "notice", "reload", "gong_sink", "Creak", "damage", "explosion", "score", "reward", "cannonball_flyby", "cannon_fire" };
            foreach (string category in sfxCategories)
            {
                soundEffects[category] = new List<SoundEffect>();
                int index = 1;
                while (true)
                {
                    try
                    {
                        string fileName = $"{category}{index:D2}";
                        SoundEffect sfx = content.Load<SoundEffect>($"SFX/{fileName}");
                        soundEffects[category].Add(sfx);
                        index++;
                    }
                    catch (ContentLoadException)
                    {
                        break;
                    }
                }
            }
        }

        public void PlayMusicForState(GameState state, float transitionTime = 1f)
        {
            if (!stateMusicMap.ContainsKey(state) || stateMusicMap[state].Count == 0) return;

            if (state == currentGameState && MediaPlayer.State == MediaState.Playing)
            {
                return;
            }

            if (MediaPlayer.State == MediaState.Playing)
            {
                musicPositionPerState[currentGameState] = MediaPlayer.PlayPosition;
                isFadingOut = true;
                pendingGameState = state;
                musicTransitionTime = transitionTime;
                initialTransitionTime = transitionTime;
            }
            else
            {
                FadeOutAndPlayNew(state, transitionTime);
            }
        }

        private void FadeOutAndPlayNew(GameState state, float transitionTime)
        {
            currentGameState = state;

            if (currentSongIndexPerState[state] == -1)
            {
                currentSongIndexPerState[state] = random.Next(stateMusicMap[state].Count);
            }

            Song selectedSong = stateMusicMap[state][currentSongIndexPerState[state]];
            PlayMusic(selectedSong, musicPositionPerState[state], transitionTime);
        }

        private void PlayMusic(Song song, TimeSpan startPosition, float transitionTime = 1f)
        {
            if (!IsMusicEnabled) return;

            currentSong = song;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0;

            MediaPlayer.Play(currentSong, startPosition);

            musicTransitionTime = transitionTime;
            initialTransitionTime = transitionTime;
        }

        public void Play(string sfxName, int? specificIndex = null, float volume = 1f, float pitch = 0f, float pan = 0f)
        {
            if (!IsSfxEnabled || !soundEffects.ContainsKey(sfxName)) return;

            List<SoundEffect> sfxList = soundEffects[sfxName];
            if (sfxList.Count == 0) return;

            SoundEffect sfx;
            if (specificIndex.HasValue && specificIndex.Value >= 0 && specificIndex.Value < sfxList.Count)
            {
                sfx = sfxList[specificIndex.Value];
            }
            else
            {
                sfx = sfxList[random.Next(sfxList.Count)];
            }

            sfx.Play(volume * SfxVolume, pitch, pan);
        }

        public void PlayLooping(string sfxName, int? specificIndex = null, float volume = 1f, float pitch = 0f, float pan = 0f)
        {
            if (!IsSfxEnabled || !soundEffects.ContainsKey(sfxName)) return;

            List<SoundEffect> sfxList = soundEffects[sfxName];
            if (sfxList.Count == 0) return;

            SoundEffect sfx;
            if (specificIndex.HasValue && specificIndex.Value >= 0 && specificIndex.Value < sfxList.Count)
            {
                sfx = sfxList[specificIndex.Value];
            }
            else
            {
                sfx = sfxList[random.Next(sfxList.Count)];
            }

            if (loopingSounds.ContainsKey(sfxName))
            {
                loopingSounds[sfxName].Stop();
                loopingSounds.Remove(sfxName);
            }

            SoundEffectInstance instance = sfx.CreateInstance();
            instance.IsLooped = true;
            instance.Volume = volume * SfxVolume;
            instance.Pitch = pitch;
            instance.Pan = pan;
            instance.Play();

            loopingSounds[sfxName] = instance;
        }

        public void StopLooping(string sfxName)
        {
            if (loopingSounds.ContainsKey(sfxName))
            {
                loopingSounds[sfxName].Stop();
                loopingSounds.Remove(sfxName);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (isFadingOut)
            {
                UpdateFadeOut(gameTime);
            }
            else
            {
                UpdateMusicTransition(gameTime);
            }

            if (MediaPlayer.State == MediaState.Stopped && !isFadingOut)
            {
                currentSongIndexPerState[currentGameState] = (currentSongIndexPerState[currentGameState] + 1) % stateMusicMap[currentGameState].Count;
                musicPositionPerState[currentGameState] = TimeSpan.Zero;
                PlayMusicForState(currentGameState, 4f);
            }
        }

        private void UpdateFadeOut(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            float progress = 1 - (musicTransitionTime / initialTransitionTime);
            MediaPlayer.Volume = MathHelper.Lerp(MusicVolume, 0, progress);

            musicTransitionTime = Math.Max(0, musicTransitionTime - deltaTime);

            if (musicTransitionTime <= 0)
            {
                isFadingOut = false;
                FadeOutAndPlayNew(pendingGameState, initialTransitionTime);
            }
        }

        private void UpdateMusicTransition(GameTime gameTime)
        {
            if (musicTransitionTime > 0)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                float progress = 1 - (musicTransitionTime / initialTransitionTime);
                    
                MediaPlayer.Volume = MathHelper.Lerp(0, MusicVolume, progress);

                musicTransitionTime = Math.Max(0, musicTransitionTime - deltaTime);
            }
            else if (MediaPlayer.Volume < MusicVolume)
            {
                MediaPlayer.Volume = MusicVolume;
            }
        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
            currentSong = null;
        }
    }
}