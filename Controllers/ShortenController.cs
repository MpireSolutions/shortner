using MatechEssential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API.Controllers
{
    public class ShortenController : BaseController
    {
        [HttpPost]
        public ActionResult Create(string Url)
        {
            string shortUrl = "";
            if (string.IsNullOrEmpty(Url))
            {
                return APIResponse.ErrorResponse(APIResponse.APIErrors.BadRequest, "Url is required");
            }
            try
            {
                SQLHelper sql = SQLHelper.Initialize();
                Dictionary<string, object> pars = new Dictionary<string, object>();
                pars.Add("_LongUrl", Url);
                pars.Add("_UserId", UserId);
                shortUrl = sql.ExecuteSPScalar("createUrl", pars).ToString();
            }catch(Exception ex)
            {
                return APIResponse.ErrorResponse(APIResponse.APIErrors.BadRequest, ex.Message);
            }
            return APIResponse.Success(new { ShortUrl = shortUrl });
        }
        public ActionResult CreateBulk(string[] Urls)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            if (Urls.Length==0)
            {
                return APIResponse.ErrorResponse(APIResponse.APIErrors.BadRequest, "at least one URL is required");
            }
            try
            {
                SQLHelper sql = SQLHelper.Initialize();
                Dictionary<string, object> pars = new Dictionary<string, object>();
                pars.Add("_UserId", UserId);
                foreach (string Url in Urls)
                {
                    pars.Add("_LongUrl", Url);
                    string shortUrl = sql.ExecuteSPScalar("createUrl", pars).ToString();
                    res.Add(Url, shortUrl);
                }
                pars.Remove("_LongUrl");
            }
            catch (Exception ex)
            {
                return APIResponse.ErrorResponse(APIResponse.APIErrors.BadRequest, ex.Message);
            }
            return APIResponse.Success(res);
        }
    }
}