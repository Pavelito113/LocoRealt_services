namespace LocoRealt.Models
{
    public class PaginationModel
    {
        // Текущая страница (по умолчанию 1)
        public int CurrentPage { get; set; } = 1;

        // Общее количество страниц
        public int TotalPages { get; set; }

        // Общее количество элементов
        public int TotalItems { get; set; }

        // Количество элементов на страницу (например, 10, 20, 50)
        public int ItemsPerPage { get; set; } = 10;

        // Можно ли перейти на предыдущую страницу
        public bool HasPrevious => CurrentPage > 1;

        // Можно ли перейти на следующую страницу
        public bool HasNext => CurrentPage < TotalPages;

      
       
        [System.Text.Json.Serialization.JsonIgnore]
        public Dictionary<string, string> RouteValues { get; set; } = new();
    }
}
