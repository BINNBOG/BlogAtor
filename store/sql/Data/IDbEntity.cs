namespace BlogAtor.Store.Sql.Data;

using BlogAtor.Framework.Entity;

internal interface IDbEntity<ET> where ET : EntityBase
{
    public ET ToEntity();
}