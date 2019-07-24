using CSV.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Http;

namespace CSV.API.Controllers
{
    public class CSVController : ApiController
    {
        List<ContactInfo> contactInfo;
        private void SetContactInfo(int length)
        {
            contactInfo = new List<ContactInfo>();
            for (int i = 0; i < length; i++)
            {
                contactInfo.Add(new ContactInfo() { Id = i, Firstname = $"ABC-{i}", Lastname = $"XYZ-{i}" });
            }
        }
        // GET: api/CSV/5
        public string Get(int id)
        {
            SetContactInfo(id);
            var result = ToCsv(",", contactInfo);
            return result;
        }
        
        public string ToCsv<T>(string separator, IEnumerable<T> objectlist)
        {
            Type t = typeof(T);
            PropertyInfo[] fields = t.GetProperties();

            string header = string.Join(separator, fields.Select(f => f.Name).ToArray());

            StringBuilder csvdata = new StringBuilder();
            csvdata.AppendLine(header);

            foreach (var o in objectlist)
                csvdata.AppendLine(ToCsvFields(separator, fields, o));

            return csvdata.ToString();
        }

        public  string ToCsvFields(string separator, PropertyInfo[] fields, object o)
        {
            StringBuilder linie = new StringBuilder();

            foreach (var f in fields)
            {
                if (linie.Length > 0)
                    linie.Append(separator);

                var x = f.GetValue(o);

                if (x != null)
                    linie.Append(x.ToString());
            }

            return linie.ToString();
        }       
    }
}
