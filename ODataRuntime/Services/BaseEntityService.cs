using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using ODataRuntime.Interfaces;
using ODataRuntime.Models;

namespace ODataRuntime.Services {
    public abstract class BaseEntityService<TKey, TEntity> : IEntityService<TKey, TEntity>
        where TEntity: BaseEntity<TKey> {
        protected Dictionary<TKey, TEntity> Entities = new Dictionary<TKey, TEntity>();
        protected object LockEntities = new object();

        public Task<bool> Delete(TKey key) {
            lock (LockEntities) {
                return Task.FromResult(Entities.Remove(key));
            }
        }

        public Task<TEntity> Get(TKey key) {
            lock (LockEntities) {
                if (Entities.ContainsKey(key)) {
                    return Task.FromResult(Entities[key]);
                }

                return Task.FromResult(null as TEntity);
            }
        }

        public IQueryable<TEntity> Get() {
            lock (LockEntities) {
                return Entities.Values.ToArray().AsQueryable();
            }
        }

        public Task<TEntity> Patch(TKey key, Delta<TEntity> data) {
            lock (LockEntities) {
                if (Entities.ContainsKey(key)) {
                    TEntity entity = Entities[key];
                    data.Patch(entity);
                    return Task.FromResult(entity);
                }

                return Task.FromResult(null as TEntity);
            }
        }

        public Task<TEntity> Post(TEntity entity) {
            lock (LockEntities) {
                entity.Key = NewKey();
                Entities.Add(entity.Key, entity);
            }

            return Task.FromResult(entity);
        }

        public Task<TEntity> Put(TEntity entity) {
            if (entity.Key.Equals(default(TKey))) {
                return Post(entity);
            }

            lock (LockEntities) {
                Entities[entity.Key] = entity;
            }

            return Task.FromResult(entity);
        }

        protected abstract TKey NewKey();
    }
}