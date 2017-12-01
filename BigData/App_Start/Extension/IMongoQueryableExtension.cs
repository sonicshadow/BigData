using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BigData
{

    public interface IPagedList
    {
        int PageIndex { get; set; }

        int PageCount { get; set; }

        int Total { get; set; }


    }

    public class PagedList<T> : IEnumerable<T>, IPagedList
    {
        public IEnumerable<T> Data { get; set; }

        public int PageIndex { get; set; }


        public int PageCount { get; set; }


        public int Total { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public static class IMongoQueryableExtension
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index, int pageSize)
        {
            if (pageSize < 1)
            {
                throw new Exception("pageSize less then 1");
            }
            if (index < 1)
            {
                throw new Exception("index less then 1");
            }
            var total = source.Count();
            var pagedList = new PagedList<T>
            {
                Data = source.Skip((index - 1) * pageSize).Take(pageSize).AsEnumerable(),
                PageCount = total / pageSize + (total % pageSize > 0 ? 1 : 0),
                PageIndex = index,
                Total = total
            };
            return pagedList;
        }
    }
}