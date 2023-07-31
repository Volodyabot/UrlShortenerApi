using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.DTOs
{
    public class DescriptionUpdateDto
    {
        public int Id { get; set; }
        public required string Value { get; set; }
    }
}