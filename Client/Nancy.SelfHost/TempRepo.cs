using System;
using WhamBase;
using System.Collections.Generic;
using System.Linq;

namespace Nancy.SelfHost
{
    public class TempRepo <T> : IRepository<T>
    {
        protected List<T> repoList { get; private set; } = new List<T>();

        #region IRepository implementation

        public IEnumerable<T> GetAll()
        {
            return repoList.AsReadOnly();
        }

        public T Get(string id)
        {
            return repoList.Where(i => i.ToString().IndexOf(id) > 0).FirstOrDefault();
        }

        public IEnumerable<T> GetByQuery(Func<T, bool> expression)
        {
            return repoList.Where(i => expression(i)).ToList().AsReadOnly();
        }

        public IEnumerable<T> GetByCommand(string command)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            var del = repoList.Where(i => i.ToString().IndexOf(id) > 0).FirstOrDefault();

            if (del != null)
                repoList.Remove(del);
            
            return del != null;
        }

        public bool Save(T item)
        {
            repoList.Add(item);
            return true;
        }

        #endregion 
    }
}

