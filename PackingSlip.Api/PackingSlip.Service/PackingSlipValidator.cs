using PackingSlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Service
{
    public interface IPackingSlipValidator
    {
        ResponseMessage IsPackingSlipRequestValid(PackingSlipHeader packingSlip);
    }

    public class PackingSlipValidator: IPackingSlipValidator
    {
        public ResponseMessage IsPackingSlipRequestValid(PackingSlipHeader packingSlip)
        {
            ResponseMessage response = new ResponseMessage { IsSuccess = true };

            return response;
        }
    }
}
