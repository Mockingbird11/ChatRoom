using ChatRoom.DataAcces.SqlSugar;
using ChatRoom.IServices;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Services
{
    public class CommonService<T> : ICommonServices<T> where T : class, new()
    {
        public SqlSugarClient _db;

        public CommonService()
        {
            _db = SqlSugarHelper.Instance;
        }

        public Task<int> Add(T model)
        {
            _db.BeginTran();
            int count = _db.Insertable<T>(model).ExecuteCommand();
            _db.CommitTran();
            return Task.Run(() => count);
        }

        public Task<int> Delete(string Id)
        {
            _db.BeginTran();
            int count = _db.Deleteable<T>(Id).ExecuteCommand();
            _db.CommitTran();
            return Task.Run(() => count);
        }

        public Task<List<T>> GetAll()
        {
            return Task.Run(() => _db.Queryable<T>().ToListAsync());
        }

        public Task<T> GetById(string Id)
        {
            return Task.Run(() => _db.Queryable<T>().InSingle(Id));
        }

        public Task<int> Update(T model)
        {
            _db.BeginTran();
            int count = _db.Updateable<T>(model).ExecuteCommand();
            _db.CommitTran();
            return Task.Run(() => count);
        }
    }
}
