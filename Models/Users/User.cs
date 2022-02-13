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
        public string Name { get; set; }
        /// <summary>
        /// Учреждение
        /// </summary>
        public Agency Agency { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Электронная почта
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        public UserRole Role { get; set; }
        /// <summary>
        /// Активен
        /// </summary>
        public bool IsActive { get; set; }
    }
}
