using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SoftwarePr.InterFaces
{
    interface IRedirectControllers
    {
        ActionResult RedirectToAnotherController();
    }
    interface IRedirectUserControllers
    {
        ActionResult RedirectToAdminController();
    }
    interface IRedirectAdminControllers
    {
        ActionResult RedirectToProductsController();
        ActionResult RedirectingLoginAdmin();
        ActionResult RedirectingIndex();

    }
}
