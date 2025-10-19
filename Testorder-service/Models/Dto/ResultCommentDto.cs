namespace Testorder_service.Models.Dto
{
    public class ResultCommentDto
    {
        public long Id { get; set; }
        public long ResultId { get; set; }
        public long? SampleId { get; set; }
        public string CommentedBy { get; set; } = default!;
        public string CommentedAt { get; set; } = default!;
        public string? CommentText { get; set; }
        public bool? IsEdited { get; set; }
        public string? EditedAt { get; set; }
        public bool? DeletedFlag { get; set; }
    }
    public class UpsertResultCommentDto
    {
        public long ResultId { get; set; }
        public long? SampleId { get; set; }
        public string CommentedBy { get; set; } = default!;
        public string? CommentText { get; set; }
        public bool? IsEdited { get; set; }
    }
}
