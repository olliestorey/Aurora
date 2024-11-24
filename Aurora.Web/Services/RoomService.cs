using Aurora.Web.Factories;
using Microsoft.Extensions.Caching.Memory;

namespace Aurora.Web.Services
{
    public class RoomService : IRoomService
    {
        private readonly IMemoryCache _memoryCache;

        public RoomService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<Room> CreateRoom(string roomCode, int numberOfWordsInGame, string? wordList)
        {
            var wordsFactory = new HardcodedWordFactory();
            var room = new Room()
            {
                Code = roomCode,
                Words = [],
                Players = []
            };

            var words = await wordsFactory.GetWords(wordList);
            room.Words = words.OrderBy(x => Guid.NewGuid()).Take(numberOfWordsInGame).ToList();
            var cacheKey = $"room_{room.Code}";
            _memoryCache.Set(cacheKey, room, TimeSpan.FromMinutes(30));

            return room;
        }

        public Room? GetRoomByCode(string code)
        {
            var cacheKey = $"room_{code}";

            if (_memoryCache.TryGetValue(cacheKey, out Room cachedRoom))
            {
                cachedRoom.Players = cachedRoom.Players.OrderByDescending(x => x.Score).ToList();
                return cachedRoom;
            }

            return null;
        }

        public Room JoinRoom(string roomCode, string playerName, string playerEmail)
        {
            var cacheKey = $"room_{roomCode}";

            if (_memoryCache.TryGetValue(cacheKey, out Room cachedRoom))
            {
                var newPlayer = new Player
                {
                    Id = Guid.NewGuid(),
                    Name = playerName,
                    Email = playerEmail,
                    Score = 0,
                    WordsSubmited = []
                };

                cachedRoom.Players.Add(newPlayer);

                _memoryCache.Set(cacheKey, cachedRoom, TimeSpan.FromMinutes(30));

                var playRoom = new Room(cachedRoom);
                playRoom.Players = new List<Player> { newPlayer };
                return playRoom;
            }

            return null;
        }

        public async Task UpdateRoom(Room room)
        {
            var cacheKey = $"room_{room.Code}";
            _memoryCache.Set(cacheKey, room, TimeSpan.FromMinutes(30));
        }

        public bool DeleteRoom(string code)
        {
            var cacheKey = $"room_{code}";
            try
            {
                _memoryCache.Remove(cacheKey);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class Room
    {
        public string Code { get; set; }
        public List<string> Words { get; set; }
        public List<Player> Players { get; set; }

        public Room()
        {
        }

        public Room(Room room)
        {
            Code = room.Code;
            Words = room.Words;
            Players = room.Players;
        }
    }

    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Score { get; set; }
        public List<string> WordsSubmited { get; set; }
    }

    public interface IRoomService
    {
        public Task<Room> CreateRoom(string roomCode, int numberOfWordsInGame, string? wordList);
        public Room? GetRoomByCode(string code);
        public Room JoinRoom(string roomCode, string playerName, string playerEmail);
        public Task UpdateRoom(Room room);
        public bool DeleteRoom(string code);
    }
}
