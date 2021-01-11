using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Interfaces
{
    public interface IUrlShortenerRepository
    {
        ShortLink GetUrlById(Guid id);
        void CreateUrl(ShortLink urlModel);
        IEnumerable<ShortLink> Get(Expression<Func<ShortLink, bool>> predicate);
    }
}
