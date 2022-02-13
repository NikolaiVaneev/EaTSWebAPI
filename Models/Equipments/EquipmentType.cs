using System.ComponentModel.DataAnnotations;

namespace EaTSWebAPI.Models.Equipments
{
    /// <summary>
    /// Тип оборудования (ОИ, Видеонаблюдение, Досмотровое)
    /// </summary>
    public class EquipmentType
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Наименование типа
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Классы типа
        /// </summary>
        public virtual ICollection<EquipmentClass>? Classes { get; set; }
        /// <summary>
        /// Сортировка в списке
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
