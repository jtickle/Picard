using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using LibEDJournal.State;

namespace Picard.Lib
{
    /// <summary>
    /// Convenience functions for working with Inara.cz.  It's mostly HTTP form posts.
    /// </summary>
    public class InaraApi : IDisposable
    {
        /// <summary>
        /// Are we currently logged into Inara?
        /// </summary>
        public bool isAuthenticated { get; private set; }

        /// <summary>
        /// What is the commander's name according to Inara?
        /// </summary>
        public string cmdrName { get; private set; }

        /// <summary>
        /// What was the last error seen?
        /// </summary>
        public string lastError { get; private set; }

        /// <summary>
        /// The sesison cookies used to maintain Inara authentication
        /// </summary>
        public CookieContainer cookies { get; private set; }

        /// <summary>
        /// A lookup table of material names to Inara IDs
        /// </summary>
        public IDictionary<string, int> MaterialInaraIDLookup;

        /// <summary>
        /// There are some places where it is convenient to get the Materials without
        /// making unnecessary requests, and we will store them here.
        /// </summary>
        public InventorySet MaterialsCache;

        /// <summary>
        /// The HTTPClient instance used to speak with Inara
        /// </summary>
        private HttpClient http;

        /// <summary>
        /// The location to which to post Authentication credentials
        /// </summary>
        private const string AuthURL = "http://inara.cz/login";

        /// <summary>
        /// The name of the username field
        /// </summary>
        private const string UserField = "loginid";

        /// <summary>
        /// The name of the password field
        /// </summary>
        private const string PassField = "loginpass";

        /// <summary>
        /// The action to post, in this case, login
        /// </summary>
        private const string AuthAction = "ENT_LOGIN";

        /// <summary>
        /// The location to redirect to after logging in
        /// </summary>
        private const string AuthLocation = "intro";

        private const string PostLocation = "cmdr-cargo";

        private const string PostAction = "USER_CARGOMATERIAL_SET";

        /// <summary>
        /// The location to get the materials sheet
        /// </summary>
        private const string MatsSheetURL = "http://inara.cz/cmdr-cargo/";

        /// <summary>
        /// The Inara Cookie URI
        /// </summary>
        private const string CookieUri = "http://inara.cz/";

        /// <summary>
        /// The standard Inara form aciton field
        /// </summary>
        private const string ActField = "formact";
        
        /// <summary>
        /// The standard Inara form location redirect field
        /// </summary>
        private const string LocField = "location";

        public InaraApi()
        {
            // Mostly initialize stuff that will be used later
            MaterialInaraIDLookup = new Dictionary<string, int>();
            lastError = "";
            cmdrName = "CMDR GUEST";

            // Create HTTP Client and tell it to notify us if there are cookies
            cookies = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            http = new HttpClient(handler);
            isAuthenticated = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool nativeOnly)
        {
            if(!nativeOnly)
            {
                http.Dispose();
            }
        }

        /// <summary>
        /// Make a Form Post on Inara.cz
        /// </summary>
        /// <param name="URL">The URL to post form data to</param>
        /// <param name="values">A dictionary of keys and values to post</param>
        /// <returns>Asynchronously, the HTTP Response</returns>
        public async Task<HttpResponseMessage> FormPost(string URL, Dictionary<string,string> values)
        {
            // Make the Requests
            var response = await http.PostAsync(URL,
                new FormUrlEncodedContent(values));

            // TODO: check response.StatusCode and handle

            return response;
        }

        /// <summary>
        /// Makes an HTTP GET on Inara.cz
        /// </summary>
        /// <param name="URL">The URL to GET</param>
        /// <returns>Asynchronously, the HTTP Response</returns>
        public async Task<HttpResponseMessage> DoGet(string URL)
        {
            var response = await http.GetAsync(URL);

            // TODO: check for errors

            return response;
        }

        /// <summary>
        /// Parses an HTML document using our HTML parser instance
        /// </summary>
        /// <param name="response">An HTTP response to parse</param>
        /// <returns>Asynchronously, an instance of the parsed HTML DOM</returns>
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

