using Backstage.Models.MetaData;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backstage.Models {
    [ModelMetadataType(typeof(ArticleMetaData))]
    public partial class Article {
    }
}
