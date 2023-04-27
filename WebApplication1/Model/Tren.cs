using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    public class Tren
    {
        public Tren() 
        {
           this.Vagonlar = new List<Vagon>();
        }

        public string Ad { get; set; }
        public virtual List<Vagon> Vagonlar { get; set; }

        public int RezervasyonYapilacakKisiSayisi { get; set; }
        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }



    }
}