        /// <summary>
        /// Finds the first instance of a DOM node that contains an attribute value
        /// </summary>
        /// <param name="dom">The parsed HTML DOM object</param>
        /// <param name="key">The attribute to look for</param>
        /// <param name="value">The value that the attribute should have</param>
        /// <returns>The first HTML node that matches</returns>
        public HtmlNode findFirstNodeByAttributeValue(HtmlDocument dom, string key, string value)
        {
            var ret = findAllNodeByAttributeValue(dom, key, value);

            if(ret.Count() > 0)
            {
                return ret.First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all DOm nodes that contain an attribute value
        /// </summary>
        /// <param name="dom">The parsed HTML DOM object</param>
        /// <param name="key">The attribute to look for</param>
        /// <param name="value">The value that the attribute should have</param>
        /// <returns>All HTML nodes that match</returns>
        public IEnumerable<HtmlNode> findAllNodeByAttributeValue(HtmlDocument dom, string key, string value)
        {
            return dom.DocumentNode.DescendantsAndSelf().Where(node =>
            {
                foreach (var attr in node.Attributes.AttributesWithName(key))
                {
                    if (attr.Value.Contains(value))
                    {
                        return true;
                    }
                }
                return false;
            });
        }

        protected async Task<InventorySet> parseMaterialsSheet(HttpResponseMessage response)
        {
            var found = new InventorySet();

            var dom = await ParseHtml(response);

            // Look for all nodes with class inventorymaterial
            foreach (var node in findAllNodeByAttributeValue(dom, "class", "inventorymaterial"))
            {
                // The name is in the span
                var mat = node.Descendants("span").First().InnerText;

                // If we have already found this, ignore
                if (found.ContainsKey(mat)) continue;

                // The count is in a form input
                var value = node.Descendants("input").First().GetAttributeValue("value", 0);
                
                // Add the material to be returned
                found.Add(mat, value);

                try
                {
                    // Add the material and its ID to the Material ID lookup for later use
                    // We do this on every page load because no reason not to, and if they
                    // change their IDs for some reason, shouldn't hurt us
                    var inputName = node.Descendants("input").First().GetAttributeValue("name", "playerinv[-1]");
                    if (!inputName.StartsWith("playerinv["))
                    {
                        Console.WriteLine("DOM Error: " + inputName);
                        continue;
                    }
                    var match = Regex.Match(inputName, @"playerinv\[(\w+)\]", RegexOptions.IgnoreCase);
                    if (match.Groups.Count < 2)
                    {
                        Console.WriteLine("Regex Error:");
                        Console.WriteLine(match.ToString());
                        continue;
                    }
                    Console.WriteLine("Matched: " + match.Groups[1].Captures[0].ToString());
                    var num = int.Parse(match.Groups[1].Captures[0].ToString());

                    Console.WriteLine("Storing: Lookup[" + mat + "] = " + num);
                    if(MaterialInaraIDLookup.ContainsKey(mat))
                    {
                        MaterialInaraIDLookup[mat] = num;
                    }
                    else
                    {
                        MaterialInaraIDLookup.Add(mat, num);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    throw e;
                }
            }

            MaterialsCache = found;
            
            return found;
        }

        /// <summary>
        /// Perform authentication at Inara.cz
        /// </summary>
        /// <param name="user">The User's Username</param>
        /// <param name="pass">The User's Password</param>
        /// <returns>A bookean representing the success of the authentication</returns>
        public async Task<bool> Authenticate(string user, string pass)
        {
            // The values we are going to pass
            var values = new Dictionary<string, string>
            {
                { UserField, user },
                { PassField, pass },
                { ActField, AuthAction },
                { LocField, PostLocation }
            };

            // POST the Form
            var response = await FormPost(AuthURL, values);

            // Parse the Response
            var dom = await ParseHtml(response);

            // Look for the Commander's Name
            var nameNode = findFirstNodeByAttributeValue(dom, "class", "menuappend");
            cmdrName = nameNode.InnerText;

            if (cmdrName == "CMDR Guest")
            {
                // If 'CMDR Guest' still logged in, probably invalid creds
                var errorNode = findFirstNodeByAttributeValue(dom, "class", "loginformbottom");
                if (errorNode == null)
                {
                    if (pass == "")
                    {
                        lastError = "No Password Specified";
                    }
                    else
                    {
                        lastError = "Invalid username or password.  You can also use your email address as username.";
                    }
                }
                else
                {
                    lastError = errorNode.InnerText;
                }
                isAuthenticated = false;
                return false;
            }
            else
            {
                // Otherwise, we're logged in!
                isAuthenticated = true;
                await parseMaterialsSheet(response);
                return true;
            }
        }

        /// <summary>
        /// Finds a cookie within cookies
        /// </summary>
        /// <param name="name">The name of the cookie</param>
        /// <returns>The value of the cookie</returns>
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

        /// <summary>
        /// Loads the Materials Sheet from Inara.cz
        /// </summary>
        /// <returns>A dictionary of materials and counts</returns>
        public async Task<InventorySet> GetMaterialsSheet()
        {
            // If this is cached, return it
            if(MaterialsCache != null)
            {
                return MaterialsCache;
            }
            // Perform the GET to retrieve the materisl page
            var response = await DoGet(MatsSheetURL + GetEliteSheet());

            // Parse the HTML
            return await parseMaterialsSheet(response);
        }

        /// <summary>
        /// Clear the materials cache so that on the next call to
        /// GetMaterialsSheet, it will go fetch from Inara
        /// </summary>
        public void ClearMaterialsCache()
        {
            MaterialsCache = null;
        }

        public async Task<InventorySet> PostMaterialsSheet(InventorySet totals)
        {
            // Invalidate the Cache
            MaterialsCache = null;

            var postData = new Dictionary<string, string>();
            
            await GetMaterialsSheet();

            foreach (var mat in totals.Keys)
            {
                if(!MaterialInaraIDLookup.Keys.Contains(mat))
                {
                    throw new Exception("Material '" + mat + "' does not exist on Inara");
                }

                postData.Add("playerinv[" + MaterialInaraIDLookup[mat] + "]", totals[mat].ToString());
            }

            postData.Add(LocField, PostLocation);
            postData.Add(ActField, PostAction);

            foreach(var entry in postData)
            {
                Console.Write("Would POST:\t" + entry.Key + "\t" + entry.Value + "\n");
            }

            // POST the form
            var response = await FormPost(MatsSheetURL + GetEliteSheet(), postData);

            // Parse the DOM
            return await parseMaterialsSheet(response);
        }

        /// <summary>
        /// Return the cookie value of the user's internal ID in Inara.cz
        /// </summary>
        /// <returns>
        /// The user's internail Inara ID
        /// </returns>
        public string GetEliteSheet()
        {
            return FindCookie("elitesheet");
        }

        /// <summary>
        /// Gets the user's session ID in Inara.cz
        /// </summary>
        /// <returns>
        /// The user's Inara.cz session ID
        /// </returns>
        public string GetESID()
        {
            return FindCookie("esid");
        }
    }
}
