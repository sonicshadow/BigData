using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigData.Models
{
    public class Coupon
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string Image { get; set; }

        public IEnumerable<string> Images { get; set; }

        public Gprs Gprs { get; set; }

    }

    public class Gprs
    {
        public double Lng { get; set; }

        public double Lat { get; set; }
    }
}