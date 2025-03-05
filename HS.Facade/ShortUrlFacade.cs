using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;

namespace HS.Facade
{
    public class ShortUrlFacade : BaseFacade
    {
        ShortUrlDataAccess _ShortUrlDataAccess;
        public ShortUrlFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_ShortUrlDataAccess == null)
                _ShortUrlDataAccess = new ShortUrlDataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
        }
        public ShortUrlFacade()
        {
            if (_ShortUrlDataAccess == null)
                _ShortUrlDataAccess = new ShortUrlDataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);

        }
        

        public ShortUrl GetSortUrlByUrl(string fullurl,Guid? CustomerId)
        {
            ShortUrl model =  _ShortUrlDataAccess.GetByQuery(string.Format(" Url='{0}'",fullurl)).FirstOrDefault();
            if (model == null)
            {
                string RandomCode = RandomString(7);

                 while(_ShortUrlDataAccess.GetByQuery(string.Format(" Code = '{0}'", RandomCode)).FirstOrDefault() != null)
                {
                    RandomCode = RandomString(7);
                }

                model = new ShortUrl()
                {
                    Code = RandomCode,
                    Url = fullurl,
                    CustomerId = CustomerId.HasValue? CustomerId.Value: new Guid()
                };
                model.Id = (int)_ShortUrlDataAccess.Insert(model);
                return model;
            }
            else
            {
                return model;
            }
        }

        public int InsertShortUrl(ShortUrl shortUrl)
        {
            return (int)_ShortUrlDataAccess.Insert(shortUrl);
        }

        public ShortUrl GetSortUrlCode(string code)
        {
            return _ShortUrlDataAccess.GetByQuery(string.Format("Code = '{0}' COLLATE SQL_Latin1_General_CP1_CS_AS", code)).FirstOrDefault();
        }
        private static string RandomString(int length)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            string Alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);

        }
    }
}
