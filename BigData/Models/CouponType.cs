using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace BigData.Models
{
    public class CouponType
    {
        [BsonId]
        public ObjectId Id { get; set; }


        public string Name { get; set; }


        public int Sort { get; set; }

        public ObjectId? ParentID { get; set; }

    }
}

