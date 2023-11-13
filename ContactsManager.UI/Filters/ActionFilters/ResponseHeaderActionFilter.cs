using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ActionFilters
{
    public class ResponseHeaderFilterFactory : Attribute, IFilterFactory
    {
        private readonly string _key;
        private readonly string _value;

        public int Order { get;}

        public ResponseHeaderFilterFactory(string key, string value, int order)
        {
            _key = key;
            _value = value;
            Order = order;
        }
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            //return filter object
            var filter= new ResponseHeaderActionFilter(_key, _value, Order);
            return filter;
        }
    }

    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        

        private readonly string _key;
        private readonly string _value;

        public int Order {  get; set; }

        public ResponseHeaderActionFilter(string key, string value, int order)
        {
            _key = key;
            _value = value;
            Order = order;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
         
            await next();
            context.HttpContext.Response.Headers[_key] = _value;

        }
    }
}
