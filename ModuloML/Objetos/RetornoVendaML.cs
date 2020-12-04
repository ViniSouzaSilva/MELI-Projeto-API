using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloML.Objetos
{
    public class RetornoVendaML
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Phone
        {
            public string number { get; set; }
            public string extension { get; set; }
            public string area_code { get; set; }
            public bool verified { get; set; }
        }

        public class AlternativePhone
        {
            public string number { get; set; }
            public string extension { get; set; }
            public string area_code { get; set; }
        }

        public class Seller
        {
            public Phone phone { get; set; }
            public AlternativePhone alternative_phone { get; set; }
            public string nickname { get; set; }
            public string last_name { get; set; }
            public string id { get; set; }
            public string first_name { get; set; }
            public string email { get; set; }
        }

        public class Collector
        {
            public string id { get; set; }
        }

        public class AtmTransferReference
        {
            public string transaction_id { get; set; }
            public object company_id { get; set; }
        }

        public class Payment
        {
            public string reason { get; set; }
            public object status_code { get; set; }
            public string total_paid_amount { get; set; }
            public string operation_type { get; set; }
            public string transaction_amount { get; set; }
            public DateTime date_approved { get; set; }
            public Collector collector { get; set; }
            public object coupon_id { get; set; }
            public string installments { get; set; }
            public string authorization_code { get; set; }
            public string taxes_amount { get; set; }
            public object id { get; set; }
            public DateTime date_last_modified { get; set; }
            public string coupon_amount { get; set; }
            public List<string> available_actions { get; set; }
            public string shipping_cost { get; set; }
            public string? installment_amount { get; set; }
            public DateTime date_created { get; set; }
            public object activation_uri { get; set; }
            public string overpaid_amount { get; set; }
            public long? card_id { get; set; }
            public string status_detail { get; set; }
            public string issuer_id { get; set; }
            public string payment_method_id { get; set; }
            public string payment_type { get; set; }
            public object deferred_period { get; set; }
            public AtmTransferReference atm_transfer_reference { get; set; }
            public string site_id { get; set; }
            public string payer_id { get; set; }
            public object order_id { get; set; }
            public string currency_id { get; set; }
            public string status { get; set; }
            public object transaction_order_id { get; set; }
        }

        public class Taxes
        {
            public object amount { get; set; }
            public object currency_id { get; set; }
        }

        public class OrderRequest
        {
            public object change { get; set; }
            public object @return { get; set; }
        }

        public class Sale
        {
            public object reason { get; set; }
            public DateTime date_created { get; set; }
            public bool fulfilled { get; set; }
            public string rating { get; set; }
            public long id { get; set; }
            public string status { get; set; }
        }

        public class Purchase
        {
            public object reason { get; set; }
            public DateTime date_created { get; set; }
            public bool fulfilled { get; set; }
            public string rating { get; set; }
            public long id { get; set; }
            public string status { get; set; }
        }

        public class Feedback
        {
            public Sale sale { get; set; }
            public Purchase purchase { get; set; }
        }

        public class Shipping
        {
            public object id { get; set; }
        }

        public class Item
        {
            public object seller_custom_field { get; set; }
            public string condition { get; set; }
            public object global_price { get; set; }
            public string category_id { get; set; }
            public object variation_id { get; set; }
            public List<object> variation_attributes { get; set; }
            public object seller_sku { get; set; }
            public string warranty { get; set; }
            public string id { get; set; }
            public string title { get; set; }
        }

        public class OrderItem
        {
            public Item item { get; set; }
            public int quantity { get; set; }
            public string sale_fee { get; set; }
            public string listing_type_id { get; set; }
            public string unit_price { get; set; }
            public double full_unit_price { get; set; }
            public string currency_id { get; set; }
            public object manufacturing_days { get; set; }
        }

        public class Coupon
        {
            public string amount { get; set; }
            public object id { get; set; }
        }

        public class BillingInfo
        {
            public string doc_number { get; set; }
            public string doc_type { get; set; }
        }

        public class Phone2
        {
            public string number { get; set; }
            public string extension { get; set; }
            public string area_code { get; set; }
            public bool verified { get; set; }
        }

        public class AlternativePhone2
        {
            public string number { get; set; }
            public string extension { get; set; }
            public string area_code { get; set; }
        }

        public class Buyer
        {
            public BillingInfo billing_info { get; set; }
            public Phone2 phone { get; set; }
            public AlternativePhone2 alternative_phone { get; set; }
            public string nickname { get; set; }
            public string last_name { get; set; }
            public string id { get; set; }
            public string first_name { get; set; }
            public string email { get; set; }
        }

        public class Result
        {
            public Seller seller { get; set; }
            public List<Payment> payments { get; set; }
            public bool? fulfilled { get; set; }
            public Taxes taxes { get; set; }
            public OrderRequest order_request { get; set; }
            public DateTime expiration_date { get; set; }
            public Feedback feedback { get; set; }
            public Shipping shipping { get; set; }
            public DateTime date_closed { get; set; }
            public object id { get; set; }
            public object manufacturing_ending_date { get; set; }
            public List<OrderItem> order_items { get; set; }
            public DateTime date_last_updated { get; set; }
            public DateTime last_updated { get; set; }
            public object comments { get; set; }
            public object pack_id { get; set; }
            public Coupon coupon { get; set; }
            public DateTime date_created { get; set; }
            public object pickup_id { get; set; }
            public object status_detail { get; set; }
            public List<string> tags { get; set; }
            public Buyer buyer { get; set; }
            public string total_amount { get; set; }
            public string paid_amount { get; set; }
            public List<object> mediations { get; set; }
            public string currency_id { get; set; }
            public string status { get; set; }
        }

        public class Sort
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class AvailableSort
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Paging
        {
            public string total { get; set; }
            public string offset { get; set; }
            public string limit { get; set; }
        }

        public class RetornaVendaML
        {
            public string query { get; set; }
            public List<Result> results { get; set; }
            public Sort sort { get; set; }
            public List<AvailableSort> available_sorts { get; set; }
            public List<object> filters { get; set; }
            public Paging paging { get; set; }
            public string display { get; set; }
        }



    }
}
