using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBSConnect.Data.Models;

public class IBSTranHistory
{
    [Key]
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string IdNo { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public decimal AllottedTime { get; set; }
    public decimal TotalMinutes { get; set; }
    public decimal PaidMinutes { get; set; }
    public decimal TimeLeft { get; set; }
    public decimal ExcessMinutes { get; set; }
    public string SySemester { get; set; }
    public decimal Rate { get; set; }

}