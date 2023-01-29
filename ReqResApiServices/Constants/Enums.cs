using APIServices;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Reflection;

namespace ReqResApiServices.Constants;

public class Enums
{
    public static readonly string BaseUri = "https://reqres.in";
    public static readonly string UsersController = "users";
    public static readonly string ColorsController = "unknown";
    public static readonly string LoginController = "login";
	public static readonly string RegisterController = "register";

    public enum RequestMethod
    {
        [Description("PUT")] Put,
        [Description("DELETE")] Delete,
        [Description("GET")] Get,
        [Description("POST")] Post,
        [Description("PATCH")] Patch,
    }

    public static string GetEnumDescription(Type enumType, string enumValueAsString)
    {
        //http://blog.spontaneouspublicity.com/associating-strings-with-enums-in-c
        var fi = enumType.GetField(enumValueAsString);

        var attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

        return attributes.Length > 0 ? attributes[0].Description : enumValueAsString;
    }
}