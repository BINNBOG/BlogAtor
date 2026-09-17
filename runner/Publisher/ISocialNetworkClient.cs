using System.Threading.Tasks;
using BlogAtor.Store.Sql.Data; 

namespace BlogAtor.Publisher
{
    public interface ISocialNetworkClient
    {
        string NetworkName { get; }
        Task<bool> PublishPostAsync(DbDataItem item);
    }
}