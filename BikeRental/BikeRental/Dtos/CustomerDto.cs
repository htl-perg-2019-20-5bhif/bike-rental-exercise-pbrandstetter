using BikeRentalService.Model;
using System;
using System.Collections.Generic;

namespace BikeRentalApi.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long BirthDay { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Town { get; set; }

        public static List<CustomerDto> ToCustomerDto(List<Customer> allCustomers)
        {
            var customers = new List<CustomerDto>();
            foreach (Customer c in allCustomers)
            {
                customers.Add(ToCustomerDto(c));

            }
            return customers;
        }

        public static CustomerDto ToCustomerDto(Customer c)
        {
            return new CustomerDto()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Gender = c.Gender,
                HouseNumber = c.HouseNumber,
                Street = c.Street,
                Town = c.Town,
                ZipCode = c.ZipCode,
                BirthDay = new DateTimeOffset(c.BirthDay).ToUnixTimeMilliseconds()
            };
        }

        public static List<Customer> FromCustomerDto(List<CustomerDto> allCustomerDtos)
        {
            var customers = new List<Customer>();
            foreach (CustomerDto c in allCustomerDtos)
            {
                customers.Add(FromCustomerDto(c));

            }
            return customers;
        }

        public static Customer FromCustomerDto(CustomerDto c)
        {
            return new Customer()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Gender = c.Gender,
                HouseNumber = c.HouseNumber,
                Street = c.Street,
                Town = c.Town,
                ZipCode = c.ZipCode,
                BirthDay = new DateTime(c.BirthDay)
            };
        }
    }
}
