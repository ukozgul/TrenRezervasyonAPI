using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Model;
using WebApplication1.Response;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestDbContext _context;

        public TestController(TestDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Tren>> PostTren(Tren command)
        {
            var responseMessage = new ReservationResponse();
            bool vagonSecenek = command.KisilerFarkliVagonlaraYerlestirilebilir;
            int rezervasyonSayisi = command.RezervasyonYapilacakKisiSayisi;

           
            if (vagonSecenek)
            {
                int bosKoltukSayisi = 0;
                foreach (var item in command.Vagonlar)
                {
                    int onlineLimit = item.Kapasite * 70 / 100; 
                    if (item.DoluKoltukAdet < onlineLimit) 
                    {
                        bosKoltukSayisi += onlineLimit - item.DoluKoltukAdet;
                    }
                }

                if (bosKoltukSayisi >= rezervasyonSayisi)
                {
                    responseMessage.RezervasyonYapilabilir = true;
                    foreach (var item in command.Vagonlar)
                    {
                        int onlineLimit = item.Kapasite * 70 / 100;
                        int musaitKoltuk = onlineLimit - item.DoluKoltukAdet;
                        if (musaitKoltuk > 0)
                        {
                            if (musaitKoltuk > rezervasyonSayisi)
                            {
                                ResponseDetails vagonSeat = new ResponseDetails
                                {
                                    KisiSayisi = musaitKoltuk - rezervasyonSayisi + item.DoluKoltukAdet,
                                    VagonAdi = item.Ad
                                };
                                responseMessage.YerlesimAyrinti.Add(vagonSeat);
                                break;
                            }
                            else
                            {
                                ResponseDetails vagonKoltuk = new ResponseDetails
                                {
                                    KisiSayisi = musaitKoltuk + item.DoluKoltukAdet,
                                    VagonAdi = item.Ad
                                };
                                responseMessage.YerlesimAyrinti.Add(vagonKoltuk);
                                rezervasyonSayisi -= rezervasyonSayisi - musaitKoltuk;
                            }
                        }
                    }
                }
            }
            
            else
            {
                foreach (var item in command.Vagonlar)
                {
                    int onlineLimit = item.Kapasite * 70 / 100;
                    if (item.DoluKoltukAdet + rezervasyonSayisi < onlineLimit)
                    {
                        responseMessage.RezervasyonYapilabilir = true;
                        ResponseDetails vagonSeat = new ResponseDetails
                        {
                            KisiSayisi = item.DoluKoltukAdet + rezervasyonSayisi + item.DoluKoltukAdet,
                            VagonAdi = item.Ad
                        };
                        responseMessage.YerlesimAyrinti.Add(vagonSeat);
                        break;
                    }
                }
            }
            return Ok(responseMessage);


            //_context.Trens.Add(request);
            //await _context.SaveChangesAsync();

            //return Ok("GetTrens");
        }
    }
}
