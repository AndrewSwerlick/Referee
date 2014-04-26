using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Swerl.Glasses.Providers
{
    public interface IViewOptionsProvider
    {
        IList<SelectListItem> BuildOptions();
    }
}
