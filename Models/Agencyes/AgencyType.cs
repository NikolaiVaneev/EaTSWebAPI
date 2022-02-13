using System.ComponentModel.DataAnnotations;

namespace EaTSWebAPI.Models.Agencyes
{
    /// <summary>
    /// Тип учреждения
    /// </summary>
    public class AgencyType
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Полное наименование
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Краткое наименование
        /// </summary>
        [Required]
        public string ShortName { get; set; }
        /// <summary>
        /// Учреждения
        /// </summary>
        public virtual ICollection<Agency>? Agencies { get; set; }

    }
}
