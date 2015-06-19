
namespace SocialNetwork.Dal.Interface
{
    /// <summary>
    /// Represent some transaction for work with storage.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// End transaction and commit.
        /// </summary>
        void Commit();
    }
}
