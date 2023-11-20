using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeekShopping.Email.Model.Base
{
    public class BaseEntity
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
    }
}
