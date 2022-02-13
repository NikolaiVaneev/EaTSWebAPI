using System.ComponentModel;

namespace EaTSWebAPI
{
    /// <summary>
    /// Web constants
    /// </summary>
    public class WC
    {
        /// <summary>
        /// Состояние оборудования
        /// </summary>
        public enum EquipmentCondition
        {
            [Description("На ремонте")]
            OnRepair,
            [Description("Отремонтировано")]
            Repaired,
            [Description("Не подлежит ремонту")]
            NotRepairable,
            [Description("Подготовлено ТЗ")]
            TechnicalConclusion
        }
        /// <summary>
        /// Статус ремонта
        /// </summary>
        public enum RepairEventType
        {
            [Description("Поступило на ремонт")]
            ReceivedForRepair,
            [Description("Направлено в ОМР")]
            SendForDIR,
            [Description("Принято из ОМР")]
            TakenFromDIR,
            [Description("Передано в учреждение")]
            ReturnedToOwner
        }
        /// <summary>
        /// Роли пользователей
        /// </summary>
        public enum UserRole
        {
            [Description("Администратор")]
            Administrator,
            [Description("Куратор")]
            Curator,
            [Description("Пользователь")]
            User
        }
    }
}
