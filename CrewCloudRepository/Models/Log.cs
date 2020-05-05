using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CrewCloudRepository.Models
{
    public class Log
    {
		
    [Key]
	public int Id { get; set; }

     public string Application { get; set; }

     public DateTime Logged { get; set; }
    
     public string Level { get; set; }


     public string Message { get; set; }
	public string Logger { get; set; }

	public string Callsite { get; set; }
	
    public string Exception { get; set; }

	}
}
