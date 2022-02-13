using System.ComponentModel.DataAnnotations;

namespace EaTSWebAPI.Models.Equipments
{
    /// <summary>
    /// Оборудование (Арбалет, Микрос, Внутренняя камера...)
    /// </summary>
    public class EquipmentKind
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Средняя стоимость
        /// </summary>
        public int? Price { get; set; }
        /// <summary>
        /// Устаревшее
        /// </summary>
        public bool IsObsolete { get; set; }
        /// <summary>
        /// Класс оборудования
        /// </summary>
        public virtual EquipmentClass EquipmentClass { get; set; }

    }
}
