using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MatechEssential
{
    public class APIResponse
    {


        public bool HasError { get; set; }
        public string ErrorType { get; set; }
        public object Data { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }

        public APIResponse()
        {
            HasError = false;
            ErrorType = "";
            ErrorCode = 0;
            Data = null;
        }

        public APIResponse(object data, string message ="")
        {
            Data = data;
            HasError = false;
            ErrorType = message;
        }

        public static JsonResult Success(JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            APIResponse ar = new APIResponse() { HasError = false };
            return new JsonResult() { Data = ar, JsonRequestBehavior = behavior };
        }

        public static JsonResult Success(object data, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            APIResponse ar = new APIResponse() { HasError = false , Data = data };
            return new JsonResult() { Data = ar, JsonRequestBehavior = behavior };
        }

        public static JsonResult ErrorResponse(APIErrors error)
        {
            APIResponse ar = new APIResponse() { ErrorType = error.ToString(), ErrorCode = (int)error, HasError = true };
            return new JsonResult() { Data = ar, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public static JsonResult ErrorResponse(APIErrors error, int errorCode, string errorDescription)
        {
            APIResponse ar = new APIResponse() { ErrorType = error.ToString(), HasError = true, ErrorDescription = errorDescription, ErrorCode= errorCode };
            return new JsonResult() { Data = ar, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public static JsonResult ErrorResponse(APIErrors error, string errorDescription)
        {
            APIResponse ar = new APIResponse() { ErrorType = error.ToString(), HasError = true, ErrorDescription = errorDescription };
            return new JsonResult() { Data =  ar, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        public enum APIErrors
        {
            ValidationError = 1,
            InvalidCredentials = 2,
            BadRequest = 3,
            MissingLocationInfo = 4,
            GeneralError =5
        }
    }


}