using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

public class ServiceCache : IServiceCache{
    IActionResult listCache;    
    readonly IDictionary<int, IActionResult> cache;
    
    public ServiceCache() {
        cache = new Dictionary<int, IActionResult>();
    }

    public void Add(IActionResult result) {
        listCache = result;
    }

    public IActionResult Get() {
        if (listCache == null) {
            return null;
        }

        return listCache;
    }

    public void Add(int id, IActionResult result) {
        cache.Add(id, result);
    }

    public IActionResult Get(int id) {
        if (cache.ContainsKey(id)) {
            return cache[id];
        }
        
        return null;
    }

    public void MarkAsDirty(int id) {
        
    }
}
