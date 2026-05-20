using System.Security.Cryptography;

namespace RealEstateManagementAPI.Helper;

public class Helper()
{
    public void GenerateToken()
    {
          var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
          Console.WriteLine(key);
    }
  
}
