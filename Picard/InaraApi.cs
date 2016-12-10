using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;

namespace Picard
{
    public class InaraApi
    {
        public bool isAuthenticated { get; private set; }
        public string cmdrName { get; private set; }
        public string lastError { get; private set; }
        public CookieContainer cookies { get; private set; }

        private HttpClient http;

        private const string AuthURL = "http://inara.cz/login";
        private const string UserField = "loginid";
        private const string PassField = "loginpass";
        private const string AuthAction = "ENT_LOGIN";
        private const string AuthLocation = "intro";

        private const string MatsSheetURL = "http://inara.cz/cmdr-cargo/";

        private const string CookieUri = "http://inara.cz/";
        private const string ActField = "formact";
        private const string LocField = "location";

        public InaraApi()
        {
            cookies = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            http = new HttpClient(handler);
            isAuthenticated = false;
            lastError = "";
            cmdrName = "CMDR GUEST";
            // TODO: Make MSHTML
        }

        public async Task<HttpResponseMessage> FormPost(string URL, Dictionary<string,string> values)
        {
            // Make the Requests
            var response = await http.PostAsync(URL,
                new FormUrlEncodedContent(values));

            // TODO: check response.StatusCode and handle

            return response;
        }

        public async Task<HttpResponseMessage> DoGet(string URL)
        {
            var response = await http.GetAsync(URL);

            // TODO: check for errors

            return response;
        }

        public async Task<HtmlDocument> ParseHtml(HttpResponseMessage response)
        {
            // Decode Response String
            string source = Encoding.GetEncoding("utf-8")
                .GetString(await response.Content.ReadAsByteArrayAsync());
            source = WebUtility.HtmlDecode(source);

            // Parse DOM
            HtmlDocument result = new HtmlDocument();
            result.LoadHtml(source);

            return result;
        }

        public HtmlNode findFirstNodeByAttributeValue(HtmlDocument dom, string key, string value)
        {
            foreach(var node in dom.DocumentNode.DescendantsAndSelf())
            {
                foreach (var attr in node.Attributes.AttributesWithName(key))
                {
                    if (attr.Value.Contains(value))
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        public IEnumerable<HtmlNode> findAllNodeByAttributeValue(HtmlDocument dom, string key, string value)
        {
            return dom.DocumentNode.DescendantsAndSelf().Where(node =>
            {
                foreach (var attr in node.Attributes.AttributesWithName(key))
                {
                    if(attr.Value.Contains(value))
                    {
                        return true;
                    }
                }
                return false;
            });
        }

        public async Task<bool> Authenticate(string user, string pass)
        {
            var values = new Dictionary<string, string>
            {
                { UserField, user },
                { PassField, pass },
                { ActField, AuthAction },
                { LocField, AuthLocation }
            };

            var response = await FormPost(AuthURL, values);

            var dom = await (ParseHtml(response));

            var nameNode = findFirstNodeByAttributeValue(dom, "class", "menuappend");
            cmdrName = nameNode.InnerText;

            // If 'CMDR Guest' still logged in, probably invalid creds
            if (cmdrName == "CMDR Guest")
            {
                var errorNode = findFirstNodeByAttributeValue(dom, "class", "loginformbottom");
                lastError = errorNode.InnerText;
                isAuthenticated = false;
                return false;
            }
            else
            {
                isAuthenticated = true;
                return true;
            }
        }

        public string FindCookie(string name)
        {
            var cookieJar = cookies.GetCookies(new Uri(CookieUri)).Cast<Cookie>();
            foreach (Cookie c in cookieJar)
            {
                if (c.Name == name)
                {
                    return c.Value;
                }
            }

            return null;
        }

        public async Task<IDictionary<string, int>> GetMaterialsSheet()
        {
            var found = new Dictionary<string, int>();

            var response = await DoGet(MatsSheetURL + GetEliteSheet());

            var dom = await ParseHtml(response);

            foreach(var node in findAllNodeByAttributeValue(dom, "class", "inventorymaterial"))
            {
                var mat = node.Descendants("span").First().InnerText;
                var value = node.Descendants("input").First().GetAttributeValue("value", 0);

                if (found.ContainsKey(mat)) continue;

                found.Add(mat, value);
            }

            return found;
        }

        public string GetEliteSheet()
        {
            return FindCookie("elitesheet");
        }

        public string GetESID()
        {
            return FindCookie("esid");
        }
    }
}
