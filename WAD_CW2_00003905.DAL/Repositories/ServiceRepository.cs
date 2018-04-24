using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAD_CW2_00003905.DAL.Entities;

namespace WAD_CW2_00003905.DAL.Repositories
{
    public class ServiceRepository
    {
        private KpiDbEntities kpiDb;

        public ServiceRepository()
        {
            kpiDb = new KpiDbEntities();
        }

        public IQueryable<Service> GetAll()
        {
            return kpiDb.Services;
        }

        public void Create(Service service)
        {
            kpiDb.Services.Add(service);
            kpiDb.SaveChanges();
        }

        public Service GetById(int id)
        {
            return kpiDb.Services.Find(id);
        }

        public void Delete(int serviceId)
        {
            kpiDb.Services.Remove(kpiDb.Services.Find(serviceId));
            kpiDb.SaveChanges();
        }

        public void Update(Service updatedService)
        {
            kpiDb.Services.Find((object)updatedService.Id).Name = updatedService.Name;
            kpiDb.Services.Find((object)updatedService.Id).Type = updatedService.Type;
            kpiDb.Services.Find((object)updatedService.Id).Description = updatedService.Description;
            kpiDb.Services.Find((object)updatedService.Id).Price = updatedService.Price;
            kpiDb.SaveChanges();
        }
    }
}
