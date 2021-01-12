using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Newtonsoft.Json;
using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Models.LDAPUser;
using System.Linq;

namespace FoxRehabilitationAPI.App_Start
{
    public class LDAPClient
    {
        LDAPUser GetUserInformation(DirectoryEntry de)
        {
            LDAPUser user = new LDAPUser();

            if (de != null && de.Properties != null && de.Properties.Count > 0)
            {
                if (de.Properties.Contains("givenName"))
                {
                    user.FirstName = de.Properties["givenName"].Value == null ? string.Empty : de.Properties["givenName"].Value.ToString();
                }
                if (de.Properties.Contains("sn"))
                {
                    user.LastName = de.Properties["sn"].Value == null ? string.Empty : de.Properties["sn"].Value.ToString();
                }
                if (de.Properties.Contains("sAMAccountName"))
                {
                    user.FullName = de.Properties["sAMAccountName"].Value == null ? string.Empty : de.Properties["sAMAccountName"].Value.ToString();
                }
                if (de.Properties.Contains("title"))
                {
                    user.Title = de.Properties["title"].Value == null ? string.Empty : de.Properties["title"].Value.ToString();
                }
                if (de.Properties.Contains("extensionAttribute15"))
                {
                    user.ExtensionAttribute = de.Properties["extensionAttribute15"].Value == null ? string.Empty : de.Properties["extensionAttribute15"].Value.ToString();
                }
                if (de.Properties.Contains("employeeType"))
                {
                    user.Type = de.Properties["employeeType"].Value == null ? string.Empty : de.Properties["employeeType"].Value.ToString();
                }
                if (de.Properties.Contains("employeeID"))
                {
                    user.ID = de.Properties["employeeID"].Value == null ? string.Empty : de.Properties["employeeID"].Value.ToString();
                }
                if (de.Properties.Contains("mail"))
                {
                    user.Email = de.Properties["mail"].Value == null ? string.Empty : de.Properties["mail"].Value.ToString();
                }
                if (de.Properties.Contains("division"))
                {
                    user.Division = de.Properties["division"].Value == null ? string.Empty : de.Properties["division"].Value.ToString();
                }
                if (de.Properties.Contains("employeeNumber"))
                {
                    user.employeeNumber = de.Properties["employeeNumber"].Value == null ? string.Empty : de.Properties["employeeNumber"].Value.ToString();
                }
                de.Dispose();
            }
            //user.Title = "CLINICIAN";
            return user;
        }

        public LDAPUser GetADUser(string domain, string email, string userName, string password)
        {
            try
            {
                GetADUserResViewModel getADUserResViewModel = GetADUserWS(new GetADUserReqViewModel() { Domain = domain, Email = email, UserName = userName, Password = password }).Result;
                if (getADUserResViewModel != null && (getADUserResViewModel?.IsSussess ?? false))
                {
                    if (getADUserResViewModel.lDAPUser != null)
                    {
                        Helper.LogADLoginStatus(userName, "Success_WS", string.Empty);
                        return getADUserResViewModel.lDAPUser;
                    }
                    else
                    {
                        Helper.LogADLoginStatus(userName, "Fail", "Invalid Credential");
                        return null;
                    }
                }
                else
                {
                    throw getADUserResViewModel?.Exception;
                }
            }
            catch (Exception ex)
            {
                Helper.LogADLoginStatus(userName, "Fail_WS", "Exception occurred");
                Helper.SendEmail(AppConfiguration.SendADExceptionTo, "AD WS Exception: " + ex.Message + Environment.NewLine + "User Name: " + userName + ", Domain: " + domain, Helper.LogADException(ex), null, null, AppConfiguration.SendADExceptionCCList);

                try
                {
                    return GetADUser_Portal(domain, email, userName, password);
                }
                catch (Exception exception)
                {
                    Helper.LogADLoginStatus(userName, "Fail", "Exception occurred");
                    Helper.SendEmail(AppConfiguration.SendADExceptionTo, "AD Exception: " + exception.Message + Environment.NewLine + "User Name: " + userName + ", Domain: " + domain, Helper.LogADException(exception), null, null, AppConfiguration.SendADExceptionCCList);
                    return null;
                }
            }
        }

