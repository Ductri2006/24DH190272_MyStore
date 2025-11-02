using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _24DH190272_MyStore.Models
{
    public class PartialClasses
    {
        [MetadataType(typeof(UserMetadata))]
        public partial class User
        {
            [NotMapped]
            [Compare("Password")]
            public string ConfirmedPassword { get; set; }
        }

        [MetadataType(typeof(CategoryMetadata))]
        public class Category 
        {
          
        }

        [MetadataType(typeof(CustomerMetadata))]
        public partial class Customer
        {
            // không cần code gì thêm, chỉ để gắn metadata
        }

        [MetadataType(typeof(ProductMetadata))]
        public partial class Product
        {
            // không cần viết gì thêm
        }

        [MetadataType(typeof(OrderMetadata))]
        public partial class Order 
        {
        
        }

        [MetadataType(typeof(OrderDetailMetadata))]
        public partial class OrderDetail
        {
            // không cần code thêm gì, chỉ để gắn metadata
        }





    }
}