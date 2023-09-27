using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldBadgeChallenge.Data.Entities;

namespace GoldBadgeChallenge.Repository
{
    public class DeliveryRepository
    {
        private readonly List<Delivery> _deliveryDbContext = new List<Delivery>();

        private int _count = 0;

        public bool AddDelivery(Delivery delivery)
        {
            if(delivery is null)
            {
                return false;
            }
            else
            {
                _count++;
                delivery.Id = _count;
                _deliveryDbContext.Add(delivery);
                return true;
            }
        }

        public List<Delivery> GetAllDeliveries()
        {
            return _deliveryDbContext;
        }

        public Delivery GetDeliveryByItemNumber(int itemNumber)
        {
            foreach (Delivery delivery in _deliveryDbContext)
            {
                if (delivery.ItemNumber == itemNumber)
                {
                    return delivery;
                }
            }
            return null!;
        }

        public List<Delivery> GetEnRouteDeliveries()
        {
            List<Delivery> enRouteDeliveries = new List<Delivery>();

            foreach (Delivery delivery in _deliveryDbContext)
            {
                if(delivery.OrderStatus == Data.Entities.Enums.OrderStatus.EnRoute)
                {
                    enRouteDeliveries.Add(delivery);
                }
            }
            return enRouteDeliveries;

            // return _deliveryDbContext.Where(enRoute => enRoute.OrderStatus == Data.Entities.Enums.OrderStatus.EnRoute).ToList();
        }

        public List<Delivery> GetCompleteDeliveries()
        {
            List<Delivery> completeDeliveries = new List<Delivery>();

            foreach (Delivery delivery in _deliveryDbContext)
            {
                if (delivery.OrderStatus == Data.Entities.Enums.OrderStatus.Complete)
                {
                    completeDeliveries.Add(delivery);
                }
            }
            return completeDeliveries;

            // return _deliveryDbContext.Where(complete => complete.OrderStatus == Data.Entities.Enums.OrderStatus.Complete).ToList();
        }

        public bool UpdateDeliveryStatus(int itemNumber, Delivery updatedData)
        {
            Delivery entityInDb = GetDeliveryByItemNumber(itemNumber);
            if (entityInDb != null)
            {
                entityInDb.OrderStatus = updatedData.OrderStatus;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteDelivery(Delivery delivery)
        {
            return _deliveryDbContext.Remove(delivery);
        }

    }
}