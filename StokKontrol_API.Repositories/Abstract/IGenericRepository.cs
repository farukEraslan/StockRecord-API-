using Microsoft.EntityFrameworkCore;
using StokKontrol_API.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol_API.Repositories.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        bool Add(T item);
        bool Add(List<T> items);
        bool Update(T item);
        bool Remove(T item);
        bool Remove(int id);
        bool RemoveAll(Expression<Func<T, bool>> exp);
        public T GetById(int id);
        public T GetByDefault(Expression<Func<T, bool>> predicate); // FirstOrDefault'a benzer bir metot oluşturur.
        public List<T> GetActive();
        IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes);
        public List<T> GetDefault(Expression<Func<T, bool>> predicate);
        public List<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        public bool Activate(int id); // Aktifleştirmek için kullanılacak metot.
        public bool Any(Expression<Func<T, bool>> predicate); // LINQ ifadesi ile var mı diye sorgulama yapacağımız metot.
        public int Save(); // DB'de manipülasyon işleminden sonra 1 veya daha fazla satır eklendiğinde bize kaç satırın etkilendiğini döndürecek metot
        public void Detached(T item);

    }
}
