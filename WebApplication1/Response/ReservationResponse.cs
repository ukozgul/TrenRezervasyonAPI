using WebApplication1.Model;

namespace WebApplication1.Response
{
    public class ReservationResponse
    {
        public ReservationResponse()
        {
            this.YerlesimAyrinti=new List<ResponseDetails>();
        }
        public bool RezervasyonYapilabilir { get; set; } = false;

        public List<ResponseDetails> YerlesimAyrinti { get; set; }
    }
}
