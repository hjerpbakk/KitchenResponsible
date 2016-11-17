using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

public class ServiceCache : IServiceCache{     
    readonly ConcurrentDictionary<int, IActionResult> cache;
    
    public ServiceCache() {
        cache = new ConcurrentDictionary<int, IActionResult>();
    }

    public void Add(IActionResult result) {
        cache.AddOrUpdate(-1, result, (a,b) => result);
    }

    public IActionResult Get() {
        IActionResult result = null;
        cache.TryGetValue(-1, out result);
        return result;
    }

    public void Add(int id, IActionResult result) {
        cache.AddOrUpdate(id, result, (a,b) => result);
    }

    public IActionResult Get(int id) {
        IActionResult result = null;
        cache.TryGetValue(id, out result);
        return result;
    }

    public void MarkAsDirty(int id) {
        IActionResult result = null;
        cache.TryRemove(id, out result);
        cache.TryRemove(-1, out result);
    }
}
