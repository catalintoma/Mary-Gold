using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PagedList.Core;

namespace Marigold
{
    public static class RepositoryExtensions
    {
        //Extends uow paged api with All behavior
        public async static Task<IList<T>> All<T>(this IRepository<T> source, Expression<Func<T, bool>> predicate = null,
            int pageSize = 20, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
                where T : class
        {
            if (predicate == null)
                predicate = notUsed => true;

            var firstPage = await source.GetPagedListAsync(predicate, pageSize: pageSize, include: include);

            if (firstPage.TotalCount <= pageSize)
                return firstPage.Items;

            return (await Task.WhenAll(Enumerable.Range(1, firstPage.TotalPages)
                .Select(i => source.GetPagedListAsync(predicate, pageIndex: i)))).SelectMany(p => p.Items).ToList();
        }


        public async static Task<Unit> RoomUnit(this IRepository<Room> roomRepo)
        {
            return (await roomRepo.GetPagedListAsync(r => r is Room,pageSize:1,
                include:l => l.Include(r => r.Unit)))
            .Items.First().Unit;
        }

        //Adapt two different Page API's (uow to PagedList)
        public static StaticPagedList<T> AdaptPaged<T>(this IRepository<T> source, int pageIndex = 0, int pageSize = 20, 
            Expression<Func<T, bool>> predicate = null,Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
            where T : class
        {
            var paged = source.GetPagedList(pageIndex: pageIndex, pageSize: pageSize,include:include);
            return new StaticPagedList<T>(paged.Items, paged.PageIndex + 1, paged.PageSize, paged.TotalCount);
        }

        //Adapt two different Page API's (uow to PagedList)
        public static StaticPagedList<U> AdaptPaged<T,U>(this IRepository<T> source, Func<T,U> mapper, int pageIndex = 0, int pageSize = 20, 
            Expression<Func<T, bool>> predicate = null,Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
            where T : class
        {
            var paged = source.GetPagedList(pageIndex: pageIndex, pageSize: pageSize,include:include);
            return new StaticPagedList<U>(paged.Items.Select(t => mapper(t)), paged.PageIndex + 1, paged.PageSize, paged.TotalCount);
        }
    }
}