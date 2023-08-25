﻿using System.ComponentModel.DataAnnotations;

namespace Interior_Decoration_Services.Models
{
    public class Product
    {
        public Product()
        {
            //groupToProducts = new List<GroupToProduct>();
            //subGroupToProducts = new List<subGroupToProduct>();
            orders = new List<Order>();
            carts = new List<Cart>();
        }
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Summary { get; set; }
        //public int Weight { get; set; }
        //public UnitOFMassMeasurement WeightMassUnit { get; set; }
        //public int Price { get; set; }
        public string? Material { get; set; }
        public string? Color { get; set; }     
        public bool Stock { get; set; }
        //public UnitOFMassMeasurement StockMassUnit { get; set; }
        //public AcceptProduct confirmation { get; set; }
        public DateTime registerDate { get; set; }
        public bool IsDelete { get; set; }
        //Navigation Property
        //public int? sellerId { get; set; }
        public int? groupId { get; set; }
        public int? subGroupId { get; set; }
        public string productImage { get; set; }
        //public Seller seller { get; set; }
        public Group group { get; set; }
        public SubGroup subGroup { get; set; }

        //CartItem cartItem { get; set; }
        //ICollection<Category> categories { get; set; }
        //public ICollection<GroupToProduct> groupToProducts { get; set; }
        public ICollection<Cart> carts { get; set; }
        //public ICollection<subGroupToProduct> subGroupToProducts { get; set; }
        public ICollection<Order> orders { get; set; }
    }
}
