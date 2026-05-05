using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;

public static class SaveLoadRotRec
{
    private static string path = "PlatBuildvOne.json";

    // =========================
    // SAVE
    // =========================
    public static void Save(List<MinRotRect> list)
    {
        try
        {
            List<SaveRect> saveList = new List<SaveRect>();

            foreach (var r in list)
            {
                saveList.Add(new SaveRect{X = r.centrum.X,Y = r.centrum.Y,Width = r.width,Height = r.height,Rotation = r.rotation});
            }

            string json = JsonSerializer.Serialize(saveList, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json);
        }
        catch (Exception e)
        {
            Console.WriteLine("SaveFel");
            Console.WriteLine("Save error: " + e.Message);
        }
    }

    // =========================
    // LOAD
    // =========================
    public static List<MinRotRect> Load()
    {
        try
        {
            if (!File.Exists(path)){
                return new List<MinRotRect>();
            }

            string json = File.ReadAllText(path);

            List<SaveRect> loadList = JsonSerializer.Deserialize<List<SaveRect>>(json);

            List<MinRotRect> result = new List<MinRotRect>();

            foreach (var s in loadList)
            {
                result.Add(new MinRotRect(s.Rotation,new Vector2(s.X, s.Y),s.Width,s.Height));
            }

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("LoadFel");
            Console.WriteLine("Load error: " + e.Message);
            return new List<MinRotRect>();
        }
    }

    // =========================
    // INTERN SAVE-KLASS
    // =========================
    private class SaveRect
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float Rotation { get; set; }
    }
}