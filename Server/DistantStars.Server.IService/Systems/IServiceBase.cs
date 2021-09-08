using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DistantStars.Server.IService.Systems
{
    public interface IServiceBase
    {
        #region 查询
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindAsync<T>(int id) where T : class;

        /// <summary>
        /// 根据表达式条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(Expression<Func<T, bool>> funcWhere) where T : class;
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<T> InsertAsync<T>(T t) where T : class;
        /// <summary>
        /// 多条新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> InsertAsync<T>(IEnumerable<T> tList) where T : class;
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        Task<T> UpdateAsync<T>(T t) where T : class;
        /// <summary>
        /// 多条更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        Task<IEnumerable<T>> UpdateAsync<T>(IEnumerable<T> tList) where T : class;
        #endregion

        #region 删除
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        Task DeleteAsync<T>(int id) where T : class;
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        Task DeleteAsync<T>(T t) where T : class;
        /// <summary>
        /// 多条删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        Task DeleteAsync<T>(IEnumerable<T> tList) where T : class;
        #endregion

        /// <summary>
        /// 提交编辑
        /// </summary>
        Task CommitAsync();
    }
}
