using System.ComponentModel.DataAnnotations;

namespace hrmApp.Core.Models.Base
{
    public class BaseEntityConcurrency : BaseEntity
    {
        // Due to concurrency handling
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

}
