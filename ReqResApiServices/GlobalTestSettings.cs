using Microsoft.Extensions.Configuration;

namespace APIServices
{
    public class GlobalTestSettings
    {
        public GlobalTestSettings(IConfiguration cfg)
        {
            var settings = cfg.GetSection("globalTestSettingsQA");
            ReqResUrl = settings.GetValue<string>("reqResURL");
        }

        public static string ReqResUrl { get; set; }
    }
}
