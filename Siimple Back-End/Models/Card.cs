using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Siimple_Back_End.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Inage { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
