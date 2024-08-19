using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GameIdea2
{
    [SerializeField]
    public class LevelObject
    {
        public string Key;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
    }
    
    [System.Serializable]
    public class LevelMap
    {
        public string Name;
        public string Author;
        public string CreatedOn;
        public List<LevelObject> TerrestrialObjects;

        public static string SerialiseWorkspace(GameObject workspace, string name, string author)
        {
            var mapData = new LevelMap() {Name = name, Author = author};
            mapData.CreatedOn = DateTime.Now.ToShortDateString();
            mapData.TerrestrialObjects = new List<LevelObject>();
            foreach (Transform child in workspace.transform)
            {
                var indx = child.name.IndexOf("(Clone", StringComparison.Ordinal);
                var assetKey = child.name.Substring(0, indx);
                var position = child.position;
                var rotation = child.rotation.eulerAngles;
                var scale = child.localScale;
                var lvlObj = new LevelObject()
                {
                    Key = assetKey,
                    Rotation = rotation,
                    Position = position,
                    Scale = scale
                };
                mapData.TerrestrialObjects.Add(lvlObj);
            }

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            return JsonConvert.SerializeObject(mapData, settings);
        }

        public static LevelMap LoadMapFromJson(string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;

            return JsonConvert.DeserializeObject<LevelMap>(json);
        }
    }
}