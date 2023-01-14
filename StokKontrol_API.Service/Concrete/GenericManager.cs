﻿using StokKontrol_API.Entities.Entities;
using StokKontrol_API.Repositories.Abstract;
using StokKontrol_API.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol_API.Service.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : BaseEntity
    {
        private readonly IGenericRepository<T> _repository;

        public GenericManager(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public bool Activate(int id)
        {
            if (id == 0 || GetById(id) == null)
            {
                return false;
            }
            else
            {
                return _repository.Activate(id);
            }
        }

        public bool Add(T item)
        {
            return _repository.Add(item);
        }

        public bool Add(List<T> items)
        {
            return _repository.Add(items);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _repository.Any(predicate);
        }

        public List<T> GetActive()
        {
            return _repository.GetActive();
        }

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            return _repository.GetActive(includes);
        }

        public List<T> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return _repository.GetAll(includes);
        }

        public T GetByDefault(Expression<Func<T, bool>> predicate)
        {
            return _repository.GetByDefault(predicate);
        }

        public T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> predicate)
        {
            return _repository.GetDefault(predicate);
        }

        public bool Remove(T item)
        {
            if (item == null)
            {
                return false;
            }
            else
            {
                return _repository.Remove(item);
            }
        }

        public bool Remove(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            else
            {
                return _repository.Remove(id);
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            return _repository.RemoveAll(exp);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(T item)
        {
            if (item == null)
            {
                return false;
            }
            else
            {
                return _repository.Update(item);
            }
        }
    }
}
