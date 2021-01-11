using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Entities;
using System;
using LiteDB;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace UrlShortener.Infrastructure.Repositories
{
    public class UrlShortenerRepository : IUrlShortenerRepository
    {
        private readonly ILiteDatabase _liteDatabase;
        private readonly ILiteCollection<ShortLink> _shortLinksContext;
        public UrlShortenerRepository(ILiteDatabase liteDatabase)
        {
            _liteDatabase = liteDatabase;
            _shortLinksContext = _liteDatabase.GetCollection<ShortLink>(nameof(ShortLink));
        }

        public ShortLink GetUrlById(Guid id)
        {
            return _shortLinksContext.Find(x => x.Id == id).First();
        }
        public IEnumerable<ShortLink> Get(Expression<Func<ShortLink, bool>> predicate)
        {
            return _shortLinksContext.Find(predicate);
        }

        public void CreateUrl(ShortLink urlModel)
        {
            _shortLinksContext.Insert(urlModel);
        }
    }
}