        public LDAPUser GetADUser_Portal(string domain, string email, string userName, string password)
        {
            //using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain))
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain, userName, password))
            {
                if (ctx != null)
                {
                    //bool isValid = ctx.ValidateCredentials(userName, password);
                    bool isValid = ctx.ValidateCredentials(userName, password, ContextOptions.Negotiate);
                    if (isValid)
                    {
                        using (UserPrincipal up = UserPrincipal.FindByIdentity(ctx, userName))
                        {
                            if (up != null)
                            {
                                using (DirectoryEntry de = (up.GetUnderlyingObject() as DirectoryEntry))
                                {
                                    if (de != null)
                                    {
                                        Helper.LogADLoginStatus(userName, "Success", string.Empty);
                                        //LogProperties(de);
                                        return GetUserInformation(de);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            return GetADUser_Portal_SearchByEmail(domain, email, userName, password);
                        }
                    }
                }
            }
            Helper.LogADLoginStatus(userName, "Fail", "Invalid Credential");
            return null;
        }

        public LDAPUser GetADUser_Portal_SearchByEmail(string domain, string email, string userName, string password)
        {
            using (DirectoryEntry adEntry = new DirectoryEntry("LDAP://" + domain, "test4.mtbc", "2018@Fox"))
            {
                if (adEntry != null)
                {
                    using (DirectorySearcher adSearcher = new DirectorySearcher(adEntry))
                    {
                        adSearcher.Filter = ("mail=" + email);
                        SearchResult user = adSearcher.FindOne();
                        using (DirectoryEntry de = user?.GetDirectoryEntry())
                        {
                            string samAccountName = string.Empty;
                            if (de.Properties.Contains("samAccountName"))
                            {
                                samAccountName = de.Properties["samAccountName"].Value == null ? string.Empty : de.Properties["samAccountName"].Value.ToString();
                            }
                            //string samAccountName = de?.Properties["samAccountName"].Value?.ToString() ?? string.Empty;
                            if (!string.IsNullOrEmpty(samAccountName))
                            {
                                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain, samAccountName, password))
                                {
                                    if (ctx != null)
                                    {
                                        bool isValid = ctx.ValidateCredentials(samAccountName.ToLower(), password, ContextOptions.Negotiate);
                                        if (isValid)
                                        {
                                            using (UserPrincipal up = UserPrincipal.FindByIdentity(ctx, samAccountName))
                                            {
                                                if (up != null)
                                                {
                                                    using (DirectoryEntry dir = (up.GetUnderlyingObject() as DirectoryEntry))
                                                    {
                                                        if (dir != null)
                                                        {
                                                            Helper.LogADLoginStatus(samAccountName, "Success", string.Empty);                                                        //LogProperties(de);
                                                            return GetUserInformation(dir);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Helper.LogADLoginStatus(userName, "Fail", "Invalid Credential");
            return null;
        }

        async Task<GetADUserResViewModel> GetADUserWS(GetADUserReqViewModel getADUserReqViewModel)
        {

            string url;
            //string url = "https://mhealth.mtbc.com/fox_test/api/ADLogin/GetADUser";
            //string url = "https://mhealth.mtbc.com/fox/api/ADLogin/GetADUserV1";
            if (getADUserReqViewModel.Domain == "40.143.53.71")
            {
                url = "https://mhealth.mtbc.com/fox_test/api/ADLogin/GetADUserV2";
            }
            else
            {
                url = "https://mhealth.mtbc.com/fox_test/api/ADLogin/GetADUserWithCustomDomain";
            }

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessTokenInfo.token_type, accessTokenInfo.access_token);

                var json = JsonConvert.SerializeObject(getADUserReqViewModel);

                var contentType = new StringContent(json, Encoding.UTF8, "application/json");
                var responseMessage = Task.Run(async () => { return await client.PostAsync("", contentType); }).Result;
                
                using (HttpContent content = responseMessage.Content)
                {
                    string result = await content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GetADUserResViewModel>(result);
                }
            }
        }

        void LogProperties(DirectoryEntry de)
        {
            if (de != null && de.Properties != null && de.Properties.Count > 0)
            {
                PropertyCollection p = de.Properties;
                string email;
                Dictionary<string, string> properties = new Dictionary<string, string>();
                if (de.Properties.Contains("mail"))
                {
                    email = de.Properties["mail"].Value == null ? "No email provide" : de.Properties["mail"].Value.ToString();
                }
                else
                {
                    email = "No email provide";
                }
                foreach (string pName in p.PropertyNames)
                {
                    string Value = de.Properties[pName].Value == null ? string.Empty : de.Properties[pName].Value.ToString();
                    properties.Add(pName, Value);
                }
                Helper.LogADProperties(Email: email, Properties: properties);
                //de.Dispose();
            }
        }
    }
}
