using BusinessLayer.Interfaces;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CacheServiceBL:ICacheServiceBL
    {
        private readonly ICacheServiceRL _cacheService;

        public CacheServiceBL(ICacheServiceRL cacheService)
        {
            _cacheService = cacheService;
        }

        public T GetData<T>(string key)
        {
            return _cacheService.GetData<T>(key);
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            return _cacheService.SetData<T>(key, value, expirationTime);
        }
        public bool RemoveData(string key)
        {
            return _cacheService.RemoveData(key);
        }
    }
}
