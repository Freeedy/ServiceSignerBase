using ServiceSignerBase.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase.Extentions
{
    public static  class AttributeHelper
    {

        public static List<SignablePropertyInfo> GetPropertiesInfo(object obj, string route = "")
        {
            List<SignablePropertyInfo> results = new List<SignablePropertyInfo>();

            // You can filter wich property you want https://docs.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo?view=net-6.0
            var objectProperties = obj.GetType().GetProperties().Where(p => p.CanRead);
            foreach (var property in objectProperties)
            {
                var value = property.GetValue(obj);
                if (value == null) return results;
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    results.AddRange(GetPropertiesInfo(value, route + property.Name + "."));
                }
                else
                {
                    // Check if the property has the Custom Attribute
                    var customAttributes = property.GetCustomAttributes<SignableAttribute>();
                    if (!customAttributes.Any())
                        continue;
                    // You can set a method in your Attribute : customAttributes.First().CheckIfNeedToStoreProperty(obj);

                    results.Add(new SignablePropertyInfo()
                    {
                        Name = property.Name,
                        Value = value,
                        Route = route + property.Name
                    });
                }

            }

            return results;
        }
        public static void GetMyProperties(object obj)
        {
            foreach (PropertyInfo pinfo in obj.GetType().GetProperties())
            {
                var getMethod = pinfo.GetGetMethod();
                if (getMethod.ReturnType.IsArray)
                {
                    var arrayObject = getMethod.Invoke(obj, null);
                    foreach (object element in (Array)arrayObject)
                    {
                        foreach (PropertyInfo arrayObjPinfo in element.GetType().GetProperties())
                        {
                            Console.WriteLine(arrayObjPinfo.Name + ":" + arrayObjPinfo.GetGetMethod().Invoke(element, null).ToString());
                        }
                    }
                }
            }
        }
        public static void GetSignableProperties<T>(this T contract)
        {
           

            var properties = typeof(T).GetProperties();
            List<SignablePropertyInfo> propertiesList = new List<SignablePropertyInfo>();
            foreach (var prop in properties)
            {
                if (Attribute.IsDefined(prop, typeof(SignableAttribute)))
                {
                    var res = GetSignablePropertiesRecur(prop, prop.Name);
                    //prop.PropertyType.IsClass
                   // propertiesList.Add();
                }
            }


        }

        public static List<SignablePropertyInfo> GetSignablePropertiesRecur( PropertyInfo root ,string route, List<SignablePropertyInfo> bag = null )
        {
            List<SignablePropertyInfo> result;

            if(root.PropertyType.IsClass)
            {

            }

            return null;
        }


        public static SignablePropertyInfo GetPropValue<T>(T src, string propName , SignablePropertyInfo temppath =null)
        {
            var defaultFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            // var prop = typeof(T).GetProperty(propName,defaultFlags| System.Reflection.BindingFlags.IgnoreCase);

            if (temppath == null) temppath = new SignablePropertyInfo() { Name=typeof(T).Name };
            SignablePropertyInfo result = temppath;
            
          
            if (propName.Contains("."))//complex type nested
            {
                if(temppath!=null) result.Route=src.GetType()+".";
                
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropValue(GetPropValue(src, temp[0],result).Value, temp[1],result);
            }
            else
            {
                var type =src?.GetType();
                var prop = type.GetProperty(propName, defaultFlags | System.Reflection.BindingFlags.IgnoreCase);
                if (prop != null)
                {
                    result =new SignablePropertyInfo { Name = propName , Value=prop.GetValue(src,null)};
                    return  result;
                }

            }
            
            return result ;
           
        }
    }

    public class SignablePropertyInfo
    {


        public string Name { get; set; } = ""; 
        public object Value { get; set; } 

        public string Route { get; set; } = "";


        
       
    }
}
