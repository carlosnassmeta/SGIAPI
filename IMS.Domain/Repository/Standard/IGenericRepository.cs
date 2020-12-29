using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMS.Domain.Repository.Standard
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// Asynchronously get and entity by id. Where TEntity is a class that represents the database domain entity.
        /// If the entity loaded on the entity framework context it will be returned, if the context does not contains it
        /// the entity will be loaded on the context.
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the entity, otherwise null if the entity was not found.
        /// </returns>
        Task<TEntity> GetByIdAsync(object id);
        /// <summary>
        /// Asynchronously get a list of eitities. Where TEntity is a class that represents the database domain entity.
        /// If the entities are loaded on the entity framework context it will be returned, if the context does not contains it
        /// the entity will be loaded on the context.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains an empty list if nothing was found, otherwise an IList containing 
        /// the entities found.
        /// </returns>
        Task<IList<TEntity>> GetAllAsync();

        /// <summary>
        /// Asynchronously add new entity. Where TEntity is a class that represents the database domain entity.
        /// </summary>
        /// <param name="obj">Entity to be added</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the new entity added on the database.
        /// </returns>
        Task<TEntity> AddAsync(TEntity obj);
        /// <summary>
        /// Asynchronously add a list of entities. Where TEntity is a class that represents the database domain entity.
        /// </summary>
        /// <param name="entities">IEnumerable of entities to be added</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Asynchronously update an entity and entity on entity framework context and database. 
        /// Where TEntity is a class that represents the database domain entity.
        /// </summary>
        /// <param name="obj">Entity to be updated</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        Task UpdateAsync(TEntity obj);
        /// <summary>
        /// Update an entity and entity on entity framework context and database. 
        /// Where TEntity is a class that represents the database domain entity.
        /// </summary>
        /// <param name="obj">Entity to be updated</param>
        /// <returns>
        /// Returns the updated entity
        /// </returns>
        TEntity Update(TEntity obj);
        /// <summary>
        /// Update a list of entities on entity framework context and database. 
        /// Where TEntity is a class that represents the database domain entity.
        /// </summary>
        /// <param name="entities">IEnumerable of entities to be updated</param>
        /// <returns>
        /// No return
        /// </returns>
        void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Asynchronously removes an entity on the entity framework context and database.
        /// </summary>
        /// <param name="id">Entity id to be removed</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        Task RemoveAsync(object id);
        /// <summary>
        /// Removes an entity on the entity framework context and database.
        /// </summary>
        /// <param name="obj">Entity to be removed</param>
        /// <returns>
        /// Returns the removed entity.
        /// </returns>
        TEntity Remove(TEntity obj);
        /// <summary>
        /// Removes a list of entities on entity framework context and database. 
        /// Where TEntity is a class that represents the database domain entity.
        /// </summary>
        /// <param name="entities">IEnumerable of entities to be removed</param>
        /// <returns>
        /// No return
        /// </returns>
        void RemoveRange(IEnumerable<TEntity> entities);
        /// <summary>
        /// Asynchronously removes a list of entities on entity framework context and database. 
        /// Where TEntity is a class that represents the database domain entity.
        /// </summary>
        /// <param name="entities">IEnumerable of entities to be removed</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}
