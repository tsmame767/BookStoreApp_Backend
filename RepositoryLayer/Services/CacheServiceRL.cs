using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RepositoryLayer.Interfaces;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class CacheServiceRL : ICacheServiceRL
    {
        private IDatabase db;

        public CacheServiceRL(IConnectionMultiplexer redis)
        {
            db = redis.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = db.StringGet(key);
            if (!value.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default(T); // Returns the default value of the type T if the key is not found
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime - DateTimeOffset.Now;
            var json = JsonConvert.SerializeObject(value);
            return db.StringSet(key, json, expiryTime);
        }

        public bool RemoveData(string key)
        {
            return db.KeyDelete(key);
        }
    }
}
