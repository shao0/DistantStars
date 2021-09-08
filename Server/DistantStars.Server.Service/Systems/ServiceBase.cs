using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DistantStars.Server.DBContext;
using DistantStars.Server.IService.Systems;
using Microsoft.EntityFrameworkCore;

namespace DistantStars.Server.Service.Systems
{
    public class ServiceBase : IServiceBase
    {
        protected readonly EFCoreContext _context;

        public ServiceBase(EFCoreContext context)
        {
            _context = context;
        }

        public async Task<T> FindAsync<T>(int id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            return await _context.Set<T>().Where(funcWhere).ToListAsync();
        }

        public async Task<T> InsertAsync<T>(T t) where T : class
        {
            if (t == null) throw new Exception("新增数据为空");
            _context.Set<T>().Add(t);
            await CommitAsync();
            return t;
        }

        public async Task<IEnumerable<T>> InsertAsync<T>(IEnumerable<T> tList) where T : class
        {
            if (tList == null) throw new Exception("新增数据为空");
            await _context.Set<T>().AddRangeAsync(tList);
            await CommitAsync();
            return tList;
        }

        public async Task<T> UpdateAsync<T>(T t) where T : class
        {
            if (t == null) throw new Exception("更新数据为空");
            _context.Set<T>().Attach(t);
            _context.Entry(t).State = EntityState.Modified;
            await CommitAsync();
            return t;
        }

        public async Task<IEnumerable<T>> UpdateAsync<T>(IEnumerable<T> tList) where T : class
        {
            if (tList == null) throw new Exception("更新数据为空");

            foreach (var t in tList)
            {
                _context.Set<T>().Attach(t);
                _context.Entry(t).State = EntityState.Modified;
            }
            await CommitAsync();
            return tList;
        }

        public async Task DeleteAsync<T>(int id) where T : class
        {
            var t = await FindAsync<T>(id);
            if (t == null) throw new Exception("删除数据为空");
            _context.Set<T>().Remove(t);
            await CommitAsync();
        }

        public async Task DeleteAsync<T>(T t) where T : class
        {
            if (t == null) throw new Exception("删除数据为空");
            _context.Set<T>().Attach(t);
            _context.Set<T>().Remove(t);
            await CommitAsync();
        }

        public async Task DeleteAsync<T>(IEnumerable<T> tList) where T : class
        {
            if (tList == null) throw new Exception("删除数据为空");

            foreach (var t in tList)
            {
                _context.Set<T>().Attach(t);
            }
            _context.Set<T>().RemoveRange(tList);
            await CommitAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            _context?.Dispose();
        }
    }
}
