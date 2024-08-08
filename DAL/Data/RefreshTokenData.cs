using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class RefreshTokenData
    {
    
            public int Id { get; set; }
            public string? Token { get; set; }
            public string? Username { get; set; }
            public DateTime ExpiryDate { get; set; }
        

    }
}
