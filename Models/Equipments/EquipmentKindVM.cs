namespace EaTSWebAPI.Models.Equipments
{
    public class EquipmentKindVM
    {
        public int Id { get; set; }
        public int EquipmentClassId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public bool IsObsolete { get; set; }
    }
}
