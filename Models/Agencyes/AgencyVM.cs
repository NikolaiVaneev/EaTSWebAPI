namespace EaTSWebAPI.Models.Agencyes
{
    public class AgencyVM
    {
        public int? Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Короткое наименование
        /// </summary>
        public string? ShortName { get; set; }
        public int? AgencyTypeId { get; set; }
        /// <summary>
        /// Положение в списке
        /// </summary>
        public int? DisplayOrder { get; set; }
    }
}
