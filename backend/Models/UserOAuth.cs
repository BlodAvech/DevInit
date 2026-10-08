using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    [Table("userOAuth")]
	public class UserOAuth : Identity
	{
        [Column("id")]
		public Guid Id { get; set; }

        public User User { get; set; } = null!;
        
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("provider")]
        public required string Provider { get; set; } 
        
        [Column("provider_id")]
        public required string ProviderId { get; set; } 
	}
}