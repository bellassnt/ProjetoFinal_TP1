using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyPlaylist
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var start = "Bem-vindo ao seu aplicativo de músicas!\n" +
                        "O aplicativo toca músicas aleatoriamente, " +
                        "mas você pode adicionar uma música a sua escolha para entrar na fila e ser programada para tocar.\n";
            Console.WriteLine(start);

            Song song = null;
            Queue<Song> programmedSongs = new Queue<Song>();

            do
            {
                Console.WriteLine("Escolha uma opção:\n" +
                                  "1 para adicionar uma música à fila;\n" +
                                  "2 para dar play;\n" +
                                  "3 para saber quais são os cinco artistas mais tocados (Top 5 Artists).\n" +
                                  "4 para sair.");

                var input = Console.ReadLine();

                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine($"{start}");
                        Console.WriteLine("Digite o nome da música.");
                        var title = Console.ReadLine();
                        Console.Clear();
                        Console.WriteLine($"{start}");

                        song = GetByTitle(title).Result;
                        if(song == null)
                            Console.WriteLine("Não existe correspondência para essa busca.\n");
                        else
                            programmedSongs.Enqueue(song);

                        break;

                    case "2":
                        if (programmedSongs.Count > 0)
                            song = programmedSongs.Dequeue();
                        else
                            song = GetRandom().Result;

                        Console.Clear();
                        Console.WriteLine($"{start}");
                        Console.WriteLine("Tocando agora: \n" +
                                          $"{song.Title}, {song.Artist}\n" +
                                          $"Álbum: {song.Album}\n" +
                                          $"Ano: {song.Year}.\n");

                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine($"{start}");

                        var artists = GetTop5Artists().Result;

                        for (var i = 0; i < artists.Count; i++)
                            Console.WriteLine($"{i + 1}º lugar: {artists[i]}");

                        Console.WriteLine();

                        break
                            ;
                    case "4":
                        Environment.Exit(0);
                        break;
                }

                if (programmedSongs.Count > 0)
                {
                    Console.WriteLine("Lista de músicas programadas: ");
                    foreach (var programmedSong in programmedSongs)
                        Console.WriteLine($"{programmedSong.Title}, {programmedSong.Artist}");

                    Console.WriteLine();
                }
            } while(true);
        }

        public static async Task<Song> GetRandom()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("https://localhost:7013/api/greatestSongsOfAllTime/random");

                response.EnsureSuccessStatusCode();

                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Song>(responseStream);
            }
        }

        public static async Task<Song> GetByTitle(string title)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"https://localhost:7013/api/greatestSongsOfAllTime/{title}");

                if (response.StatusCode == HttpStatusCode.NoContent)
                    return null;

                response.EnsureSuccessStatusCode();

                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Song>(responseStream);
            }
        }

        public static async Task<List<string>> GetTop5Artists()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"https://localhost:7013/api/greatestSongsOfAllTime/top5Artists");

                response.EnsureSuccessStatusCode();

                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<string>>(responseStream);
            }
        }
    }

    public class Song
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("artist")]
        public string Artist { get; set; }

        [JsonPropertyName("album")]
        public string Album { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }
    }
}