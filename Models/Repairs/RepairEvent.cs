using System.ComponentModel.DataAnnotations;
using static EaTSWebAPI.WC;

namespace EaTSWebAPI.Models.Repairs
{
    public class RepairEvent
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Дата события
        /// </summary>
        public DateTime DateEvent { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Объект ремонта
        /// </summary>
        public Repair Repair { get; set; }
        /// <summary>
        /// Тип события
        /// </summary>
        public RepairEventType Event { get; set; }
    }
}
