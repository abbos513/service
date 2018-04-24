using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAD_CW2_00003905.DAL.Entities;

namespace WAD_CW2_00003905.DAL.Repositories
{
    public class LoggingRepository
    {
        private  KpiDbEntities db;
        public LoggingRepository()
        {
            db = new KpiDbEntities();
        }

        public IQueryable<Log> GetAll()
        {
            return db.Logs;
        }

        public void Create(Log log)
        {
            db.Logs.Add(log);
            //db.SaveChanges();


            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

        }
    }
}
