using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge_api.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace challenge_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        #region Declarations

        CHALLENGEContext databaseContext;
        DbContextOptionsBuilder dbContextBuilder = new DbContextOptionsBuilder<Database.Models.CHALLENGEContext>() { };

        #endregion

        public BaseController() {
            Init();   
        }

        public void Init() {
            // To Do
        }

        public CHALLENGEContext DBContext(bool writingPermission = false) {
            if (!writingPermission) {
                dbContextBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }

            databaseContext = new CHALLENGEContext(dbContextBuilder.Options);

            if (!writingPermission) {
                databaseContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                databaseContext.ChangeTracker.AutoDetectChangesEnabled = false;
                databaseContext.Database.AutoTransactionsEnabled = false;
            }

            return databaseContext;
        }
    }
}
