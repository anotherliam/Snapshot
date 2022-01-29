using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class ResourceLoader
    {


        private static string BuildPath(int levelID, string fileName)
        {
            return $"LevelData/{levelID}/{fileName}";
        }

        public static List<Texture2D> LoadImages(int levelID)
        {
            var path = BuildPath(levelID, "images");
            TextAsset data = Resources.Load<TextAsset>(path);

            // For each line, load that image then return it
            List<Texture2D> images = new List<Texture2D>();
            var lines = data.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                var imagePath = BuildPath(levelID, line);
                images.Add(Resources.Load<Texture2D>(imagePath));
            }
            return images;
        }

        public static string LoadStory(int levelID)
        {
            var path = BuildPath(levelID, "story");
            TextAsset data = Resources.Load<TextAsset>(path);
            return data.text;
        }
    }
}
