
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using VdbAPI.MetaData;

namespace VdbAPI.Models {
    [ModelMetadataType(typeof(ArticleMetaData))]
    public partial class Article {
    }
}
