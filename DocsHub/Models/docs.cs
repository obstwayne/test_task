using System.Runtime.CompilerServices;

namespace DocsHub.Models
{
    public class docs
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string? Title {  get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice {  get; set; }
    }
}
