using System;
using System.Collections.Generic;
using System.Text;

namespace StullerApiClient.Models.Orders
{

    public class SubmitOrderResponse
    {
        public int ConfirmationNumber { get; set; }
        public bool Created { get; set; }
        public List<SubmitOrderResponseError> Errors { get; set; }
    }
}
