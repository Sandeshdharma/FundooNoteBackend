using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RepositoryLayer.Redis
{
    public interface IRedisService
    {

        Task<T?> GetData<T>(string key);

        Task SetData<T>(string key, T value, TimeSpan? expiry = null);

        Task RemoveData(string key);

    }
}
