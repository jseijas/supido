using Supido.Business.Query;
using System;
using System.Collections.Generic;

namespace Supido.Business.BO
{
    /// <summary>
    /// Event arguments for Business Object event
    /// </summary>
    /// <typeparam name="TDto">The type of the dto.</typeparam>
    public class BOEventArgs<TDto> : EventArgs
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        /// <value>
        /// The primary key.
        /// </value>
        public string Pk { get; set; }

        /// <summary>
        /// Gets or sets the query information.
        /// </summary>
        /// <value>
        /// The query information.
        /// </value>
        public QueryInfo Info { get; set; }

        /// <summary>
        /// Gets or sets the in data.
        /// </summary>
        /// <value>
        /// The in data.
        /// </value>
        public TDto InData { get; set; }

        /// <summary>
        /// Gets or sets the out data.
        /// </summary>
        /// <value>
        /// The out data.
        /// </value>
        public TDto OutData { get; set; }

        /// <summary>
        /// Gets or sets the list data.
        /// </summary>
        /// <value>
        /// The list data.
        /// </value>
        public IList<TDto> ListData { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        public object Entity { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BOEventArgs{TDto}"/> class.
        /// </summary>
        public BOEventArgs()
            : base() 
        {
            this.Info = null;
            this.InData = default(TDto);
            this.OutData = default(TDto);
            this.ListData = null;
            this.Pk = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BOEventArgs{TDto}"/> class.
        /// </summary>
        /// <param name="pk">The pk.</param>
        /// <param name="info">The information.</param>
        /// <param name="inData">The in data.</param>
        /// <param name="outData">The out data.</param>
        /// <param name="listData">The list data.</param>
        /// <param name="entity">The entity.</param>
        public BOEventArgs(string pk, QueryInfo info, TDto inData, TDto outData, IList<TDto> listData, object entity)
        {
            this.Pk = pk;
            this.Info = info;
            this.InData = inData;
            this.OutData = outData;
            this.ListData = listData;
            this.Entity = entity;
        }

        #endregion
    }
}
