﻿using System.ComponentModel.DataAnnotations;

namespace C3P1.Net.Data.Models
{
    public class AppParam
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? ThemeMode { get; set; }
    }
}
