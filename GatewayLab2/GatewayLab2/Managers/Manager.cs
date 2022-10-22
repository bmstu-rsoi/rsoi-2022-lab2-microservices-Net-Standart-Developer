using System.Net;

namespace GatewayLab2.Managers
{
    public abstract class Manager
    {
        public string Host { get; private set; }

        public Manager(string host)
        {
            Host = host;
        }

        protected string GetResources(string path)
        {
            WebRequest request = WebRequest.Create($"http://{Host}/{path}");
            request.Credentials = CredentialCache.DefaultCredentials;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        protected ServiceAnswer Post(string path, string body)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(body);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://{Host}/{path}");
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.UserAgent = "gateway 1.0.0";

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            ;
            reader.Close();
            dataStream.Close();

            return new ServiceAnswer(response.StatusCode, responseFromServer);
            response.Close();
        }
    }

    public class ServiceAnswer
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Body { get; set; }

        public ServiceAnswer(HttpStatusCode statusCode, string body)
        {
            StatusCode = statusCode;
            Body = body;
        }
    }

    public class OperationResult
    {
        public bool Success { get; private set; }

        public string Message { get; private set; }

        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
