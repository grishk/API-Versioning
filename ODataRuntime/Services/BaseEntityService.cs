using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using ODataRuntime.Interfaces;
using ODataRuntime.Models;

namespace ODataRuntime.Services {
    public abstract class BaseEntityService<TKey, TEntity> : IEntityService<TKey, TEntity>
        where TEntity: BaseEntity<TKey> {
        protected Dictionary<TKey, TEntity> entitis = new Dictionary<TKey, TEntity>();
        protected object lockEntitis = new object();

        public Task<bool> Delete(TKey key) {
            lock (lockEntitis) {
                return Task.FromResult(entitis.Remove(key));
            }
        }

        public Task<TEntity> Get(TKey key) {
            lock (lockEntitis) {
                if (entitis.ContainsKey(key)) {
                    return Task.FromResult(entitis[key]);
                }

                return Task.FromResult(null as TEntity);
            }
        }

        public IQueryable<TEntity> Get() {
            lock (lockEntitis) {
                return entitis.Values.ToArray().AsQueryable();
            }
        }

        public Task<TEntity> Patch(TKey key, Delta<TEntity> data) {
            lock (lockEntitis) {
                if (entitis.ContainsKey(key)) {
                    TEntity entity = entitis[key];
                    data.Patch(entity);
                    return Task.FromResult(entity);
                }

                return Task.FromResult(null as TEntity);
            }
        }

        public Task<TEntity> Post(TEntity entity) {
            lock (lockEntitis) {
                entity.Key = NewKey();
                entitis.Add(entity.Key, entity);
            }

            return Task.FromResult(entity);
        }

        public Task<TEntity> Put(TEntity entity) {
            if (entity.Key.Equals(default(TKey))) {
                return Post(entity);
            }

            lock (lockEntitis) {
                entitis[entity.Key] = entity;
            }

            return Task.FromResult(entity);
        }

        protected abstract TKey NewKey();
    }
}