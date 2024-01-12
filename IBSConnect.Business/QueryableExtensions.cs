using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IBSConnect.Business;

public static class QueryableExtensions
{

    public static async Task<(int, IQueryable<T>)> PagedFilterAsync<T>(this IQueryable<T> query, FilterRequest filter, params Expression<Func<T, bool>>[] queryFilters)
    {
        //if (filter.PageSize < 50)
        //{
        //    filter.PageSize = 50;
        //}

        //if (filter.PageSize > 100)
        //{
        //    filter.PageSize = 100;
        //}

        if (queryFilters != null && queryFilters.Length > 0)
        {
            foreach (var queryFilter in queryFilters)
            {
                if (queryFilter != null)
                {
                    query = query.Where(queryFilter);
                }
            }
        }

        var total = await query.CountAsync();

        var lastPage = total / filter.PageSize;
        lastPage += 1;

        if (filter.Page > lastPage)
        {
            filter.Page = lastPage;
        }

        // The order of these operations is important!
        if (!string.IsNullOrEmpty(filter.OrderBy))
        {
            query = query.OrderBy(filter.OrderBy + " " + filter.SortOrder);
        }

        query = query.Skip((filter.Page - 1) * filter.PageSize);
        query = query.Take(filter.PageSize);


        return (total, query);
    }

    public static (int, IEnumerable<T>) PagedFilter<T>(this IEnumerable<T> query, FilterRequest filter, params Func<T, bool>[] queryFilters)
    {
        //if (filter.PageSize < 50)
        //{
        //    filter.PageSize = 50;
        //}

        //if (filter.PageSize > 100)
        //{
        //    filter.PageSize = 100;
        //}

        if (queryFilters != null && queryFilters.Length > 0)
        {
            foreach (var queryFilter in queryFilters)
            {
                if (queryFilter != null)
                {
                    query = query.Where(queryFilter);
                }
            }
        }

        var total = query.Count();

        var lastPage = total / filter.PageSize;
        lastPage += 1;

        if (filter.Page > lastPage)
        {
            filter.Page = lastPage;
        }

        //// The order of these operations is important!
        //if (!string.IsNullOrEmpty(filter.OrderBy))
        //{
        //    query = query.OrderBy(filter.OrderBy + " " + filter.SortOrder);
        //}

        query = query.Skip((filter.Page - 1) * filter.PageSize);
        query = query.Take(filter.PageSize);


        return (total, query);
    }
}