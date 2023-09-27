using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldBadgeChallenge.Repository;
using GoldBadgeChallenge.Data.Entities;
using GoldBadgeChallenge.Data.Entities.Enums;

namespace GoldBadgeChallenge.UI
{
    public class ProgramUI
    {
        private readonly DeliveryRepository _deliveryRepo = new DeliveryRepository();
        private bool IsRunning = true;
        public void RunApplication()
        {
            Seed();
            Run();
        }
        public void Run()
        {
            while (IsRunning)
            {
                Clear();
                WriteLine("Warner Transit Federal delivery tracking software\n" +
                        "\n" +
                        "Please enter the number next to the function you would like to execute:\n" +
                        "1. Add a new delivery\n" +
                        "2. List all en route deliveries\n" +
                        "3. List all completed deliveries\n" +
                        "4. Update the status of a delivery\n" +
                        "5. Cancel a delivery\n" +
                        "0. Exit application\n");

                var userInput = ReadLine()!;
                switch (userInput)
                {
                    case "1":
                        AddDelivery();
                        break;
                    case "2":
                        GetEnRoute();
                        break;
                    case "3":
                        GetComplete();
                        break;
                    case "4":
                        EditStatus();
                        break;
                    case "5":
                        Delete();
                        break;
                    case "0":
                        IsRunning = CloseApplication();
                        break;
                    default:
                        WriteLine("Invalid Entry\n" +
                                "Press any key to continue and please ensure only the number next to the desired selection is entered");
                        ReadKey();
                        break;
                }
            }
        }

        private bool CloseApplication()
        {
            Clear();
            return false;
        }

        private void Delete()
        {
            Clear();
            RetrieveDeliveryListData();

            try
            {
                WriteLine("\n Please select an Order you would like to Cancel by entering it's Item Number.");
                int userInputItemNumber = int.Parse(ReadLine()!);
                Delivery deliveryDataFromDb = RetrieveDeliveryData(userInputItemNumber);

                if (deliveryDataFromDb == null)
                {
                    DisplayErrorHandling(userInputItemNumber);
                }
                else
                {
                    if (_deliveryRepo.DeleteDelivery(deliveryDataFromDb))
                    {
                        WriteLine("\n The selected Order has been Cancelled.");
                    }
                    else
                    {
                        WriteLine("\n The selected Order was unable to be Cancelled.");
                    }
                }
            }
            catch (System.Exception e)
            {
                WriteLine($"Something went wrong: {e.Message}");
            }

            WriteLine("\n Press any key to continue.");
            ReadKey();
        }

        private void DisplayErrorHandling(int userInputItemNumber)
        {
            WriteLine($"Item number: {userInputItemNumber} cannot be found");
            return;
        }

        private Delivery RetrieveDeliveryData(int userInputItemNumber)
        {
            Delivery delivery = _deliveryRepo.GetDeliveryByItemNumber(userInputItemNumber);
            return delivery;
        }

        private void RetrieveDeliveryListData()
        {
            var deliveriesInDb = _deliveryRepo.GetAllDeliveries();
            if (deliveriesInDb.Count > 0)
            {
                foreach (var delivery in deliveriesInDb)
                {
                    DisplayDeliveryInfo(delivery);
                }
            }
            else
            {
                WriteLine("There are no retrievable deliveries");
            }
        }

        private void DisplayDeliveryInfo(Delivery delivery)
        {
            WriteLine($"Item Number: {delivery.ItemNumber} - Item Quantity: {delivery.ItemQuantity} - Ordered by Customer ID#: {delivery.CustomerId}" +
                    $" - Order status: {delivery.OrderStatus} - Ordered Date: {delivery.OrderDate} - Delivery Date {delivery.DeliveryDate}");
        }

        private void EditStatus()
        {
            Clear();
            RetrieveDeliveryListData();

            try
            {
                WriteLine("\n Please select a delivery by Item Number to update Status.");
                int userInputItemNumber = int.Parse(ReadLine()!);
                Delivery deliveryFromDb = RetrieveDeliveryData(userInputItemNumber);

                if (deliveryFromDb == null)
                {
                    DisplayErrorHandling(userInputItemNumber);
                }
                else
                {
                    WriteLine("\nEnter the number next to the desired updated delivery Status:\n" +
                            "1. Scheduled\n" +
                            "2. EnRoute\n" +
                            "3. Complete\n" +
                            "4. Canceled\n");
                    int userInputDeliveryStatus = int.Parse(ReadLine()!);
                    deliveryFromDb.OrderStatus = (OrderStatus)userInputDeliveryStatus;

                    if (_deliveryRepo.UpdateDeliveryStatus(userInputItemNumber, deliveryFromDb))
                    {
                        WriteLine("\nThe entered Status has been successfully updated.");
                    }
                    else
                    {
                        WriteLine("\nThe entered Status was unable to be updated.");
                    }
                }
            }
            catch (System.Exception e)
            {
                WriteLine($"Something went wrong: {e.Message}");
            }

            WriteLine("\n Press any key to continue.");
            ReadKey();
        }

