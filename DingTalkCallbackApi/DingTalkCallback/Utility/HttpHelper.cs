using HttpCode.Core;
namespace Utility
{
    public class HttpHelper
    {
        HttpHelper http = new HttpHelper();
        public static HttpResults Get(string url)
        {
            HttpHelpers http = new HttpHelpers();
            HttpItems item = new HttpItems();
            item.Url = url;
            HttpResults hr = http.GetHtml(item);
            return hr;
        }

        public static HttpResults Get(string url, string type)
        {
            HttpHelpers http = new HttpHelpers();
            HttpItems item = new HttpItems();
            item.ContentType = type;
            item.Url = url;
            HttpResults hr = http.GetHtml(item);
            return hr;
        }
        public static HttpResults Post(string url, string param)
        {
            HttpHelpers http = new HttpHelpers();
            HttpItems item = new HttpItems();

            string PostD = string.Format("{0}", param);
            item.Url = url;
            item.Method = "Post";
            item.ResultType = ResultType.String;
            item.Postdata = PostD;
            HttpResults hr = http.GetHtml(item);
            return hr;
        }

        public static HttpResults Post(string url, string param, string type)
        {
            HttpHelpers http = new HttpHelpers();
            HttpItems item = new HttpItems();

            string PostD = string.Format("{0}", param);
            item.Url = url;
            item.ContentType = type;
            item.Method = "Post";
            item.ResultType = ResultType.String;
            item.Postdata = PostD;
            HttpResults hr = http.GetHtml(item);
            return hr;
        }
    }

}


