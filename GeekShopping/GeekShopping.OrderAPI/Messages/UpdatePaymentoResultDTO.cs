namespace GeekShopping.OrderAPI.Messages
{
    public class UpdatePaymentoResultDTO
    {
        public long OrderId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