        private void GetComplete()
        {
            Clear();
            WriteLine("Complete Status Deliveries\n");

            try
            {
                if (_deliveryRepo.GetAllDeliveries().Count() > 0)
                {
                    List<Delivery> completeDeliveries = _deliveryRepo.GetCompleteDeliveries();
                    if (completeDeliveries.Count() > 0)
                    {
                        foreach (var delivery in completeDeliveries)
                        {
                            DisplayDeliveryInfo(delivery);
                        }
                    }
                    else
                    {
                        WriteLine("No Delivery Status's are currently set to Complete");
                    }
                }
                else
                {
                    WriteLine("There are no Deliveries");
                }
            }
            catch (System.Exception e)
            {
                WriteLine($"Something went wrong: {e.Message}");
            }

            WriteLine("\n Press any key to continue.");
            ReadKey();
        }

        private void GetEnRoute()
        {
            Clear();
            WriteLine("EnRoute Status Deliveries\n");

            try
            {
                if (_deliveryRepo.GetAllDeliveries().Count() > 0)
                {
                    List<Delivery> enRouteDeliveries = _deliveryRepo.GetEnRouteDeliveries();
                    if (enRouteDeliveries.Count() > 0)
                    {
                        foreach (var delivery in enRouteDeliveries)
                        {
                            DisplayDeliveryInfo(delivery);
                        }
                    }
                    else
                    {
                        WriteLine("No Delivery Status's are currently set to EnRoute");
                    }
                }
                else
                {
                    WriteLine("There are no Deliveries");
                }
            }
            catch (System.Exception e)
            {
                WriteLine($"Something went wrong: {e.Message}");
            }

            WriteLine("\n Press any key to continue.");
            ReadKey();
        }

        private void AddDelivery()
        {
            Clear();
            Delivery delivery = new Delivery();

            try
            {
                WriteLine("Enter the Delivery's Item Number:");
                int userInputItemNumber = int.Parse(ReadLine()!);
                delivery.ItemNumber = userInputItemNumber;

                WriteLine("Enter the Item Quantity in a whole number:");
                int userInputItemQuantity = int.Parse(ReadLine()!);
                delivery.ItemQuantity = userInputItemQuantity;

                WriteLine("Enter the Customer ID by the orderer:");
                int userInputCustomerId = int.Parse(ReadLine()!);
                delivery.CustomerId = userInputCustomerId;

                WriteLine("Enter the number next to the correct delivery Status:\n" +
                        "1. Scheduled\n" +
                        "2. EnRoute\n" +
                        "3. Complete\n" +
                        "4. Canceled\n");
                int userInputDeliveryStatus = int.Parse(ReadLine()!);

                delivery.OrderStatus = (OrderStatus)userInputDeliveryStatus;

                WriteLine("Enter the Order Date in the following format 'yyyy/mm/dd':");
                DateOnly userInputOrderDate = DateOnly.Parse(ReadLine()!);
                delivery.OrderDate = userInputOrderDate;

                WriteLine("Enter the Delivery Date in the following format 'yyyy/mm/dd':");
                DateOnly userInputDeliveryDate = DateOnly.Parse(ReadLine()!);
                delivery.DeliveryDate = userInputDeliveryDate;

                if (_deliveryRepo.AddDelivery(delivery))
                {
                    WriteLine("\nThe entered Delivery has been successfully added.");
                }
                else
                {
                    WriteLine("\nThe entered Delivery was unable to be added.");
                }
            }
            catch (System.Exception e)
            {
                WriteLine($"Something went wrong: {e.Message}");
            }

            WriteLine("Press any key to continue.");
            ReadKey();
        }

        private void Seed()
        {
            Delivery delivery42 = new Delivery(DateOnly.Parse("2023/09/25"), DateOnly.Parse("2023/10/07"), OrderStatus.Scheduled, 42, 35, 56);
            Delivery delivery100 = new Delivery(DateOnly.Parse("2023/08/21"), DateOnly.Parse("2023/11/28"), OrderStatus.EnRoute, 100, 1, 3);
            Delivery delivery789 = new Delivery(DateOnly.Parse("2023/09/26"), DateOnly.Parse("2024/06/13"), OrderStatus.Complete, 789, 3, 35);
            Delivery delivery1 = new Delivery(DateOnly.Parse("2023/09/27"), DateOnly.Parse("2023/10/30"), OrderStatus.Canceled, 1, 10, 70);

            _deliveryRepo.AddDelivery(delivery42);
            _deliveryRepo.AddDelivery(delivery100);
            _deliveryRepo.AddDelivery(delivery789);
            _deliveryRepo.AddDelivery(delivery1);
        }

    }
}