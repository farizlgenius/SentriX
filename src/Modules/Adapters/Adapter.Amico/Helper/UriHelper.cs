namespace Adapter.Amico.Helper;

public static class UriHelper
{
      public static string UriBuilder(string ip,bool isSecure)
      {
            if(isSecure)
                  return $"https://{ip}";

            return $"http://{ip}";
      }

}