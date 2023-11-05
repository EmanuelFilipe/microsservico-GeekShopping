﻿using GeekShopping.CartAPI.Data.DTO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeekShopping.CartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _client;

        public CouponRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<CouponDTO> GetCoupon(string couponCode, string token)
        {
            //"api/v1/coupon"
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"/api/v1/coupon/{couponCode}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK) return new CouponDTO();
            return JsonSerializer.Deserialize<CouponDTO>(content,
                new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true });
        }
    }
}
