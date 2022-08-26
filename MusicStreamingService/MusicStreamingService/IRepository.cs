using System.Collections.Generic;

namespace WebApplication1
{
    public interface IRepository<Song>
    {
        public List<Song> Load();

        public void Save(List<Song> songs);
    }
}
