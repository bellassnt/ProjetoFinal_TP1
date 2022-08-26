using System.Collections.Generic;

namespace MusicStreamingService
{
    public interface IRepository<Song>
    {
        public List<Song> Load();

        public void Save(List<Song> songs);
    }
}
