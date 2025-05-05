﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Books")]
        public int BookId { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
    }
}
