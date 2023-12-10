using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MillesHotel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var app = new App();
            var temp = DbConfiguration.StartDatabase();
            app.Build(temp);


            //DbContextOptionsBuilder<HotelDbContext> options i metodnamnen 

            //kontrollera bokningar på tiderna som finns (svårast) sen lätt

            //IServiceStrategy

            //HotelLibrary

            //Interfaces:
            //IBookingService
            //ICustomerService
            //IInvoiceService
            //IRoomService

            //Classes:
            //RoomService : IRoomService
            //CustomerService : ICustomerService
            //BookingService : IBookingService
            //InvoiceService : IInvoiceService

            //Classes:

            //AppRun (Constructor: IRoom roomService, ICustomer customerService, IBooking bookingService, IInvoice invoiceService)
            //AppRun (Instance Variables (private readonly) IRoom roomService, ICustomer customerService, IBooking bookingService, IInvoice invoiceService)

            //Room (Instance Variable: private List<Room>)
            //Room (Ctor: rooms = new List<Room>)
            //Room (Properties: RoomID(PK) RoomSize, Double/Single Room, Extra beds, InvoiceID BookingID(FK))
            //RoomService : IRoom (Methods: CreateRoom, GetRoomID, UpdateRoom, DeleteRoom)

            //Customer (Instance Variable: private List<Customer>)
            //Customer (Ctor: Customer = new List<Customer>)
            //Customer (Properties: CustomerID(PK) FirstName, LastName, Age, Email, Phone, Country)
            //CustomerService : ICustomer (Methods: CreateCustomer, GetCustomerID, UpdateCustomer, DeleteCustomer)

            //Booking (Instance Variable: private List<Booking>)
            //Booking (Ctor: Booking = new List<Booking>)
            //Booking (Properties: BookingID(PK), BookingDate, CustomerID(FK)
            //BookingService : IBooking (Methods: CreateBooking, GetBookingID, UpdateBooking, DeleteBooking)

            //Invoice (Instance Variable: private List<Invoice>)
            //Invoice (Ctor: Invoice = new List<Invoice>)
            //Invoice (Properties: InvoiceID(PK), InvoiceAmount, InvoiceDue, CustomerID(FK)
            //InvoiceService : IInvoice (Methods: CreateInvoice, GetInvoiceID, UpdateInvoice, DeleteInvoice)

            //MainMenu (Switch case, 1. Booking, 2. Customer, 3. Room, 4. Invoice, 0. Exit Program)

            //Booking MainMenu Case 1 (1. Register Booking, 2. See Booking, 3. Update Booking, 4. Delete Booking, 0. Return to MainMenu)
            //Booking Menu Case 1 (Input: 1. CustomerID, RoomSize, Double/Single Room, AmountOfBeds, 0. Return to MainMenu)
            //Booking Menu Case 2 (1. See booking by bookingID, 2. See all bookings, 0. Return to MainMenu)
            //Booking Menu Case 3 (Input: BookingID, 1. NewBookingDate, 0. Return to MainMenu)
            //Booking Menu Case 4 (Input: 1. BookingID, 0. Return to MainMenu)
            //Booking Menu Case 0 (Return to MainMenu)

            //Customer MainMenu Case 2 (1. Register Customer, 2. See Customer, 3. Update Customer, 4. Delete Customers. 0. Return to MainMenu)
            //Customer Menu Case 1 (Input: 1. Name, Age, Email, Country 0. Return to MainMenu)
            //Customer Menu Case 2 (1. See customers by customerID, 2. see all customers, 0. Return to MainMenu)
            //Customer Menu Case 3 (Input: CustomerID, 1. NewCustomerName, 2. NewCustomerAge, 3. NewCustomerEmail, 4. NewCustomerCountry, 0. Return to MainMenu)
            //Customer Menu Case 4 (Input: 1. CustomerID, 0. Return to MainMenu)
            //Customer Menu Case 0 (Return to MainMenu)

            //Room MainMenu Case 3 (1. Register Room, 2. See Room, 3. Update Room, 4. Delete Room, 0. Return to MainMenu)
            //Room Menu Case 1 (Input: 1. RoomSize, Double/Single Room, Extra beds, 0. Return to MainMenu)
            //Room Menu Case 2 (1. See room by roomID, 2. see all rooms, 0. Return to MainMenu)
            //Room Menu Case 3 (Input: RoomID, 1. NewRoomSize, 2. NewDouble/SingleRoom, 3. Extrabeds, 0. Return to MainMenu)
            //Room Menu Case 4 (Input: 1. RoomID, 0. Return to MainMenu)
            //Room Menu Case 0 (Return to MainMenu)

            //Invoice MainMenu Case 4 (1. Register Invoice, 2. See Invoice, 3. Update Invoice, 4. Delete Invoice, 0. Return to MainMenu)
            //Invoice Menu Case 1 (Input: CustomerID, 1. InvoiceAmount, InvoiceDue, 0. Return to MainMenu)
            //Invoice Menu Case 2 (1. See Invoice, 2. See all Invoice, 0. Return to MainMenu)
            //Invoice Menu Case 3 (Input invoiceID, 1. UpdateInvoiceAmount, 2. UpdateInvoiceDue, 0. Return to MainMenu)
            //Invoice Menu Case 4 (Input: 1. InvoiceID, 0. Return to MainMenu)

            //Library med BookingService, RoomService, CustomerService

            //Skapa IsActive för softdelete

            //customer provar skriva en bokstav

            //En map för CRUD
            //Eller 3 mappar för de 3 cases
        }
    }
}
