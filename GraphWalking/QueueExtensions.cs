namespace GraphWalking;

public static class QueueExtensions
{
    public static void EnqueueAll<T>(this Queue<T> queue, IEnumerable<T> values)
    {
        foreach(T value in values) 
        {
            queue.Enqueue(value);
        }
    }
}
