using System;

// In this moment not used

namespace hrmApp.Web.DTO.Base
{
    public abstract class BaseTrackedDTO : BaseDTO
    {
        public DateTime? CtreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CraetedByUser { get; set; }

        public int? ModifiedByUser { get; set; }

    }
}
