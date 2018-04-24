using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAD_CW2_00003905.DAL.Entities;

namespace WAD_CW2_00003905.DAL.Repositories
{
    public class UserRepository
    {
        private KpiDbEntities kpiDb;

        public UserRepository()
        {
            kpiDb = new KpiDbEntities();
        }

        public IQueryable<User> GetAll()
        {
            return kpiDb.Users;
        }

        public void Create(User user)
        {
            kpiDb.Users.Add(user);
            kpiDb.SaveChanges();
        }

        public User GetById(int id)
        {
            return kpiDb.Users.Find(id);
        }

        public void Delete(int serviceTypeId)
        {
            kpiDb.Users.Remove(kpiDb.Users.Find(serviceTypeId));
            kpiDb.SaveChanges();
        }

        public void Update(User updatedUser)
        {
            kpiDb.Users.Find(updatedUser.Id).Username = updatedUser.Username;
            kpiDb.Users.Find(updatedUser.Id).Password = updatedUser.Password;
            kpiDb.SaveChanges();
        }
    }
}
