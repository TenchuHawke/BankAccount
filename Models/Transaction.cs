using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class Transaction : BaseEntity{
[Key]
    public int TransactionsId{get; set;}
    public double Amount{get; set;}
    public int Users_UserId{get; set;}
    public User User{get; set;}
    public DateTime CreatedAt{get; set;}
    public DateTime UpdatedAt{get; set;}
    public DateTime Date{get; set;}
     
    }
}