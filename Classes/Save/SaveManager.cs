using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using HandsOnDeck2.Classes.GameObject.Entity;
using HandsOnDeck2.Classes.GameObject;
using HandsOnDeck2.Classes.Global;
using System.Diagnostics;

namespace HandsOnDeck2.Classes
{
    public class SaveManager
    {
        private const int MaxSaveSlots = 5;
        private static readonly string SaveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HandsOnDeck", "Saves");
        private static SaveManager instance;

        public static SaveManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SaveManager();
                }
                return instance;
            }
        }

        private SaveManager()
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        public void SaveGame(string saveName, GameSaveData gameState)
        {
            if (gameState == null)
            {
                throw new ArgumentNullException(nameof(gameState), "Game state cannot be null.");
            }

            string filePath = Path.Combine(SaveDirectory, $"{saveName}.json");
            string json = JsonConvert.SerializeObject(gameState, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            File.WriteAllText(filePath, json);
        }

        public GameSaveData LoadData(string saveName)
        {
            string filePath = Path.Combine(SaveDirectory, $"{saveName}.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<GameSaveData>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            }
            return null;
        }

        public List<SaveFileInfo> GetSaveFiles()
        {
            var saveFiles = new List<SaveFileInfo>();
            var directory = new DirectoryInfo(SaveDirectory);
            var files = directory.GetFiles("*.json").OrderByDescending(f => f.LastWriteTime).Take(MaxSaveSlots);

            foreach (var file in files)
            {
                saveFiles.Add(new SaveFileInfo
                {
                    Name = Path.GetFileNameWithoutExtension(file.Name),
                    Date = file.LastWriteTime
                });
            }

            Console.WriteLine($"Found {saveFiles.Count} save files in {SaveDirectory}"); // Debug output
            return saveFiles;
        }

        public bool HasSaveFiles()
        {
            bool hasSaves = GetSaveFiles().Any();
            Console.WriteLine($"HasSaveFiles: {hasSaves}"); // Debug output
            return hasSaves;
        }
    }
    public class SaveFileInfo
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class GameSaveData
    {
        public Boat PlayerBoat { get; set; }
        public List<Island> Islands { get; set; }
        public int Score { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
    }
}