using EaTSWebAPI.Data;
using EaTSWebAPI.Models.Agencyes;
using EaTSWebAPI.Service.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static EaTSWebAPI.WC;

namespace EaTSWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class AgencyController : ControllerBase
    {
        public AgencyController(ApplicationDbContext db)
        {
            _db = db;
        }
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Получить список типов и их учреждений
        /// </summary>
        /// <returns></returns>
        [Route("/AgencyTypes")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgencyType>>> GetAll()
        {
            return await _db.AgencyType.Include(a => a.Agencies.OrderBy(x => x.DisplayOrder)).ToListAsync();
        }


        /// <summary>
        /// Создать новый тип учреждения
        /// </summary>
        /// <param name="agencyType"></param>
        /// <returns>объект Тип учреждения</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "name": "Исправительные колонии",
        ///        "shortName": "ИК"
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/AgencyTypes/Create")]
        [HttpPost]
        public async Task<ActionResult> CreateAgencyType(AgencyType agencyType)
        {
            // TODO Засунуть эти проверки в отдельный метод  
            if (agencyType == null)
            {
                return BadRequest("Object is null");
            }

            if (string.IsNullOrWhiteSpace(agencyType.Name) || string.IsNullOrWhiteSpace(agencyType.ShortName))
            {
                return BadRequest("Object data is not correct (name or shortname)");
            }

            if (_db.AgencyType.Where(a => a.Name == agencyType.Name || a.ShortName == agencyType.ShortName).Any())
            {
                return BadRequest("Object already exists in the database");
            }
            ////////////////////////////////////////
            _db.AgencyType.Add(agencyType);
            await _db.SaveChangesAsync();

            return Ok(agencyType);
        }

        /// <summary>
        /// Изменить тип учреждения
        /// </summary>
        /// <param name="agencyType"></param>
        /// <returns>объект Тип учреждения</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": 1,
        ///        "name": "Исправительные колонии",
        ///        "shortName": "ИК"
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/AgencyTypes/Update")]
        [HttpPut]
        public async Task<ActionResult> EditAgencyType(AgencyType agencyType)
        {
            // TODO Засунуть эти проверки в отдельный метод            
            if (agencyType == null)
            {
                return BadRequest("Object is null");
            }

            if (string.IsNullOrWhiteSpace(agencyType.Name) || string.IsNullOrWhiteSpace(agencyType.ShortName))
            {
                return BadRequest("Object data is not correct (name or shortname)");
            }

            if (_db.AgencyType.Where(a => a.Name == agencyType.Name && a.ShortName == agencyType.ShortName).Any())
            {
                return BadRequest("Object already exists in the database");
            }
            ////////////////////////////////////////

            var type = _db.AgencyType.Find(agencyType.Id);
            if (type == null)
            {
                return BadRequest("Object not found. Check key field");
            }

            type.ShortName = agencyType.ShortName;
            type.Name = agencyType.Name;

            _db.AgencyType.Update(type);
            await _db.SaveChangesAsync();
            return Ok(type);
        }

        /// <summary>
        /// Удалить тип учреждения
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/AgencyTypes/Delete")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAgencyType(int id)
        {
            var agencyType = _db.AgencyType.Find(id);
            if (agencyType == null)
            {
                return BadRequest("Object not found. Check key field");
            }

            _db.AgencyType.Remove(agencyType);
            await _db.SaveChangesAsync();
            return Ok("Object deleted successfully");
        }

        /// <summary>
        /// Создать новое учреждение
        /// </summary>
        /// <param name="agencyVM"></param>
        /// <returns>Объект учреждения</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "name": "ФКУ ИК-2 УФСИН России по РТ",
        ///        "shortName": "ИК-2",
        ///        "agencyTypeId": 1
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/Agencyes/Create")]
        [HttpPost]
        public async Task<ActionResult> CreateAgency(AgencyVM agencyVM)
        {

            if (agencyVM == null)
            {
                return BadRequest("Object is null");
            }

            if (string.IsNullOrWhiteSpace(agencyVM.Name) || string.IsNullOrWhiteSpace(agencyVM.ShortName))
            {
                return BadRequest("Object data is not correct (name or shortname)");
            }

            var agencyType = _db.AgencyType.Find(agencyVM.AgencyTypeId);
            if (agencyType == null)
            {
                return BadRequest("Object data is not correct (agency type)");
            }

            if (_db.Agency.Where(a => a.Name == agencyType.Name || a.ShortName == agencyType.ShortName).Any())
            {
                return BadRequest("Object already exists in the database");
            }

            var agency = new Agency
            {
                ShortName = agencyVM.ShortName,
                Name = agencyVM.Name,
                AgencyType = agencyType
            };

            _db.Agency.Add(agency);
            await _db.SaveChangesAsync();

            return Ok(agency);
        }

        /// <summary>
        /// Изменить данные учреждения
        /// </summary>
        /// <param name="agencyVM"></param>
        /// <returns>Объект учреждения</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///        "id": 1,
        ///        "name": "ФКУ ИК-2 УФСИН России по РТ",
        ///        "shortName": "ИК-2"
        ///     }
        ///     or
        ///     {
        ///        "id": 1,
        ///        "displayOrder": 10
        ///     }
        ///     or
        ///     {
        ///        "id": 1,
        ///        "name": "ФКУ ИК-2 УФСИН России по РТ",
        ///        "shortName": "ИК-2",
        ///        "agencyTypeId": 2,
        ///        "displayOrder": 3
        ///     }
        ///     
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/Agencyes/Update")]
        [HttpPut]
        public async Task<ActionResult> UpdateAgency(AgencyVM agencyVM)
        {
            if (agencyVM == null)
            {
                return BadRequest("Object is null");
            }

            if (agencyVM.Id == null)
            {
                return BadRequest("Object data is not correct (unique identificator)");
            }

            if (_db.Agency.Where(a => a.Name == agencyVM.Name || a.ShortName == agencyVM.ShortName).Any())
            {
                return BadRequest("Object already exists in the database");
            }

            var agency = _db.Agency.Find(agencyVM.Id);

            if (agency == null)
            {
                return BadRequest("Agency not found (check unique identificator)");
            }

            if (agencyVM.ShortName != null)
                agency.ShortName = agencyVM.ShortName;
            if (agencyVM.Name != null)
                agency.Name = agencyVM.Name;

            if (agencyVM.AgencyTypeId != null && agencyVM.AgencyTypeId != 0)
            {
                var agencyType = _db.AgencyType.Find(agencyVM.AgencyTypeId);
                if (agencyType == null)
                {
                    return BadRequest("Object data is not correct (agency type)");
                }
                else
                {
                    agency.AgencyType = agencyType;
                }
            }

            if (agencyVM.DisplayOrder != null && agencyVM.DisplayOrder != 0)
            {
                agency.DisplayOrder = (int)agencyVM.DisplayOrder;
            }

            _db.Agency.Update(agency);
            await _db.SaveChangesAsync();

            return Ok(agency);
        }

        /// <summary>
        /// Удалить учреждения
        /// </summary>
        /// <param name="agencyVM"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///        "id": 1
        ///     }
        ///     
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/Agencyes/Delete")]
        [HttpDelete]
        public async Task<ActionResult> DeteleAgency(AgencyVM agencyVM)
        {
            if (agencyVM == null)
            {
                return BadRequest("Object is null");
            }

            if (agencyVM.Id == null)
            {
                return BadRequest("Object data is not correct (unique identificator)");
            }

            var obj = _db.Agency.Find(agencyVM.Id);
            if (obj == null)
            {
                return BadRequest("Object not found. Check key field");
            }

            _db.Agency.Remove(obj);
            await _db.SaveChangesAsync();
            return Ok("Object deleted successfully");
        }
    }
}
