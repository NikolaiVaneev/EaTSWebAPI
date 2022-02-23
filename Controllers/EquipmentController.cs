using EaTSWebAPI.Data;
using EaTSWebAPI.Models.Equipments;
using EaTSWebAPI.Service.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static EaTSWebAPI.WC;

namespace EaTSWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class EquipmentController : Controller
    {
        public EquipmentController(ApplicationDbContext db)
        {
            _db = db;
        }
        private readonly ApplicationDbContext _db;


        /// <summary>
        /// Получить вид(ы) оборудования
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         EquipmentTypes/Get/
        ///     }
        ///     or
        ///     {
        ///        EquipmentTypes/Get/2
        ///     }
        ///
        /// </remarks>
        [Route("/EquipmentTypes/Get/{id?}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentType>>> GetTypes(int? id)
        {
            if (id == null)
            {
                return await _db.EquipmentType.Include(a => a.Classes).ThenInclude(c => c.Equipments).ToListAsync();
            }
            else
            {
                var obj = await _db.EquipmentType.Include(t => t.Classes).ThenInclude(c => c.Equipments).AsNoTracking().Where(a => a.Id == id).FirstOrDefaultAsync();
                if (obj == null)
                {
                    return BadRequest("Вид не найден");
                }
                else
                {
                    return Ok(obj);
                }
            }
            
        }

        /// <summary>
        /// Создать новый вид оборудования
        /// </summary>
        /// <param name="type"></param>
        /// <returns>объект Вид оборудования</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "name": "Досмотровое оборудование"
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/EquipmentTypes/Create")]
        [HttpPost]
        public async Task<ActionResult> CreateEquipmentType(EquipmentType type)
        {
            if (type == null)
            {
                return BadRequest("Object is null");
            }

            if (string.IsNullOrWhiteSpace(type.Name))
            {
                return BadRequest("Object data is not correct (name is empty)");
            }

            if (_db.EquipmentType.Where(a => a.Name == type.Name).Any())
            {
                return BadRequest("Объект с таким именем уже существует в базе данных");
            }
            ////////////////////////////////////////
            _db.EquipmentType.Add(type);
            await _db.SaveChangesAsync();

            return Ok(type);
        }

        /// <summary>
        /// Изменить вид оборудования
        /// </summary>
        /// <param name="type"></param>
        /// <returns>объект Вид оборудования</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": 1,
        ///        "name": "Охранные извещатели"
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/EquipmentTypes/Update")]
        [HttpPut]
        public async Task<ActionResult> EditEquipmentType(EquipmentType type)
        {
            if (type == null)
            {
                return BadRequest("Object is null");
            }

            if (string.IsNullOrWhiteSpace(type.Name))
            {
                return BadRequest("Object data is not correct (name is empty)");
            }

            if (_db.EquipmentType.Where(t => t.Id == type.Id).AsNoTracking() == null)
            {
                return BadRequest("Объект не найден в базе данных");
            }

            //if (_db.EquipmentType.Where(a => a.Name == type.Name).AsNoTracking().Any())
            //{
            //    return BadRequest("Объект с таким именем уже существует в базе данных");
            //}
            ////////////////////////////////////////
            _db.EquipmentType.Update(type);
            await _db.SaveChangesAsync();

            return Ok(type);
        }

        /// <summary>
        /// Удалить вид оборудования
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ИД</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": 5
        ///     }
        ///     
        ///     or
        ///     
        ///     /EquipmentTypes/Delete?id=5
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/EquipmentTypes/Delete")]
        [HttpDelete]
        public async Task<ActionResult> DeleteEquipmentType(int id)
        {
            var equipmentType = _db.EquipmentType.Find(id);
            if (equipmentType == null)
            {
                return BadRequest("Объект не найден в базе данных");
            }

            _db.EquipmentType.Remove(equipmentType);
            await _db.SaveChangesAsync();

            return Ok("Объект удален из базы данных");
        }
    }
}
