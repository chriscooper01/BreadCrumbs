using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreadCrumb.Pages.Extended
{
    public class JsonStringHelper
    {

        public static string GetString(object payload)
        {
            if (payload == null)
                return String.Empty;


            return Newtonsoft.Json.JsonConvert.SerializeObject(payload);
        }


        public static object GetObject(string jsonString, Type objectType)
        {
            if (String.IsNullOrWhiteSpace(jsonString))
                return null;


            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString, objectType);
        }


    }
}
