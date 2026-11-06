namespace Model.Other
{
    public class PageInfo<T> where T : class, new()
    {
        public int Total { get; set; }
        public List<T> Data { get; set; }
    }
}
