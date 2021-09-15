using MatechEssential;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using API.Controllers;
using MatechEssential.Security;

namespace API.Security
{
    public class AuthenticationFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {

                try
                {
                    string token = filterContext.HttpContext.Request.Headers["Authorization"];
                    if (string.IsNullOrEmpty(token))
                        throw new Exception("No Token Provided");
                    string[] cred = Encoding.UTF8.GetString(Convert.FromBase64String(token.Replace("Basic ",""))).Split(':');
                    SQLHelper sql = SQLHelper.Initialize();
                    Dictionary<string, object> pars = new Dictionary<string, object>();
                    pars.Add("_UserName", cred[0]);
                    DataTable dt = sql.ExecuteSP("AuthAPI", pars).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Password"].ToString() != Encrypt.EncryptString(cred[1],"tariq@123"))
                        {
                            filterContext.Result = APIResponse.ErrorResponse(APIResponse.APIErrors.InvalidCredentials);
                        }
                    }
                    else
                    {
                        filterContext.Result = APIResponse.ErrorResponse(APIResponse.APIErrors.InvalidCredentials);
                    }
//                    filterContext.Controller.TempData["UserId"] = dt.Rows[0]["UserId"];
                    ((BaseController)filterContext.Controller).UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                }
                catch (Exception ex)
                {
                    filterContext.Result = APIResponse.ErrorResponse(APIResponse.APIErrors.InvalidCredentials);
                }
            }
        }
    }
}