using System;
using System.Collections.Generic;

namespace WhamBase
{ 
    public interface IRepository<T>
    { 
        IEnumerable<T> GetAll();
        T Get(string id);
        IEnumerable<T> GetByQuery(Func<T, bool> expression);
        IEnumerable<T> GetByCommand(string command);
        bool Delete(string id);
        bool Save(T item);
    }

    public interface IObjectWithId
    {
        string Id { get; set; }
    }
}
