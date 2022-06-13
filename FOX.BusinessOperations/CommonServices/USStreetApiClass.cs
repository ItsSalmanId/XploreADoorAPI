using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SmartyStreets;
using SmartyStreets.USStreetApi;
using FOX.DataModels.Models.CommonModel;
using System.Net;

namespace FOX.BusinessOperations.CommonServices
{
    public class USStreetApiClass
    {
        public USStreetApiClass()
        {
        }
        public List<ZipCityStateAddress> GetDataUSStreetApi(Lookup lookup)
        {
            string authId = "e01bd070-93cd-396c-aad8-b22cfa4d24d3";
            string authToken = "wMxF3SExbqkZyMosMnWf";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new ClientBuilder(authId, authToken)
                        // uncomment the line below to point to the specified proxy.
                        //.ViaProxy("http://localhost:8080", "username", "password")
                        .BuildUsStreetApiClient();
            try
            {
                client.Send(lookup);
            }
            catch (SmartyException)
            {
               throw;
            }
            catch (IOException)
            {
                throw;
            }

            var candidates = lookup.Result;
            //If there is no 9 Digit Zip Code Against the given address then return the exiting address 
            if (candidates.Count == 0)
            {
                List<ZipCityStateAddress> listF = new List<ZipCityStateAddress>();
                ZipCityStateAddress objFailure = new ZipCityStateAddress();

                objFailure.ADDRESS = lookup.Street;
                objFailure.CITY_NAME = lookup.City;
                objFailure.STATE_CODE = lookup.State;
                objFailure.ZIP_CODE = lookup.ZipCode;
                objFailure.TIME_ZONE = "";
                listF.Add(objFailure);

                return listF;
            }

            //If there is 9 Digit Zip Code Against the given address then return the New address
            var firstCandidate = candidates[0];
            List<ZipCityStateAddress> list = new List<ZipCityStateAddress>();
            ZipCityStateAddress obj = new ZipCityStateAddress();
          
            obj.ADDRESS = (firstCandidate.Components.PrimaryNumber ?? "")+" "+(firstCandidate.Components.StreetPredirection ?? "")+ " " + (firstCandidate.Components.StreetName ?? "")+ " " +
                (firstCandidate.Components.StreetSuffix ?? "") + " " + (firstCandidate.Components.SecondaryDesignator ?? "") + " " + (firstCandidate.Components.SecondaryNumber ?? "");
            obj.CITY_NAME = (firstCandidate.Components.CityName ?? "");
            obj.STATE_CODE = (firstCandidate.Components.State ?? "");
            obj.ZIP_CODE = (firstCandidate.Components?.ZipCode ?? "") + "-" + (firstCandidate.Components?.Plus4Code ?? "");
            obj.TIME_ZONE = (firstCandidate.Metadata.TimeZone ?? "");
            list.Add(obj);

            return list;
        }
    }
}
