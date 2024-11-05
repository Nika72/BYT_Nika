﻿using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Enums;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Manager : SerializableObject<Manager>
    {
        [Required(ErrorMessage = "Manager ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Manager ID must be a positive integer.")]
        public int IdManager { get; set; }

        [Required(ErrorMessage = "Level is required.")]
        public ManagerLevel Level { get; set; }
        
        public Manager(){}
        public Manager(int idManager, ManagerLevel level)
        {
            IdManager = idManager;
            Level = level;
        }

        
        //METHODS
        public bool AssignTableToWaiter(Waiter waiter, Table table)
        {
            if (waiter == null)
            {
                throw new ArgumentNullException(nameof(waiter), "Waiter cannot be null.");
            }

            if (table == null)
            {
                throw new ArgumentNullException(nameof(table), "Table cannot be null.");
            }
            
            if (waiter.AssignedTables.Any())
            {
                Console.WriteLine($"Waiter {waiter.IdWaiter} is already assigned to another table.");
                return false;
            }
            waiter.AssignTable(table);
            return true;
        }
    }
}
