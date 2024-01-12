using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IBSConnect.Security;

public static class TokenExtensions
{
    /// <summary>
    /// Creates an instance of <see cref="{T}"/> and populates the properties from matching claims
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="claims"></param>
    /// <returns></returns>
    public static T GetClaimsAs<T>(this IEnumerable<Claim> claims)
    {
        var lookup = claims.ToDictionary(claim => claim.Type.ToLower(), claim => claim.Value);

        var instance = Activator.CreateInstance<T>();

        var props = typeof(T).GetProperties();

        foreach (var prop in props)
        {
            if (lookup.TryGetValue(prop.Name.ToLower(), out var value))
            {
                object objectValue = null;
                var propertyType = prop.PropertyType;
                var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);

                if (underlyingType != null)
                {
                    propertyType = underlyingType;
                }

                if (propertyType == typeof(string))
                {
                    objectValue = value;
                }
                else if (propertyType == typeof(int))
                {
                    objectValue = int.Parse(value);
                }
                else if (propertyType == typeof(long))
                {
                    objectValue = long.Parse(value);
                }
                else if (propertyType == typeof(DateTime))
                {
                    objectValue = DateTime.Parse(value);
                }
                else if (propertyType == typeof(bool))
                {
                    objectValue = bool.Parse(value);
                }
                prop.SetValue(instance, objectValue);
            }
            else
            {
                //if (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                //{
                //}
            }
        }

        return instance;
    }

    public static T GetClaimsAs<T>(this JwtSecurityToken jwtToken)
    {
        return GetClaimsAs<T>(jwtToken.Claims);
    }

}

public class UserClaims
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Role { get; set; }
}