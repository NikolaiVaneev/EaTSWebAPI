using EaTSWebAPI.Models.Agencyes;
using System.ComponentModel.DataAnnotations;
using static EaTSWebAPI.WC;

namespace EaTSWebAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Фамилия и инициалы
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Учреждение
        /// </summary>
        [Required]
        public Agency Agency { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        [Required]
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Телефон
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Электронная почта
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        [Required]
        public UserRole Role { get; set; }
        /// <summary>
        /// Активен
        /// </summary>
        public bool IsActive { get; set; }
    }
}
