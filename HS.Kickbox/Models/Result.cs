using System.Runtime.Serialization;
namespace HS.Kickbox.Models
{
    [DataContract]
    public enum Result
    {
        [EnumMember(Value = "deliverable")]
        Deliverable,

        [EnumMember(Value = "undeliverable")]
        Undeliverable,

        [EnumMember(Value = "risky")]
        Risky,

        [EnumMember(Value = "unknown")]
        Unknown
    }
}
