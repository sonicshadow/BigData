using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigData.Models;
using MongoDB.Driver;
using System.Diagnostics;
using MongoDB.Bson;

namespace BigData.Controllers
{

    public class TestController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Test
        public ActionResult Index()
        {
            var ids = db.Coupons.AsQueryable().Take(10000).Select(s => s.Id);
            db.Coupons.UpdateMany(Builders<Coupon>.Filter.In("_id", ids), Builders<Coupon>.Update.Set("CreateDateTime", new DateTime(2017, 12, 6)));
            return Json("1", JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 批量插入
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {

            //List<Coupon> list = new List<Coupon>();
            //var ramdom = new Random();
            //var users = db.Users.AsQueryable().Select(s => s.Id).ToList();
            //var userCount = users.Count();
            //var types = db.CouponTypes.AsQueryable().Where(s => s.ParentID != null).Select(s => s.Id).ToList();
            //for (int i = 0; i < 1000000; i++)
            //{
            //    var count = ramdom.Next(users.Count);
            //    var tCount = ramdom.Next(types.Count - 1);
            //    var randomUser = new List<ObjectId>();
            //    for (int y = 0; y < count; y++)
            //    {
            //        var rId = users[ramdom.Next(userCount)];
            //        if (!randomUser.Any(s => s == rId))
            //        {
            //            randomUser.Add(users[ramdom.Next(userCount)]);
            //        }
            //    }
            //    list.Add(new Coupon()
            //    {
            //        CreateDateTime = DateTime.Now,
            //        Images = new string[] { "https://www.baidu.com/img/bd_logo1.png", "https://www.taobao.com/?spm=a21bo.2017.201857.1.64e2e3bcxp9Lyc" },
            //        Image = "https://www.baidu.com/img/bd_logo1.png",
            //        Name = $"test_{i.ToString("0000000")}",
            //        Price = ramdom.Next(100),
            //        Users = randomUser,
            //        TypeID = types[tCount]
            //    });
            //}
            //db.Coupons.InsertMany(list);
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 单个插入
        /// </summary>
        /// <returns></returns>
        public ActionResult Create1()
        {
            var user = db.Users.AsQueryable().Select(s => s.Id).ToList();
            var random = new Random();
            Coupon model = new Coupon
            {
                CreateDateTime = DateTime.Now,
                Name = "1",
                Price = 10,
                Gprs = new Gprs { Lat = 1, Lng = 1 },
                Count = 10,
                Platform = Platform.Tmall,
                Users = user
            };
            db.Coupons.InsertOne(model);

            return Json("1", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 条件更新
        /// </summary>
        /// <returns></returns>
        public ActionResult Update1()
        {
            var update = Builders<Coupon>.Update.Set("Name", "51234").Set("Price", 1);
            var filter = Builders<Coupon>.Filter.Eq("_id", new ObjectId("5a1f81074fddf5c15cee66bf"));
            db.Coupons.UpdateMany(filter, update);
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 替换更新
        /// </summary>
        /// <returns></returns>
        public ActionResult Update2()
        {
            var coupon = db.Coupons.AsQueryable().FirstOrDefault(s => s.Id == new MongoDB.Bson.ObjectId("5a1f81074fddf5c15cee66bf"));
            coupon.Name = "123123123";
            coupon.Price = 10;
            db.Coupons.ReplaceOne(s => s.Id == new ObjectId("5a1f81074fddf5c15cee66bf"), coupon);
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 条件删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete1()
        {
            var filter = Builders<Coupon>.Filter.Eq("_id", new ObjectId("5a1f81074fddf5c15cee66bf"));
            db.Coupons.DeleteMany(filter);
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 条件删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete2()
        {
            db.Coupons.DeleteOne(s => s.Id == new ObjectId("5a1f81074fddf5c15cee66bf"));
            return Json("1", JsonRequestBehavior.AllowGet);
        }


        public ActionResult Match()
        {
            ////var result = db.Coupons.AsQueryable().Where(s => s.Name.Contains("1"));
            ////var models = result.ToList();
            //var indexkey = IndexKeysDefinition<Coupon>;
            //var index = db.Coupons.Indexes.CreateOne();
            //var c = db.Coupons.AsQueryable().FirstOrDefault(s => s.Id == new ObjectId("5a28a8984fddf331fc9bd93f"));
            //var q = db.Users.AsQueryable().Where(s => c.Users.Contains(s.Id));
            //var u = q.ToList();

            var u1 = db.Users.AsQueryable().FirstOrDefault(s => s.UserName == "test_0@126.com");

            var q1 = db.Coupons.AsQueryable()
                .Where(s => s.Users.Contains(u1.Id))
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Users,
                    s.Price,
                    s.Image,
                    s.CreateDateTime
                });
            var filter = Builders<Coupon>.Filter.AnyEq("Users", u1.Id);
            var q2 = db.Coupons.Aggregate(new AggregateOptions { AllowDiskUse = true }).Match(filter);
            var paged = q2.SortByDescending(s => s.Id).Skip(10000).Limit(100);
            return Json("1", JsonRequestBehavior.AllowGet);
        }


        public ActionResult LeftJoin()
        {
            //var m1 = (from t in db.CouponTypes.AsQueryable()
            //          join c in db.Coupons.AsQueryable()
            //            on t.Id equals c.TypeID into ct
            //          where t.Id == new ObjectId("5a28d9644fddf9deb010aed9")
            //          select new
            //          {
            //              t.Name,
            //              Coupons = ct
            //          });
            //var x = m1.ToList();
            var m2 = (from c in db.Coupons.AsQueryable()
                      join t in db.CouponTypes.AsQueryable()
                        on c.TypeID equals t.Id into ct
                      where c.TypeID == null
                      select new
                      {
                          c.Name,
                          TypeName = ct.Count() > 0 ? ct.First() : null
                      });
            var m3 = m2.ToList();

            return Json("1", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Builders多条件查询，因为IQuery排序占用内存超过100M的时候会报错
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public ActionResult ManyFilter(int page = 1, int pageSize = 50, decimal? min = null, decimal? max = null)
        {
            var filters = new List<FilterDefinition<Coupon>>();
            if (min.HasValue)
            {
                filters.Add(Builders<Coupon>.Filter.Gt("Price", min));
            }
            if (max.HasValue)
            {
                filters.Add(Builders<Coupon>.Filter.Lt("Price", max));
            }
            var query = db.Coupons
               .Aggregate(new AggregateOptions { AllowDiskUse = true });
            if (filters.Count > 0)
            {
                query = query.Match(Builders<Coupon>.Filter.And(filters));
            }
            var result1 = query.SortBy(s => s.Price)
                .Skip(pageSize * 50)
                .Limit(pageSize)
                .ToList();

            return Json("1", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 按日期分组
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupByDate()
        {
            var query = db.Coupons.Aggregate()
                       .Group(new BsonDocument {
                           { "_id", new BsonDocument {
                               { "month", new BsonDocument("$month", "$CreateDateTime") },
                               { "day", new BsonDocument("$dayOfMonth", "$CreateDateTime") },
                               { "year", new BsonDocument("$year", "$CreateDateTime") } } },
                           { "count", new BsonDocument("$sum", 1) } });
            var result = query.ToListAsync().Result.Select(s => new
            {
                Date = new DateTime(s["_id"]["year"].AsInt32,
                s["_id"]["month"].AsInt32,
                s["_id"]["day"].AsInt32),
                Count = s["count"].AsInt32
            }).ToList();
            return Json("1", JsonRequestBehavior.AllowGet);
        }

    }
}