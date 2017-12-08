using AspNet.Identity.MongoDB;
using BigData.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BigData
{
    public class ApplicationDbContext : IDisposable
    {

        private MongoClient client;
        private IMongoDatabase database;

        public ApplicationDbContext()
        {
            client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("Coupon");

        }

        public IMongoCollection<IdentityRole> Roles { get { return database.GetCollection<IdentityRole>("Roles"); } }

        public IMongoCollection<ApplicationUser> IdentityUsers { get { return database.GetCollection<ApplicationUser>("Users"); } }

        public IMongoCollection<User> Users { get { return database.GetCollection<User>("Users"); } }

        public Task<List<IdentityRole>> AllRolesAsync()
        {
            return Roles.Find(r => true).ToListAsync();
        }

        public IMongoCollection<Coupon> Coupons { get { return database.GetCollection<Coupon>("Coupons"); } }


        public IMongoCollection<CouponType> CouponTypes { get { return database.GetCollection<CouponType>("CouponType"); } }

        public void Dispose()
        {
        }
    }
}