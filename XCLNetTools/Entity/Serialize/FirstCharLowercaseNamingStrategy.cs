using Newtonsoft.Json.Serialization;

namespace XCLNetTools.Entity.Serialize
{
    /// <summary>
    /// Newtonsoft.Json 首字母小写策略
    /// </summary>
    public class FirstCharLowercaseNamingStrategy : NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            if (name.Length == 1)
            {
                return name.ToLower();
            }
            return char.ToLower(name[0]) + name.Substring(1);
        }
    }
}