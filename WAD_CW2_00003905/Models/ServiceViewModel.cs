using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WAD_CW2_00003905.DAL.Entities;

namespace WAD_CW2_00003905.Models
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}