﻿namespace GeekShopping.CartAPI.Data.DTO
{
    public class CouponDTO
    {
        public long Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
