namespace ExcludedProductAPI.Helpers
{
    public static class Utils
    {
        public static List<List<T>> SplitListIntoBatches<T>(List<T> list, int batchSize)
        {
            List<List<T>> batches = new List<List<T>>();
            for (int i = 0; i < list.Count; i += batchSize)
            {
                List<T> batch = list.GetRange(i, Math.Min(batchSize, list.Count - i));
                batches.Add(batch);
            }
            return batches;
        }
    }
}
