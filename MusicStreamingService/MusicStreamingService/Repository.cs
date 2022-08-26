using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WebApplication1
{
    public class Repository<Song> : IRepository<Song>
    {
        private readonly static string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "songs.json");

        public List<Song> Load()
        {
            if (!File.Exists(_path))
            {
                List<Song> songs = new List<Song>();
                Save(songs);
            }

            using var reader = new StreamReader(_path);
            var json = reader.ReadToEnd();

            return JsonSerializer.Deserialize<List<Song>>(json);
        }

        public void Save(List<Song> songs)
        {
            var content = JsonSerializer.Serialize(songs);

            File.WriteAllText(_path, content);
        }
    }
}
