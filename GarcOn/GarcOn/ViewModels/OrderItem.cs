using GarcOn.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarcOn.ViewModels
{
    public class OrderItem
    {
        public Guid UniqueId { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public List<Adicional> SelectedAditionals { get; set; }
        public string ImageDownArrow { get; set; }

        public OrderItem(Guid uniqueId, long id, string name, string description, double unitPrice, int quantity, double totalPrice, List<Adicional> selectedAditionals, string imageDownArrow)
        {
            this.UniqueId = uniqueId;
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.UnitPrice = unitPrice;
            this.Quantity = quantity;
            this.TotalPrice = totalPrice;
            this.SelectedAditionals = selectedAditionals;
            this.ImageDownArrow = imageDownArrow;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
