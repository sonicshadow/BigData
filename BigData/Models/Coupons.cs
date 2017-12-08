using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigData.Models
{
    public class Coupon
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string Image { get; set; }

        public IEnumerable<string> Images { get; set; }

        [BsonIgnoreIfNull]
        public Gprs Gprs { get; set; }

        public int Count { get; set; }

        public Platform Platform { get; set; }

        public List<ObjectId> Users { get; set; }

        public ObjectId? TypeID { get; set; }

    }

    public enum Platform
    {
        Taobao,
        Tmall,
        Jd
    }

    public class Gprs
    {
        public double Lng { get; set; }

        public double Lat { get; set; }
    }
}