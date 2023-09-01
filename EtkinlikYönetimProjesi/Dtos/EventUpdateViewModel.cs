namespace EtkinlikYönetimProjesi.Dtos
{
    public class EventUpdateViewModel
    {
        public int Id { get; set; } 
        public int? Capacity { get; set; }
        public string? Address { get; set; }
        public bool IsCancelled { get; set; }

    }
}
