using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Framework
{
    public static class ZipUnzip
    {
        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (MemoryStream msi = new MemoryStream(bytes))
            using (MemoryStream mso = new MemoryStream())
            {
                using (GZipStream gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                    //CopyTo(msi, gs);
                }

                //return mso.ToArray();
                string response = Convert.ToBase64String(mso.ToArray());
                return Base64UrlEncoder.Encode(response);
            }
        }
        public static string Unzip(string str)
        {
            try
            {
                string Base64String = Base64UrlEncoder.Decode(str);
                byte[] bytes = System.Convert.FromBase64String(Base64String);
                using (MemoryStream msi = new MemoryStream(bytes))
                using (MemoryStream mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        //gs.CopyTo(mso);
                        CopyTo(gs, mso);
                    }

                    return Encoding.UTF8.GetString(mso.ToArray());
                }
            }catch(Exception ex)
            {
                return str;
            }
            
        }
    }
}
