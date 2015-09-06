using Supido.Business.BO;
using Supido.Business.Context;
using Supido.Business.Meta;
using Supido.Business.Query;
using Supido.Core.Container;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Supido.Business.Filter
{
    /// <summary>
    /// Base Context filter for a business object
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class BaseContextBOFilter<TEntity> : BaseBOFilter
    {
        #region - Static Fields -

        /// <summary>
        /// The contains method
        /// </summary>
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

        /// <summary>
        /// The starts with method
        /// </summary>
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });

        /// <summary>
        /// The ends with method
        /// </summary>
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the context manager.
        /// </summary>
        /// <value>
        /// The context manager.
        /// </value>
        public IUserContext ContextManager
        {
            get
            {
                return (this.Parent as IContextEntityBO).ContextManager;
            }
        }

        #endregion

        #region - Methods -

        #region - Protected Methods -

        /// <summary>
        /// From a DTO field name, returns its equivalent entity field name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        protected virtual string DtoFieldNameToEntityFieldName(string name) 
        {
            IMetamodelEntity metaEntity = IoC.Get<ISecurityManager>().MetamodelManager.GetEntity(this.Parent.EntityType);
            return metaEntity.GetEntityFieldName(this.Parent.DtoType, name);

        }

        /// <summary>
        /// From a facet value, returns an expression.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected virtual Expression GetValueExpression(MemberExpression member, FacetValueInfo value)
        {
            PropertyInfo info = (PropertyInfo)member.Member;
            object convertedvalue = TypeDescriptor.GetConverter(info.PropertyType).ConvertFromString(value.Value);
            Expression constantExpression = Expression.Constant(convertedvalue, info.PropertyType);
            switch (value.Operation)
            {
                case FacetOperation.Contains: return Expression.Call(member, containsMethod, constantExpression);
                case FacetOperation.EndsWith: return Expression.Call(member, endsWithMethod, constantExpression);
                case FacetOperation.Equal: return Expression.Equal(member, constantExpression);
                case FacetOperation.GreaterEqual: return Expression.GreaterThanOrEqual(member, constantExpression);
                case FacetOperation.GreaterThan: return Expression.GreaterThan(member, constantExpression);
                case FacetOperation.IsNotNull: return Expression.NotEqual(member, Expression.Constant(null, info.PropertyType));
                case FacetOperation.IsNull: return Expression.Equal(member, Expression.Constant(null, info.PropertyType));
                case FacetOperation.LessEqual: return Expression.LessThanOrEqual(member, constantExpression);
                case FacetOperation.LessThan: return Expression.LessThan(member, constantExpression);
                case FacetOperation.NotEqual: return Expression.NotEqual(member, constantExpression);
                case FacetOperation.StartsWith: return Expression.Call(member, startsWithMethod, constantExpression);
                default: return Expression.Equal(member, constantExpression);
            }
        }

        /// <summary>
        /// Gets an expression for a facet.
        /// </summary>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <param name="facet">The facet.</param>
        /// <returns></returns>
        protected virtual Expression GetFacetExpression(ParameterExpression parameterExpression, FacetInfo facet)
        {
            string fieldName = this.DtoFieldNameToEntityFieldName(facet.Name);
            string[] tokens = fieldName.Split('.');
            MemberExpression member = Expression.Property(parameterExpression, tokens[0]);
            for (int i = 1; i < tokens.Length; i++)
            {
                member = Expression.Property(member, tokens[i]);
            }
            Expression firstExpression = this.GetValueExpression(member, facet.Values[0]);
            for (int i = 1; i < facet.Values.Count; i++)
            {
                Expression currentExpression = this.GetValueExpression(member, facet.Values[i]);
                firstExpression = Expression.Or(firstExpression, currentExpression);
            }
            return firstExpression;
        }

        /// <summary>
        /// Gets the field expression.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        protected virtual Expression<Func<TEntity, object>> GetFieldExpression(string fieldName)
        {
            ParameterExpression parameter = Expression.Parameter(this.Parent.EntityType, "p");
            string[] tokens = fieldName.Split('.');
            MemberExpression member = Expression.Property(parameter, tokens[0]);
            for (int i = 1; i < tokens.Length; i++)
            {
                member = Expression.Property(member, tokens[i]);
            }
            return Expression.Lambda<Func<TEntity, object>>(Expression.Convert(member, typeof(object)), parameter);
        }

        /// <summary>
        /// Gets the dto field expression.
        /// </summary>
        /// <param name="dtoFieldName">Name of the dto field.</param>
        /// <returns></returns>
        protected virtual Expression<Func<TEntity, object>> GetDtoFieldExpression(string dtoFieldName)
        {
            return this.GetFieldExpression(this.DtoFieldNameToEntityFieldName(dtoFieldName));
        }

        /// <summary>
        /// Applies the default order.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        protected virtual IQueryable ApplyDefaultOrder(IQueryable query)
        {
            IQueryable result = query;
            IMetamodelEntity metaEntity = IoC.Get<ISecurityManager>().MetamodelManager.GetEntity(this.Parent.EntityType);
            int i = 0;
            foreach (IMetamodelField field in metaEntity.Fields)
            {
                if (field.IsPrimaryKey)
                {
                    if (i == 0)
                    {
                        result = (result as IQueryable<TEntity>).OrderBy(this.GetFieldExpression(field.Name));
                    }
                    else
                    {
                        result = ((IOrderedQueryable<TEntity>)result).ThenBy(this.GetFieldExpression(field.Name));
                    }
                    i++;
                }
            }
            return result;
        }

        #endregion

        #region - Methods from BaseBOFilter -

        /// <summary>
        /// Applies the order filter.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        protected override IQueryable ApplyOrderFilter(IQueryable query, QueryInfo info)
        {
            IQueryable result = query;
            if ((info == null) || (info.Orders == null) || (info.Orders.Count == 0))
            {
                return this.ApplyDefaultOrder(result);
            }
            IOrderedQueryable<TEntity> resultq;
            OrderInfo orderInfo = info.Orders[0];
            if (orderInfo.IsAscending)
            {
                resultq = (result as IQueryable<TEntity>).OrderBy(this.GetDtoFieldExpression(orderInfo.Name));
            }
            else
            {
                resultq = (result as IQueryable<TEntity>).OrderByDescending(this.GetDtoFieldExpression(orderInfo.Name));
            }
            for (int i = 1; i < info.Orders.Count; i++)
            {
                orderInfo = info.Orders[i];
                if (orderInfo.IsAscending)
                {
                    resultq = resultq.ThenBy(this.GetDtoFieldExpression(orderInfo.Name));
                }
                else
                {
                    resultq = resultq.ThenByDescending(this.GetDtoFieldExpression(orderInfo.Name));
                }
            }
            return resultq;
        }

        /// <summary>
        /// Applies the where filter.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        protected override IQueryable ApplyWhereFilter(IQueryable query, QueryInfo info)
        {
            IQueryable result = query;
            if ((info == null) || (info.Facets == null) || (info.Facets.Count == 0)) 
            {
                return result;
            }
            ParameterExpression parameterExpression = Expression.Parameter(this.Parent.EntityType, "p");
            Expression firstExpression = this.GetFacetExpression(parameterExpression, info.Facets[0]);
            for (int i = 1; i < info.Facets.Count; i++)
            {
                Expression currentExpression = this.GetFacetExpression(parameterExpression, info.Facets[i]);
                firstExpression = Expression.And(firstExpression, currentExpression);
            }
            result = (result as IQueryable<TEntity>).Where(Expression.Lambda<Func<TEntity, bool>>(firstExpression, parameterExpression));
            return result;
        }

        #endregion

        #endregion
    }
}
