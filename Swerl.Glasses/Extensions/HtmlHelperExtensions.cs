using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Swerl.Glasses.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static void DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var options = (IList<SelectListItem>) metaData.AdditionalValues["Options"];
            htmlHelper.DropDownListFor(expression,options);
        }
    }
}
