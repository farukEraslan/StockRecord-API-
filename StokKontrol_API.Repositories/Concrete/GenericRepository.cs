﻿using Microsoft.EntityFrameworkCore;
using StokKontrol_API.Entities.Entities;
using StokKontrol_API.Repositories.Abstract;
using StokKontrol_API.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StokKontrol_API.Repositories.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StokKontrolContext _context;

        public GenericRepository(StokKontrolContext context)
        {
            _context = context;
        }

        public bool Add(T item)
        {
            try
            {
                item.AddedDate = DateTime.Now;
                _context.Set<T>().Add(item);
                return Save() > 0; // 1 satır etkileniyorsa true döndürsün.
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    foreach (T item in items) 
                    {
                        item.AddedDate = DateTime.Now;
                        _context.Set<T>().Add(item);
                    }
                    //_context.Set<T>().AddRange(items);
                    ts.Complete(); // Tüm işlemler başarılı olduğunda, yani tüm ekleme işlemleri başarılı olduğunda Complete() olmuş olacak.
                    return Save() > 0; // 1 veya daha fazla satır ekleniyorsa...
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Any(Expression<Func<T, bool>> predicate) => _context.Set<T>().Any(predicate);

        public List<T> GetActive()
        {
            return _context.Set<T>().Where(x => x.IsActive == true).ToList();
        }

        public List<T> GetAll() => _context.Set<T>().ToList();

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query;
        }

        public T GetByDefault(Expression<Func<T, bool>> predicate) => _context.Set<T>().FirstOrDefault(predicate);

        public T GetById(int id) => _context.Set<T>().Find(id);

        public List<T> GetDefault(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate).ToList();

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.IsActive == true);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public bool Remove(T item)
        {
            item.IsActive = false;
            return Update(item);
        }

        public bool Remove(int id)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    T item = GetById(id);
                    item.IsActive = false;
                    return Update(item);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> predicate)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var collection = GetDefault(predicate);     // Verilen ifadeye göre ilgili nesneleri collection'a atıyoruz.
                    int counter = 0;

                    foreach (var item in collection)
                    {
                        item.IsActive = false;
                        bool operationResult = Update(item);    // DB'den silmiyoruz. Durumunu silindi ayarlıyoruz ve bunu da Update() metodu ile gerçekleştiriyoruz.
                        if (operationResult)
                        {
                            counter++;  // Sıradaki item'ın silinme işlemi yani silindi işaretleme başarılı ise sayacı 1 arttırıyoruz.
                        }
                    }

                    if (collection.Count == counter)
                    {
                        ts.Complete();  // Koleksiyondaki eleman sayısı ile silinme işlemi gerçekleşen eleman sayısı eşit ise bu işlemler başarılıdır.
                    }
                    else
                    {
                        return false;   // Aksi halde hiç birinde bir değişiklik yapmadan false döndürür.
                    }
                }

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Update(T item)
        {
            try
            {
                item.ModifiedDate = DateTime.Now;
                _context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Activate(int id)
        {
            T item = GetById(id);
            item.IsActive = true;
            return Update(item);
        }

        public void Detached(T item)
        {
            _context.Entry<T>(item).State = EntityState.Detached;   // Bir entry'i takip etmeyi bırakmak için kullanılır.
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
