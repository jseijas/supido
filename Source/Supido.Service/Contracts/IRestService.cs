using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Supido.Service.Contracts
{
    /// <summary>
    /// Interface for a Rest Service.
    /// </summary>
    [ServiceContract]
    public interface IRestService
    {
        #region - Methods -

        /// <summary>
        /// Options Verb, for CORS
        /// </summary>
        [OperationContract]
        void GetOptions();

        /// <summary>
        /// Get Verb.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [OperationContract]
        Message Get(Message message);

        /// <summary>
        /// Post Verb
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [OperationContract]
        Message Post(Message message);

        /// <summary>
        /// Put Verb
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [OperationContract]
        Message Put(Message message);

        /// <summary>
        /// Delete Verb.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [OperationContract]
        Message Delete(Message message);

        #endregion
    }
}
