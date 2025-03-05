using Forte.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FortePaymethod : ForteResourceClass
    {
        //[JsonProperty("account_id")]
        //public string account_id
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("location_id")]
        //public string location_id
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("customer_token")]
        //public string customer_token
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("paymethod_token")]
        //public string paymethod_token
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("label")]
        //public string label
        //{
        //    get;
        //    set;
        //}

        [JsonProperty("card")]
        public string card
        {
            get;
            set;
        }
        
       [JsonProperty("echeck")]
        public string echeck
        {
            get;
            set;
        }

        //[JsonProperty("echeck")]
        //public ForteEcheck echeck
        //{
        //    get;
        //    set;
        //}

        [JsonProperty("notes")]
        public string notes
        {
            get;
            set;
        }
        public List<ForteCard> CardList { get; set; }
        public List<ForteAchCard> AchCardList { get; set; }
        public List<UpdatePayInfoForCreditCard> UpdateCardList { get; set; }
        public List<UpdatePaymentInfoForAch> UpdateAchCardList { get; set; }
        //public FortePaymethod()
        //{
        //    echeck = new ForteEcheck();
        //    card = new ForteCard();
        //    links = new ForteLinks();
        //    response = new ForteResponse();
        //}
    }
}
