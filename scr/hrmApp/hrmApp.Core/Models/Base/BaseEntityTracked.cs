using System;
namespace hrmApp.Core.Models.Base
{
    public abstract class BaseEntityTracked : BaseEntity
    {
        public DateTime? CtreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CraetedByUser { get; set; }

        public int? ModifiedByUser { get; set; }

    }
}
