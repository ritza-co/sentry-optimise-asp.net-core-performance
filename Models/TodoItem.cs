namespace TodoApi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public bool IsDone { get; set; }

        // 🆕 Self-referencing relationship
        public int? ParentId { get; set; }
        public TodoItem? Parent { get; set; }

        public List<TodoItem> Children { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to TodoItem
        public int TodoItemId { get; set; }
        public TodoItem TodoItem { get; set; }
    }
}
