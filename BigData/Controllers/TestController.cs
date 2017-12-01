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
        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            double s1 = 0, s2 = 0, s3 = 0;
            Stopwatch sw = new Stopwatch();
            Func<Action, double> testTime = a =>
            {
                sw.Start();
                a();
                sw.Stop();
                double time = sw.Elapsed.TotalSeconds;
                sw.Reset();
                return time;
            };
            s3 = testTime(() =>
            {
                var count = db.Coupons.Find(s => s.Price > 10).Count();
                db.Coupons.Find(s => s.Price > 10).Skip(page).Limit(pageSize).ToList();
            });
            s1 = testTime(() =>
            {
                var temp = db.Coupons.AsQueryable().Where(s => s.Price > 10).ToPagedList(page, pageSize).ToList();
            });

            return Json($"s1:{s1},s2:{s2},s3:{s3}", JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {

            List<Coupon> list = new List<Coupon>();
            var ramdom = new Random();
            for (int i = 0; i < 100000; i++)
            {
                list.Add(new Coupon()
                {
                    CreateDateTime = DateTime.Now,
                    Images = new string[] { "https://www.baidu.com/img/bd_logo1.png", "https://www.taobao.com/?spm=a21bo.2017.201857.1.64e2e3bcxp9Lyc" },
                    Image = "https://www.baidu.com/img/bd_logo1.png",
                    Name = "test",
                    Price = ramdom.Next()
                });
            }
            db.Coupons.InsertMany(list);
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update1()
        {
            var update = Builders<Coupon>.Update.Set("Name", "51234").Set("Price", 1);
            var filter = Builders<Coupon>.Filter.Eq("_id", new ObjectId("5a1f81074fddf5c15cee66bf"));
            db.Coupons.UpdateMany(filter, update);
            return Json("1", JsonRequestBehavior.AllowGet);
        }


        public ActionResult Update2()
        {
            var coupon = db.Coupons.AsQueryable().FirstOrDefault(s => s.Id == new MongoDB.Bson.ObjectId("5a1f81074fddf5c15cee66bf"));
            coupon.Name = "123123123";
            coupon.Price = 10;
            db.Coupons.ReplaceOne(s => s.Id == new ObjectId("5a1f81074fddf5c15cee66bf"), coupon);
            return Json("1", JsonRequestBehavior.AllowGet);
        }


        public ActionResult Update3()
        {
            var update = Builders<Coupon>.Update.Pull(s => s.Images, "www.baidu.com");
            var filter = Builders<Coupon>.Filter.Eq("_id", new ObjectId("5a1f81074fddf5c15cee66bf"));
            db.Coupons.UpdateMany(filter, update);
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete1()
        {
            var filter = Builders<Coupon>.Filter.Eq("_id", new ObjectId("5a1f81074fddf5c15cee66bf"));
            db.Coupons.DeleteMany(filter);
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete2()
        {
            db.Coupons.DeleteOne(s => s.Id == new ObjectId("5a1f81074fddf5c15cee66bf"));
            return Json("1", JsonRequestBehavior.AllowGet);
        }
    }
}