using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using CinemaManagementSystem.AssociationClasses;
using CinemaManagementSystem.Enums;
using CinemaManagementSystem.Exceptions;
using CinemaManagementSystem.PersistenceForAllClasses;

namespace CinemaManagementSystem
{
    [Serializable]
    public class Customer : Person, IExtent<Customer>
    {
        // ================= CLASS EXTENT =================
        private static List<Customer> _customers = new();
        public static IReadOnlyList<Customer> Customers => _customers.AsReadOnly();

        private static void AddCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            _customers.Add(customer);
        }

        public static void ClearAllCustomers()
        {
            _customers.Clear();
        }

        // ================= CONSTRUCTORS =================
        public Customer() { }

        public Customer(
            string name,
            string surname,
            DateTime birthDate,
            GenderEnum gender)
            : base(name, surname, birthDate, gender)
        {
            AddCustomer(this);
        }

        // ================= ORDER ASSOCIATION =================
        [XmlIgnore]
        private Dictionary<DateTime, Order> _orders = new();

        [XmlIgnore]
        public IReadOnlyDictionary<DateTime, Order> Orders => _orders.AsReadOnly();

        internal void AddOrderInternal(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (_orders.ContainsKey(order.DateTimeOfCreation))
                throw new DuplicateException(order, this);

            _orders.Add(order.DateTimeOfCreation, order);
        }

        internal void RemoveOrderInternal(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (!_orders.ContainsKey(order.DateTimeOfCreation))
                throw new ExistenceException(order, this);

            _orders.Remove(order.DateTimeOfCreation);
        }

        public void CreateOrder(Screening screening, Seat seat)
        {
            Order.Create(this, screening, seat);
        }

        public void RemoveOrder(DateTime dateTimeOfCreation)
        {
            Order.RemoveOrder(this, dateTimeOfCreation);
        }

        public void ChooseNewTicket(Screening screening, Seat seat, DateTime dateTimeOfCreation)
        {
            if (!Orders.ContainsKey(dateTimeOfCreation))
                throw new ExistenceException($"Order with date of purchase {dateTimeOfCreation}");

            Orders[dateTimeOfCreation].AddTicket(screening, seat);
        }

        public void RemoveTicket(Screening screening, Seat seat, DateTime dateTimeOfCreation)
        {
            if (!Orders.ContainsKey(dateTimeOfCreation))
                throw new ExistenceException($"Order with date of purchase {dateTimeOfCreation}");

            Orders[dateTimeOfCreation].RemoveTicket(screening, seat);
        }

        public void PayForOrder(DateTime dateTimeOfCreation, CardInfo cardInfo)
        {
            if (!Orders.ContainsKey(dateTimeOfCreation))
                throw new ExistenceException($"Order with date of purchase {dateTimeOfCreation}");

            Orders[dateTimeOfCreation].DateOfPurchase =
                DateTime.Now.Date + DateTime.Now.TimeOfDay;

            Orders[dateTimeOfCreation].cardInfo = cardInfo;
        }

        public void CancelOrder(DateTime dateTimeOfCreation)
        {
            if (!Orders.ContainsKey(dateTimeOfCreation))
                throw new ExistenceException($"Order with date of purchase {dateTimeOfCreation}");

            if (Orders[dateTimeOfCreation].DateOfPurchase != null)
                throw new CancelOrderException(Orders[dateTimeOfCreation]);

            Orders[dateTimeOfCreation].Cancel();
        }

        // ================= STAMPCARD ASSOCIATION =================
        private Dictionary<DateTime, Stampcard> _stampcards = new();

        public IReadOnlyDictionary<DateTime, Stampcard> Stampcards => _stampcards;

        public bool HasActiveStampcard()
        {
            foreach (var pair in _stampcards)
            {
                if (pair.Value.Status == StampCardStatus.Active)
                    return true;
            }
            return false;
        }

        public Stampcard RequestNewStampcard()
        {
            if (HasActiveStampcard())
                throw new StampException(this, "already has active stampcard");

            return Stampcard.CreateStampcard(this);
        }

        internal void SetStampcardInternal(Stampcard stampcard)
        {
            if (stampcard == null)
                throw new ArgumentNullException(nameof(stampcard));

            _stampcards.Add(stampcard.DateOfPurchase, stampcard);
        }

        internal void RemoveStampcardInternal(Stampcard card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            DateTime key = card.DateOfPurchase.Date;

            if (!_stampcards.ContainsKey(key))
                throw new ExistenceException(card, this);

            _stampcards.Remove(key);
        }

        public void ApplyStampCardToOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var stamp = _stampcards.Values
                .FirstOrDefault(s => s.Status == StampCardStatus.Active);

            if (stamp == null)
                throw new StampException(this, "doesn't have active stampcard");

            if (!_orders.ContainsKey(order.DateTimeOfCreation))
                throw new ExistenceException(order, this);

            if (order.DateOfPurchase != null)
                throw new StampException(stamp, order);

            order.ApplyStampCard(stamp);
        }

        // ================= PERSISTENCE =================
        public static void Save(string filePath)
        {
            StreamWriter sw = File.CreateText(filePath);
            XmlSerializer serializer = new(typeof(List<Customer>));
            using XmlTextWriter writer = new(sw);
            serializer.Serialize(writer, _customers);
        }

        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Customer file not found.");

            XmlSerializer serializer = new(typeof(List<Customer>));
            using StreamReader reader = new(filePath);
            _customers = (List<Customer>)serializer.Deserialize(reader) ?? new();
        }

        // ================= EXTENT INTERFACE =================
        public List<Customer> GetExtent() => _customers;

        public void ReplaceExtent(List<Customer> newExtent)
        {
            _customers = newExtent ?? new();
        }

        // ================= OVERRIDE =================
        public override string ToString()
        {
            return $"{base.ToString()}";
        }
    }
}
