using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.IServices
{
    public interface ICommonServices<T>
    {
        Task<int> Add(T model);

        Task<int> Update(T model);

        Task<int> Delete(string Id);

        Task<List<T>> GetAll();

        Task<T> GetById(string Id);
    }
}
