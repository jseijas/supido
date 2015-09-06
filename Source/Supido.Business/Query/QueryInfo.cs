using System.Collections.Generic;

namespace Supido.Business.Query
{
    /// <summary>
    /// Class for the information of a query.
    /// </summary>
    public class QueryInfo
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the facets.
        /// </summary>
        /// <value>
        /// The facets.
        /// </value>
        public IList<FacetInfo> Facets { get; set; }

        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>
        /// The orders.
        /// </value>
        public IList<OrderInfo> Orders { get; set; }

        /// <summary>
        /// Gets or sets the skip records.
        /// </summary>
        /// <value>
        /// The skip records.
        /// </value>
        public int? SkipRecords { get; set; }

        /// <summary>
        /// Gets or sets the take records.
        /// </summary>
        /// <value>
        /// The take records.
        /// </value>
        public int? TakeRecords { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryInfo"/> class.
        /// </summary>
        public QueryInfo()
        {
            this.Facets = new List<FacetInfo>();
            this.Orders = new List<OrderInfo>();
            this.SkipRecords = null;
            this.TakeRecords = null;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds the facets from another query info.
        /// </summary>
        /// <param name="src">The source.</param>
        public void AddFacetsFrom(QueryInfo src)
        {
            foreach (FacetInfo facet in src.Facets)
            {
                foreach (FacetValueInfo value in facet.Values)
                {
                    this.Where(facet.Name, value.Operation, value.Value);
                }
            }
        }

        /// <summary>
        /// Gets the facet information given the facet name.
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <returns></returns>
        public FacetInfo GetFacetInfo(string facetName)
        {
            foreach (FacetInfo facet in this.Facets)
            {
                if (facet.Name.Equals(facetName))
                {
                    return facet;
                }
            }
            return null;
        }

        /// <summary>
        /// Removes the facet information.
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        public void RemoveFacetInfo(string facetName)
        {
            FacetInfo info = this.GetFacetInfo(facetName);
            if (info != null)
            {
                this.Facets.Remove(info);
            }
        }

        /// <summary>
        /// Adds a new order by.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public QueryInfo OrderBy(string fieldName)
        {
            this.Orders.Add(new OrderInfo(fieldName));
            return this;
        }

        /// <summary>
        /// Adds a new order by descendant.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public QueryInfo OrderByDesc(string fieldName)
        {
            this.Orders.Add(new OrderInfo(fieldName, false));
            return this;
        }

        /// <summary>
        /// Skips the specified number of records.
        /// </summary>
        /// <param name="skipRecords">The skip records.</param>
        /// <returns></returns>
        public QueryInfo Skip(int skipRecords)
        {
            this.SkipRecords = skipRecords;
            return this;
        }

        /// <summary>
        /// Takes the specified number of records.
        /// </summary>
        /// <param name="takeRecords">The take records.</param>
        /// <returns></returns>
        public QueryInfo Take(int takeRecords)
        {
            this.TakeRecords = takeRecords;
            return this;
        }

        /// <summary>
        /// Adds a new facet value.
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public QueryInfo Where(string facetName, FacetOperation operation, string value)
        {
            FacetInfo facet = this.GetFacetInfo(facetName);
            if (facet == null)
            {
                facet = new FacetInfo(facetName);
                this.Facets.Add(facet);
            }
            facet.Values.Add(new FacetValueInfo(operation, value));
            return this;
        }

        /// <summary>
        /// Adds a Where with Equal
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public QueryInfo Equal(string facetName, string value)
        {
            return this.Where(facetName, FacetOperation.Equal, value);
        }

        /// <summary>
        /// Adds a Where with Not Equal
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public QueryInfo NotEqual(string facetName, string value)
        {
            return this.Where(facetName, FacetOperation.NotEqual, value);
        }

        /// <summary>
        /// Adds a Where with Contains
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public QueryInfo Contains(string facetName, string value)
        {
            return this.Where(facetName, FacetOperation.Contains, value);
        }

        /// <summary>
        /// Adds a Where with Starts With
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public QueryInfo StartsWith(string facetName, string value)
        {
            return this.Where(facetName, FacetOperation.StartsWith, value);
        }

        /// <summary>
        /// Adds a Where with Ends With
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public QueryInfo EndsWith(string facetName, string value)
        {
            return this.Where(facetName, FacetOperation.EndsWith, value);
        }

        /// <summary>
        /// Adds a Where with Is Null
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <returns></returns>
        public QueryInfo IsNull(string facetName)
        {
            return this.Where(facetName, FacetOperation.IsNull, null);
        }

        /// <summary>
        /// Adds a Where with Is Not Null
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <returns></returns>
        public QueryInfo IsNotNull(string facetName)
        {
            return this.Where(facetName, FacetOperation.IsNotNull, null);
        }

        /// <summary>
        /// Adds a Where with Less Than
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public QueryInfo LessThan(string facetName, string value)
        {
            return this.Where(facetName, FacetOperation.LessThan, value);
        }

        /// <summary>
        /// Adds a Where with Less Equal
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public QueryInfo LessEqual(string facetName, string value)
        {
            return this.Where(facetName, FacetOperation.LessEqual, value);
        }

        /// <summary>
        /// Adds a Where with Greater Equal
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public QueryInfo GreaterEqual(string facetName, string value)
        {
            return this.Where(facetName, FacetOperation.GreaterEqual, value);
        }

        /// <summary>
        /// Adds a Where with Greater Than
        /// </summary>
        /// <param name="facetName">Name of the facet.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public QueryInfo GreaterThan(string facetName, string value)
        {
            return this.Where(facetName, FacetOperation.GreaterThan, value);
        }

        #endregion
    }
}
