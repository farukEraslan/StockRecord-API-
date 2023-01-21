using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol_API.Service.Abstract
{
    public interface IGenericService<T>
    {
        bool Add(T item);
        bool Add(List<T> items);
        bool Update(T item);
        bool Remove(T item);
        bool Remove(int id);
        bool RemoveAll(Expression<Func<T, bool>> exp);
        public T GetById(int id);
        IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes);
        public T GetByDefault(Expression<Func<T, bool>> predicate); // FirstOrDefault'a benzer bir metot oluşturur.
        public List<T> GetActive();
        IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes);
        public List<T> GetDefault(Expression<Func<T, bool>> predicate);
        public List<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        public bool Activate(int id); // Aktifleştirmek için kullanılacak metot.
        public bool Any(Expression<Func<T, bool>> predicate); // LINQ ifadesi ile var mı diye sorgulama yapacağımız metot.
    }
}
