using Newtonsoft.Json;
using System;
using System.Collections;
using System.Net;
using System.Web.Mvc;

namespace Precedente.Controllers
{
    public class ControllerExtensions : Controller
    {
        protected enum ContentTypeResponse
        {
            XML,
            JSON
        }

        protected ActionResult Ok()
        {
            return new EmptyResult();
        }

        protected ActionResult Ok<T>(T data)
        {
            return GetContent(data, ContentTypeResponse.JSON);
        }

        protected ActionResult Ok<T>(T data, ContentTypeResponse contentType)
        {
            return GetContent(data, contentType);
        }

        protected ActionResult Ok(string message)
        {
            return Ok(message, ContentTypeResponse.JSON);
        }

        protected ActionResult Ok(string message, ContentTypeResponse contentType)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;

            return GetContent(new { Message = message }, contentType);
        }

        protected ActionResult InternalServerError()
        {
            return InternalServerError(ContentTypeResponse.JSON);
        }

        protected ActionResult InternalServerError(string message)
        {
            return InternalServerError(message, ContentTypeResponse.JSON);
        }

        protected ActionResult InternalServerError(string message, ContentTypeResponse contentType)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return GetContent(new { Message = message }, contentType);
        }

        protected ActionResult InternalServerError(ContentTypeResponse contentType)
        {
            return InternalServerError("An error has ocurred.", contentType);
        }

        protected ActionResult InternalServerError(Exception ex)
        {
            return InternalServerError(ex, ContentTypeResponse.JSON);
        }

        protected ActionResult InternalServerError(Exception ex, ContentTypeResponse contentType)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return GetContent(new { Message = "An error has ocurred.", ExceptionMessage = ex.Message, StackTrace = ex.StackTrace, ExceptionType = ex.GetType().FullName }, contentType);
        }

        protected ActionResult BadRequest()
        {
            return BadRequest(ContentTypeResponse.JSON);
        }

        protected ActionResult BadRequest(string message)
        {
            return BadRequest(message, ContentTypeResponse.JSON);
        }

        protected ActionResult BadRequest(string message, ContentTypeResponse contentType)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return GetContent(new { Message = message }, contentType);
        }

        protected ActionResult BadRequest(ContentTypeResponse contentType)
        {
            return BadRequest("The request is invalid.", contentType);
        }

        protected ActionResult BadRequest<T>(T data)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return GetContent(data, ContentTypeResponse.JSON);
        }

        protected ActionResult BadRequest<T>(T data, ContentTypeResponse contentType)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return GetContent(data, contentType);
        }

        protected ActionResult Conflict()
        {
            return Conflict(ContentTypeResponse.JSON);
        }

        protected ActionResult Conflict(string message)
        {
            return Conflict(message, ContentTypeResponse.JSON);
        }

        protected ActionResult Conflict(string message, ContentTypeResponse contentType)
        {
            Response.StatusCode = (int)HttpStatusCode.Conflict;

            return GetContent(new { Message = message }, contentType);
        }

        protected ActionResult Conflict(ContentTypeResponse contentType)
        {
            return Conflict("The request is invalid.", contentType);
        }

        protected ActionResult Conflict<T>(T data)
        {
            Response.StatusCode = (int)HttpStatusCode.Conflict;

            return GetContent(data, ContentTypeResponse.JSON);
        }

        protected ActionResult Conflict<T>(T data, ContentTypeResponse contentType)
        {
            Response.StatusCode = (int)HttpStatusCode.Conflict;

            return GetContent(data, contentType);
        }

        private ContentResult GetContent<T>(T data, ContentTypeResponse contentType)
        {
            if (data == null)
                return Content("");

            try
            {
                if (Request.Headers["Accept"] != null && Request.Headers["Accept"].IndexOf("application/xml", StringComparison.Ordinal) > -1)
                    return Content(GetXml(data), "application/xml");

                switch (contentType)
                {
                    case ContentTypeResponse.JSON:
                        return Content(GetJson(data), "application/json");
                    case ContentTypeResponse.XML:
                        return Content(GetXml(data), "application/xml");
                    default:
                        return Content(string.Empty);
                }
            }
            catch
            {
                return GetContent(new { Message = data.ToString() }, contentType);
            }
        }

        private string GetJson<T>(T data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        private string GetXml<T>(T data)
        {
            var json = GetJson(data);

            if (data is IEnumerable)
                json = "{ \"" + data.GetType().GetGenericArguments()[0].Name + "\": " + json.Trim() + " }";


            return JsonConvert.DeserializeXmlNode(json, "Root").InnerXml;
        }
    }
}