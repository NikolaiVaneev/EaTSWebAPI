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

        #region Типы оборудования
        /// <summary>
        /// Получить вид(ы) оборудования
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///         EquipmentTypes/Get/
        ///     or
        ///        EquipmentTypes/Get/2
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
            var obj = _db.EquipmentType.Find(id);
            if (obj == null)
            {
                return BadRequest("Объект не найден в базе данных");
            }

            _db.EquipmentType.Remove(obj);
            await _db.SaveChangesAsync();

            return Ok("Объект удален из базы данных");
        }

        #endregion

        #region Классы оборудования
        /// <summary>
        /// Получить класс(ы) оборудования
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///        EquipmentClasses/Get/
        ///     or
        ///        EquipmentClasses/Get/2
        ///
        /// </remarks>
        [Route("/EquipmentClasses/Get/{id?}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentClass>>> GetClasses(int? id)
        {
            if (id == null)
            {
                return await _db.EquipmentClass.Include(u => u.EqupmentType).Include(a => a.Equipments).ToListAsync();
            }
            else
            {
                var obj = await _db.EquipmentClass.Include(u => u.EqupmentType).Include(c => c.Equipments).AsNoTracking().Where(a => a.Id == id).FirstOrDefaultAsync();
                if (obj == null)
                {
                    return BadRequest("Класс не найден");
                }
                else
                {
                    return Ok(obj);
                }
            }

        }

        /// <summary>
        /// Создать новый класс оборудования
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>объект Класс оборудования</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "equipmenttypeId": 1,
        ///        "fullname": "Радиоволновой охранный извещатель",
        ///        "shortname": "РВОИ",
        ///        "isRepair" : true
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/EquipmentClasses/Create")]
        [HttpPost]
        public async Task<ActionResult> CreateEquipmentClass(EquipmentClassVM obj)
        {
            if (obj == null)
            {
                return BadRequest("Object is null");
            }

            // Проверка на наличие типа в БД
            var type = _db.EquipmentType.Find(obj.EquipmentTypeId);
            if (type == null)
            {
                return BadRequest("Тип оборудования не найден в БД");
            }

            // Проверка на пустые строки
            if (string.IsNullOrWhiteSpace(obj.FullName) || string.IsNullOrWhiteSpace(obj.ShortName))
            {
                return BadRequest("Object data is not correct (name or shortname is empty)");
            }

            if (_db.EquipmentClass.Where(a => a.FullName == obj.FullName).Any())
            {
                return BadRequest("Класс с таким именем уже существует в базе данных");
            }



            var eqClass = new EquipmentClass
            {
                EqupmentType = type,
                ShortName = obj.ShortName,
                FullName = obj.FullName,
                IsRepair = obj.IsRepair
            };

            ////////////////////////////////////////
            _db.EquipmentClass.Add(eqClass);
            await _db.SaveChangesAsync();

            return Ok(eqClass);
        }

        /// <summary>
        /// Изменить класс оборудования
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>объект Класс оборудования</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": 1,
        ///        "equipmenttypeId": 1,
        ///        "fullname": "Радиоволновой охранный извещатель",
        ///        "shortname": "РВОИ",
        ///        "isRepair" : true
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/EquipmentClasses/Edit")]
        [HttpPut]
        public async Task<ActionResult> EditEquipmentClass(EquipmentClassVM obj)
        {
            if (obj == null)
            {
                return BadRequest("Object is null");
            }

            // Проверка на наличие типа в БД
            var type = _db.EquipmentType.Find(obj.EquipmentTypeId);
            if (type == null)
            {
                return BadRequest("Тип оборудования не найден в БД");
            }

            // Проверка на пустые строки
            if (string.IsNullOrWhiteSpace(obj.FullName) || string.IsNullOrWhiteSpace(obj.ShortName))
            {
                return BadRequest("Object data is not correct (name or shortname is empty)");
            }

            var oldObj = _db.EquipmentClass.AsNoTracking().Where(u => u.Id == obj.Id).FirstOrDefault();
            if (oldObj == null)
            {
                return BadRequest("Класс оборудования не найден в БД");
            }


            oldObj.EqupmentType = type;
            oldObj.ShortName = obj.ShortName;
            oldObj.FullName = obj.FullName;
            oldObj.IsRepair = obj.IsRepair;
    

            ////////////////////////////////////////
            _db.EquipmentClass.Update(oldObj);
            await _db.SaveChangesAsync();

            return Ok(oldObj);
        }

        /// <summary>
        /// Удалить класс оборудования
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>объект Класс оборудования</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/EquipmentClasses/Delete")]
        [HttpDelete]
        public async Task<ActionResult> DeleteEquipmentClass(int id)
        {
            var obj = _db.EquipmentClass.Find(id);
            if (obj == null)
            {
                return BadRequest("Объект не найден в базе данных");
            }

            _db.EquipmentClass.Remove(obj);
            await _db.SaveChangesAsync();

            return Ok("Объект удален из базы данных");
        }
        #endregion

        #region Виды оборудования
        /// <summary>
        /// Получить вид(ы) оборудования
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///        Equipment/Get/
        ///     or
        ///        Equipment/Get/1
        ///
        /// </remarks>
        [Route("/Equipment/Get/{id?}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentKind>>> GetEquipment(int? id)
        {
            if (id == null)
            {
                return await _db.EquipmentKind.Include(u => u.EquipmentClass).ToListAsync();
            }
            else
            {
                var obj = await _db.EquipmentKind.Include(u => u.EquipmentClass).Where(a => a.Id == id).FirstOrDefaultAsync();
                if (obj == null)
                {
                    return BadRequest("Вид оборудования не найден");
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
        /// <param name="obj"></param>
        /// <returns>объект Вид оборудования</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "equipmentclassId": 1,
        ///        "name": "Арбалет-БЦ",
        ///        "description": "Охранный извещатель для периметра",
        ///        "price" : 89625,
        ///        "isObsolete" : false
        ///     }
        ///     or
        ///     {
        ///         "equipmentclassId": 1,
        ///         "name": "ПИОН-Т",
        ///         "isObsolete" : true
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/Equipment/Create")]
        [HttpPost]
        public async Task<ActionResult> CreateEquipment(EquipmentKindVM obj)
        {
            if (obj == null)
            {
                return BadRequest("Object is null");
            }

            // Проверка на наличие класса в БД
            var eqClass = _db.EquipmentClass.Find(obj.EquipmentClassId);
            if (eqClass == null)
            {
                return BadRequest("Класс оборудования не найден в БД");
            }

            // Проверка на пустые строки
            if (string.IsNullOrWhiteSpace(obj.Name))
            {
                return BadRequest("Object data is not correct (name is empty)");
            }

            if (_db.EquipmentKind.Where(a => a.Name == obj.Name).Any())
            {
                return BadRequest("Вид оборудования с таким именем уже существует в базе данных");
            }

            var equipment = new EquipmentKind
            {
                EquipmentClass = eqClass,
                Name = obj.Name,
                Description = obj.Description,
                Price = obj.Price,
                IsObsolete = obj.IsObsolete
            };

            ////////////////////////////////////////
            _db.EquipmentKind.Add(equipment);
            await _db.SaveChangesAsync();

            return Ok(equipment);
        }

        /// <summary>
        /// Изменить вид оборудования
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>объект Вид оборудования</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": 1,
        ///        "equipmentclassId": 1,
        ///        "name": "Арбалет-БЦ",
        ///        "description": "Охранный извещатель для периметра",
        ///        "price" : 89625,
        ///        "isObsolete" : false
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/Equipment/Edit")]
        [HttpPut]
        public async Task<ActionResult> EditEquipment(EquipmentKindVM obj)
        {
            if (obj == null)
            {
                return BadRequest("Object is null");
            }

            // Проверка на наличие класса в БД
            var eqClass = _db.EquipmentClass.Find(obj.EquipmentClassId);
            if (eqClass == null)
            {
                return BadRequest("Класс оборудования не найден в БД");
            }

            // Проверка на пустые строки
            if (string.IsNullOrWhiteSpace(obj.Name))
            {
                return BadRequest("Object data is not correct (name is empty)");
            }

            var oldObj = _db.EquipmentKind.AsNoTracking().Where(u => u.Id == obj.Id).FirstOrDefault();
            if (oldObj == null)
            {
                return BadRequest("Оборудование не найдено в БД");
            }

            oldObj.EquipmentClass = eqClass;
            oldObj.Name = obj.Name;
            oldObj.Description = obj.Description;
            oldObj.Price = obj.Price;
            oldObj.IsObsolete = obj.IsObsolete;

            ////////////////////////////////////////
            _db.EquipmentKind.Update(oldObj);
            await _db.SaveChangesAsync();

            return Ok(oldObj);
        }

        /// <summary>
        /// Удалить вид оборудования
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>объект Вид оборудования</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        [AuthorizeRoles(UserRole.Administrator)]
        [Route("/Equipment/Delete")]
        [HttpDelete]
        public async Task<ActionResult> DeleteEquipment(int id)
        {
            var obj = _db.EquipmentKind.Find(id);
            if (obj == null)
            {
                return BadRequest("Объект не найден в базе данных");
            }

            _db.EquipmentKind.Remove(obj);
            await _db.SaveChangesAsync();

            return Ok("Объект удален из базы данных");
        }

        #endregion
    }
}
