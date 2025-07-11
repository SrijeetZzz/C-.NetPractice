namespace MyApi.DTOs
{
    public class SubCategoryWithCategoryName
    {
        public string SubName { get; set; } = string.Empty;
        public string CategoryType { get; set; } = string.Empty;
        // public string CategoryId { get; set; } = string.Empty;
    }
    public class SubCategoryWithNameAndLabel
    {
        public string SubName { get; set; } = string.Empty;
        public string CategoryType { get; set; } = string.Empty;
        public string CategoryLabel { get; set; } = string.Empty;
    }
}
