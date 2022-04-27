using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace SelfHost2.Models
{
    class CustomExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new InternalServerErrorResult(context.Request);
        }
    }
}
