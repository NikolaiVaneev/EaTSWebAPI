using EaTSWebAPI.Models.Agencyes;
using EaTSWebAPI.Models.Equipments;
using System.ComponentModel.DataAnnotations;
using static EaTSWebAPI.WC;

namespace EaTSWebAPI.Models.Repairs
{
    public class Repair
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Номер в журнале
        /// </summary>
        public int JournalNumber { get; set; }
        /// <summary>
        /// Сдавшее учреждение
        /// </summary>
        public Agency AgencyEntered { get; set; }
        /// <summary>
        /// Принявшее учреждение
        /// </summary>
        public Agency AgencyPassed { get; set; }
        /// <summary>
        /// Оборудование
        /// </summary>
        public EquipmentKind Equipment { get; set; }
        /// <summary>
        /// Кто сдал
        /// </summary>
        public string WhoPassed { get; set; }
        /// <summary>
        /// Кто принял
        /// </summary>
        public string WhoTook { get; set; }
        /// <summary>
        /// Принявший оборудование на ремонт
        /// </summary>
        public User Responsible { get; set; }
        /// <summary>
        /// Серийный номер
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Повреждено?
        /// </summary>
        public bool IsDamaged { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Цена ремонта
        /// </summary>
        public double RepairPrice { get; set; }
        /// <summary>
        /// Текущий статус
        /// </summary>
        public EquipmentCondition CurrentCondition { get; set; }
        /// <summary>
        /// История
        /// </summary>
        public virtual ICollection<RepairEvent>? RepairHistory { get; set; }
    }
}
