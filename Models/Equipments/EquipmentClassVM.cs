namespace EaTSWebAPI.Models.Equipments
{
    public class EquipmentClassVM
    {
        public int Id { get; set; }
        public int EquipmentTypeId { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public bool IsRepair { get; set; }
    }
}
