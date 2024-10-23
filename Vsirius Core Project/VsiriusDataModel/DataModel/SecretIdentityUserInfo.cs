using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VsiriusDataModel.DataModel
{
    public interface ISecretIdentityUserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; set; }
        string Content { get; set; }
        string UserInfo { get; set; }
        string CookiesInfo { get; set; }
        string Header { get; set; }
        string Connection { get; set; }
        DateTime DateRequest { get; set; }  

    }

    public class SecretIdentityUserInfo : ISecretIdentityUserInfo
    {
        public int Id { get ; set ; }
        public string Content { get; set; } 
        public string UserInfo { get ; set ; }
        public string CookiesInfo { get ; set ; }
        public string Header { get ; set ; }
        public string Connection { get ; set ; }
        public DateTime DateRequest { get ; set ; }
    }

}
