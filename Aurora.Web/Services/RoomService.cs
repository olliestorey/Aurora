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

        public async Task<Room> CreateRoom(string roomCode, int numberOfWordsInGame)
        {
            var wordsFactory = new HardcodedWordFactory();
            var room = new Room
            {
                Code = roomCode,
                Words = [],
                Players = []
            };

            var words = await wordsFactory.GetWords();
            room.Words = words.OrderBy(x => Guid.NewGuid()).Take(numberOfWordsInGame).ToList();
            var cacheKey = $"room_{room.Code}";
            _memoryCache.Set(cacheKey, room, TimeSpan.FromMinutes(30));

            return room;
        }

        public Room GetRoomByCode(string code)
        {
            var cacheKey = $"room_{code}";

            if (_memoryCache.TryGetValue(cacheKey, out Room cachedRoom))
            {
                return cachedRoom;
            }

            return null;
        }

        public Room JoinRoom(string roomCode, string playerName, string playerEmail)
        {
            var cacheKey = $"room_{roomCode}";

            if (_memoryCache.TryGetValue(cacheKey, out Room cachedRoom))
            {
                cachedRoom.Players.Add(new Player
                {
                    Id = Guid.NewGuid(),
                    Name = playerName,
                    Email = playerEmail,
                    Score = 0,
                    WordsSubmited = []
                });

                _memoryCache.Set(cacheKey, cachedRoom, TimeSpan.FromMinutes(30));

                return cachedRoom;
            }

            return null;
        }

        public async Task UpdateRoom(Room room)
        {
            var cacheKey = $"room_{room.Code}";
            _memoryCache.Set(cacheKey, room, TimeSpan.FromMinutes(30));
        }

        public void DeleteRoom(string code)
        {
            var cacheKey = $"room_{code}";
            _memoryCache.Remove(cacheKey);
        }
    }

    public class Room
    {
        public string Code { get; set; }
        public List<string> Words { get; set; }
        public List<Player> Players { get; set; }
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
        public Task<Room> CreateRoom(string roomCode, int numberOfWordsInGame);
        public Room GetRoomByCode(string code);
        public Room JoinRoom(string roomCode, string playerName, string playerEmail);
        public Task UpdateRoom(Room room);
        public void DeleteRoom(string code);
    }
}
