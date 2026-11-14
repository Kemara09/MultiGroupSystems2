using System;

namespace MultiGroupSystemsTester.Services
{
    /// <summary>
    /// Single source of truth for South African VAT.
    /// Prices in dbo.Inventory (UnitPrice), the cart and Order_Line are ALL stored EXCLUSIVE of VAT.
    /// VAT is only ever added on top, once, at cart/checkout/invoice level.
    /// The VAT-inclusive figure is for DISPLAY ONLY (e.g. Products page) and must never be stored
    /// back into the cart or the database, otherwise VAT would be charged twice.
    /// </summary>
    public static class VatHelper
    {
        public const decimal Rate = 0.15m;
        public const string RateLabel = "15%";

        /// <summary>VAT payable on an amount that is exclusive of VAT .</summary>
        public static decimal VatOf(decimal exclAmount)
        {
            return exclAmount * Rate;
        }

        /// <summary>Price including VAT for an exclusive amount.</summary>
        public static decimal InclusiveOf(decimal exclAmount)
        {
            return exclAmount + VatOf(exclAmount);
        }

        /// <summary>Rounds the way money is shown to customers.</summary>
        public static decimal Round(decimal amount)
        {
            return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        }
    }
}
