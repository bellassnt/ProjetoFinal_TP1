using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/greatestSongsOfAllTime")]
    [ApiController]
    public class SongController : ControllerBase
    {
        public readonly IRepository<Song> _repository;

        public SongController(IRepository<Song> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Song>>> GetAll()
        {
            var songs = _repository.Load();

            return Ok(songs);
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<Song>> GetByTitle(string title)
        {
            var songs = _repository.Load();

            var song = songs.Where(x => String.Equals(x.Title, title, StringComparison.CurrentCultureIgnoreCase)).
                FirstOrDefault();
            if (song == null)
                return NoContent();

            return Ok(song);
        }

        [HttpGet("random")]
        public async Task<ActionResult<Song>> GetRandom()
        {
            var songs = _repository.Load();

            var random = new Random();
            var randomSong = songs.OrderBy(x => random.Next()).FirstOrDefault();

            return Ok(randomSong);
        }

        [HttpGet("top5Artists")]
        public async Task<ActionResult<List<string>>> GetTop5Artists()
        {
            var songs = _repository.Load();

            var top5Artists = songs.GroupBy(x => x.Artist)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .Take(5)
                .ToList();

            return Ok(top5Artists);
        }

        [HttpPost]
        public async Task<ActionResult<List<Song>>> Add(string title, string artist, string album, string year)
        {
            var songs = _repository.Load();

            Song song = new()
            {
                Id = songs.Count + 1,
                Title = title,
                Artist = artist,
                Album = album,
                Year = year,
            };

            songs.Add(song);

            _repository.Save(songs);

            return Ok(songs);
        }

        [HttpDelete]
        public async Task<ActionResult<List<Song>>> Delete(int id)
        {
            var songs = _repository.Load();

            var song = songs.Find(x => x.Id == id);
            if (song == null)
                return NoContent();

            songs.Remove(song);

            _repository.Save(songs);

            return Ok(songs);
        }

        [HttpPut]
        public async Task<ActionResult<List<Song>>> Update([FromBody]int id, string title, string artist, string album, string year)
        {
            var songs = _repository.Load();

            var song = songs.Find(x => x.Id == id);
            if (song == null)
                return BadRequest("Não há música com esse identificador.");

            song.Title = title;
            song.Artist = artist;
            song.Album = album;
            song.Year = year;

            _repository.Save(songs);

            return Ok(songs);
        }
    }
}