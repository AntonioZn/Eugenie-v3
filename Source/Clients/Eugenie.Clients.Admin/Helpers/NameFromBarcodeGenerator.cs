namespace Eugenie.Clients.Admin.Helpers
{
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public static class NameFromBarcodeGenerator
    {
        private static readonly Regex titleCheck = new Regex(@"<title>\s*(.+?)\s*</title>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static async Task<string> GetName(string barcode)
        {
            return await Task.Run(() =>
                                  {
                                      var url = $"http://barcode.bg/barcode/BG/barcode-{barcode}/%D0%98%D0%BD%D1%84%D0%BE%D1%80%D0%BC%D0%B0%D1%86%D0%B8%D1%8F-%D0%B7%D0%B0-%D0%B1%D0%B0%D1%80%D0%BA%D0%BE%D0%B4.htm";
                                      var title = string.Empty;
                                      var request = WebRequest.Create(url) as HttpWebRequest;
                                      var response = request.GetResponse() as HttpWebResponse;

                                      using (var stream = response.GetResponseStream())
                                      {
                                          var bytesToRead = 8092;
                                          var buffer = new byte[bytesToRead];
                                          var contents = string.Empty;
                                          int length;
                                          while ((length = stream.Read(buffer, 0, bytesToRead)) > 0)
                                          {
                                              contents += Encoding.UTF8.GetString(buffer, 0, length);

                                              var isMatching = titleCheck.Match(contents);
                                              if (isMatching.Success)
                                              {
                                                  title = isMatching.Groups[1].Value;
                                                  break;
                                              }
                                              if (contents.Contains("</head>"))
                                              {
                                                  break;
                                              }
                                          }
                                      }

                                      if (!title.Contains("Баркод"))
                                      {
                                          return string.Empty;
                                      }

                                      title = title.Replace('/', ' ').Replace('-', ' ').Replace(".", ". ");
                                      title = Regex.Replace(title, @"\s+", " ");
                                      return title.Substring(0, title.LastIndexOf("Баркод") - 1).ToLower();
                                  });
        }
    }
}