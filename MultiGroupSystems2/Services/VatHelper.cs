using System;

namespace MultiGroupSystemsTester.Services
{
   
    public static class VatHelper
    {
        public const decimal Rate = 0.15m;
        public const string RateLabel = "15%";

        public static decimal VatOf(decimal exclAmount)
        {
            return exclAmount * Rate;
        }

        public static decimal InclusiveOf(decimal exclAmount)
        {
            return exclAmount + VatOf(exclAmount);
        }

        public static decimal Round(decimal amount)
        {
            return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        }
    }
}
