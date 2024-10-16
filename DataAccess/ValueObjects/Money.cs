using Ardalis.GuardClauses;
using CatalogService.Domain.Enums;
using System;

namespace CartServiceApp.DataAccess.ValueObjects
{
    public class Money
    {
        public decimal Value { get; set; }
        public CurrencyCode Currency { get; set; }

        public Money(decimal value, CurrencyCode currency)
        {
            Value = Guard.Against.NegativeOrZero(value);
            Currency = Guard.Against.EnumOutOfRange(currency);
        }
    }
}