namespace Arthur.Service;

public class RandomNumberService : IRandomNumberService
{
    public List<int> PickNumbers()
    {
        Random random = new();
        HashSet<int> numbersHash = new();
        while (numbersHash.Count < 7)
        {
            numbersHash.Add(random.Next(1, 60));
        }
        return numbersHash.ToList();
    }
}
