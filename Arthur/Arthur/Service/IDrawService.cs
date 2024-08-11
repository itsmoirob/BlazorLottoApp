
using Arthur.Models;

namespace Arthur.Service
{
    public interface IDrawService
    {
        Task<List<Draw>> GetDraws();
        Draw? GetDrawById(string id);
    }
}