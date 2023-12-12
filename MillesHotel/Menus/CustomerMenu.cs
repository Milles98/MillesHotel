﻿using Microsoft.EntityFrameworkCore;
using MillesHotel.Data;
using MillesHotel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Menus
{
    public static class CustomerMenu
    {
        //Customer MainMenu Case 2 (1. Register Customer, 2. See Customer, 3. Update Customer, 4. Delete Customers. 0. Return to MainMenu)
        //Customer Menu Case 1 (Input: 1. Name, Age, Email, Country 0. Return to MainMenu)
        //Customer Menu Case 2 (1. See customers by customerID, 2. see all customers, 0. Return to MainMenu)
        //Customer Menu Case 3 (Input: CustomerID, 1. NewCustomerName, 2. NewCustomerAge, 3. NewCustomerEmail, 4. NewCustomerCountry, 0. Return to MainMenu)
        //Customer Menu Case 4 (Input: 1. CustomerID, 0. Return to MainMenu)
        //Customer Menu Case 0 (Return to MainMenu)
        public static void ShowCustomerMenu(DbContextOptionsBuilder<HotelDbContext> options)
        {
            CustomerService customerService = new CustomerService(options);
            int choice;

            do
            {
                Console.Clear();
                Console.WriteLine("╭────────────────────────╮");
                Console.WriteLine("│Customer Menu           │");
                Console.WriteLine("│1. Register Customer    │");
                Console.WriteLine("│2. See Customer         │");
                Console.WriteLine("│See all Customers       │");
                Console.WriteLine("│3. Update Customer      │");
                Console.WriteLine("│4. Soft Delete Customer │");
                Console.WriteLine("│0. Return to MainMenu   │");
                Console.WriteLine("╰────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            customerService.CreateCustomer();
                            break;
                        case 2:
                            customerService.GetCustomerByID();
                            break;
                        case 3:
                            customerService.UpdateCustomer();
                            break;
                        case 4:
                            customerService.SoftDeleteCustomer();
                            break;
                        case 0:
                            Console.WriteLine("Returning to MainMenu...");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }

            } while (choice != 0);
        }
    }
}
