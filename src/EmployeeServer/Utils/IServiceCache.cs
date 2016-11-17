using Microsoft.AspNetCore.Mvc;

public interface IServiceCache {
    void Add(IActionResult result);
    IActionResult Get();    
    void Add(int id, IActionResult result);
    IActionResult Get(int id);
    void MarkAsDirty(int id);
}