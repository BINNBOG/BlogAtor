namespace BlogAtor.API.Hub;

using BlogAtor.Framework.Entity;
using BlogAtor.Store.Abstrations;

internal class ChangeNotification<ET> where ET : EntityBase
{
    public ICollection<ET> Entities { get; set; }
    public StoreChangeType ChangeType { get; set; }
    public ChangeNotification(ICollection<ET> entities, StoreChangeType changeType)
    {
        Entities = entities;
        ChangeType = changeType;
    }
}