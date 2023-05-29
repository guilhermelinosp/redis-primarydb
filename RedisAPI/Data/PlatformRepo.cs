using RedisAPI.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisAPI.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _redis;

        public PlatformRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public void CreatePlatform(Platform? platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            var db = _redis.GetDatabase();

            var serialPlatform = JsonSerializer.Serialize(platform);

            //db.StringSet(platform.Id, serialPlatform);
            //
            //db.SetAdd("PlatformSet", serialPlatform);

            db.HashSet("HashPlatform", new[] { new HashEntry(platform.Id, serialPlatform) });
        }

        public Platform? GetPlatformById(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Invalid platform ID.", nameof(id));
            }

            var db = _redis.GetDatabase();

            //var serialPlatform = db.StringGet(id);

            var serialPlatform = db.HashGet("HashPlatform", id);

            return !serialPlatform.IsNullOrEmpty ? JsonSerializer.Deserialize<Platform>(serialPlatform!) : null;
        }

        public IEnumerable<Platform?>? GetAllByPlatforms()
        {
            var db = _redis.GetDatabase();

            //var completeSet = db.SetMembers("PlatformSet");
            //
            //return completeSet.Length > 0 ? Array.ConvertAll(completeSet, item => JsonSerializer.Deserialize<Platform>(item!)).ToList() : null;

            var completeHash = db.HashGetAll("HashPlatform");

            return completeHash.Length > 0 ? Array.ConvertAll(completeHash, item => JsonSerializer.Deserialize<Platform>(item.Value!)).ToList() : null;
        }


    }
}
