using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

#region Own modules

using challenge_api.Database.Models;

#endregion

namespace challenge_api.Controllers {

    public class PersonController : BaseController {

        #region Declarations

        DbContextOptionsBuilder dbContextBuilder = new DbContextOptionsBuilder<CHALLENGEContext>() { };

        #endregion

        public PersonController() {
            dbContextBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        #region GETs

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IList<Person>>> Get() {
            IList<Person> personList;

            using (var context = DBContext()) {
                personList = await context.Person.Take(100).ToListAsync();
                return Ok(personList);
            }
        }

        // GET api/values/11161
        [HttpGet("{businessEntityId}")]
        public async Task<ActionResult<Person>> Get(int businessEntityId) {
            Person person;

            using (var context = DBContext()) {
                person = await context.Person.FirstOrDefaultAsync(x => x.BusinessEntityId == businessEntityId);

                if (person == null) {
                    return NotFound($"Person not found with ID: '{ businessEntityId }'");
                }

                return Ok(person);
            }
        }

        #endregion

        #region POSTs

        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] Person data) {
            validateModelFromRequest(data);

            BusinessEntity businessEntity;
            Person person = data;

            using (var context = DBContext(true)) {
                try {
                    businessEntity = new BusinessEntity() {
                        Rowguid = (data.Rowguid == Guid.Empty) ? Guid.NewGuid() : data.Rowguid
                    };
                    var businessEntityResult = await context.BusinessEntity.AddAsync(businessEntity);
                    await context.SaveChangesAsync();

                    person.Rowguid = Guid.NewGuid();
                    person.BusinessEntityId = businessEntityResult.Entity.BusinessEntityId;
                    var personResult = await context.Person.AddAsync(person);
                    await context.SaveChangesAsync();

                    return Ok($"Entity created with the following BusinessEntityID: '${ person.BusinessEntityId }'");
                }
                catch (Exception ex) {
                    return BadRequest($"There was an error trying to save the entity: { ex.Message }. \nDetails: { ex.InnerException }");
                }
            }
        }

        #endregion

        #region PUTs

        [HttpPut]
        public async Task<ActionResult<string>> Put([FromBody] Person data) {
            validateModelFromRequest(data);

            Person person = data;

            using (var context = DBContext(true)) {
                try {
                    var existingPerson = await context.Person.FindAsync(data.BusinessEntityId);
                    if(existingPerson == null) {
                        return NotFound($"The person you are trying to update with the ID: '{ data.BusinessEntityId }', doesn't exist.");
                    }

                    context.Entry(existingPerson).CurrentValues.SetValues(existingPerson);

                    return Ok($"Entity updated correctly.");
                }
                catch (Exception ex) {
                    return BadRequest($"There was an error trying to save the entity: { ex.Message }. \nDetails: { ex.InnerException }");
                }
            }
        }

        #endregion

        #region DELETEs

        [HttpPut("[action]/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            // To Do - Pending
            validateModelFromRequest(id);
            if (ModelState.IsValid)
                using (var context = DBContext())
                {
                    context.Database.ExecuteSqlCommand($"DeleteBusinessEntity {id}");
                }
            return Ok($"Request Completed");
        }

        #endregion

        #region Validators

        private void validateModelFromRequest<T>(T data) {
            if (data == null) {
                throw new NullReferenceException("The person's data is null or empty");
            }
        }

        #endregion
    }
}
