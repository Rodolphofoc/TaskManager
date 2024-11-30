using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Applications.Abstracts
{
    public abstract class PagedRequestBase<TModel> : IRequest<Paged<TModel>?>, IBaseRequest where TModel : class
    {
        [FromQuery]
        public int CurrentPage { get; set; }
        [FromQuery]
        public int PageSize { get; set; }
        [FromQuery]
        public int RecordsInPage { get; set; }
        [FromQuery]
        public int TotalRecords { get; set; }
    }
}
