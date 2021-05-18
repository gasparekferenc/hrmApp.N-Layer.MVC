using System.ComponentModel.DataAnnotations;

namespace hrmApp.Web.DTO.Base
{
    public class BaseConcurrencyDTO : BaseDTO
    {
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

}
