namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
