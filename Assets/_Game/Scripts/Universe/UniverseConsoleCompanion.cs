using System;
using System.IO;
using System.Threading.Tasks;
using GameIdea2.Compression;
using GameIdea2.Constants;
using RuntimeDeveloperConsole;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameIdea2
{
    public class UniverseConsoleCompanion
    {
        [ConsoleCommand("Save current level as a file on disk", "savelevel <filename>")]
        public static void SaveLevel(string[] args)
        {
            if (args.Length <= 0)
            {
                Debug.LogError("Invalid amount of args");
                return;
            }

            var filename = args[0];
            if (String.IsNullOrEmpty(filename))
            {
                Debug.LogError("Invalid filaname");
                return;
            }

            if (filename.EndsWith(LevelMapConstants.FILE_EXTENSION))
                filename = filename.Replace(LevelMapConstants.FILE_EXTENSION, String.Empty);

            if (!Universe.Instance.gameObject.scene.name.ToLower().Contains("leveleditor"))
            {
                Debug.LogError("This command cannot be called from this scene");
                return;
            }

            var json = Universe.Instance.SerializeLevel(filename, "internal");
            var compressed = StringCompression.CompressString(json);
            try
            {
                File.WriteAllBytesAsync($"{filename}{LevelMapConstants.FILE_EXTENSION}", compressed);
                Debug.Log($"{filename}{LevelMapConstants.FILE_EXTENSION} write successful");
            }
            catch (Exception ex)
            {
                Debug.Log($"{filename}{LevelMapConstants.FILE_EXTENSION} write failed with exception: {ex.Message}");
            }
        }


        [ConsoleCommand("Load level on disk", "loadlevel <filename>")]
        public static async void LoadLevel(string[] args)
        {
            if (args.Length <= 0)
            {
                Debug.LogError("Invalid amount of args");
                return;
            }

            var filename = args[0];
            if (String.IsNullOrEmpty(filename))
            {
                Debug.LogError("Invalid filaname");
                return;
            }

            if (filename.EndsWith(LevelMapConstants.FILE_EXTENSION))
                filename = filename.Replace(LevelMapConstants.FILE_EXTENSION, String.Empty);

            var path = $"{filename}{LevelMapConstants.FILE_EXTENSION}";
            if (!File.Exists(path))
            {
                Debug.LogError("Requested level file does not exist");
            }

            var prevScene = SceneManager.GetActiveScene().name;
            try
            {
                SceneManager.LoadScene("LevelPlayer");
                await Task.Delay(1000);
                var data = File.ReadAllBytes(path);
                var json = StringCompression.DecompressString(data);
                Universe.Instance.LoadLevelFromJson(json, false);
                Debug.Log($"{filename}{LevelMapConstants.FILE_EXTENSION} load successful");
            }
            catch (Exception ex)
            {
                Debug.Log($"{filename}{LevelMapConstants.FILE_EXTENSION} load failed with exception: {ex.Message}");
                SceneManager.LoadScene(prevScene);
            }
        }
        
    }
}