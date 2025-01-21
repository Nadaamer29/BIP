using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIP.DataAccess.Entities
{

    public class JWT
    {
        [Required]
        public string key { get; init; }
        [Required]
        public string Issuer { get; init; }
        [Required]
        public string Audience { get; init; }
        [Required]
        [Range(1, int.MaxValue)]
        public int ExpiryMinutes { get; init; }
    }
}
