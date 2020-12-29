using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloML.Objetos
{
    public class AtributosProdu
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Value
        {
            public string id { get; set; }
            public string name { get; set; }
            public object @struct { get; set; }
        }

        public class SaleTerm
        {
            public string id { get; set; }
            public string name { get; set; }
            public string value_id { get; set; }
            public string value_name { get; set; }
            public object value_struct { get; set; }
            public List<Value> values { get; set; }
        }

        public class Picture
        {
            public string id { get; set; }
            public string url { get; set; }
            public string secure_url { get; set; }
            public string size { get; set; }
            public string max_size { get; set; }
            public string quality { get; set; }
        }

        public class Rule
        {
            public bool @default { get; set; }
            public string free_mode { get; set; }
            public bool free_shipping_flag { get; set; }
            public object value { get; set; }
        }

        public class FreeMethod
        {
            public string id { get; set; }
            public Rule rule { get; set; }
        }

        public class Shipping
        {
            public string mode { get; set; }
            public List<FreeMethod> free_methods { get; set; }
            public List<object> tags { get; set; }
            public object dimensions { get; set; }
            public bool local_pick_up { get; set; }
            public bool free_shipping { get; set; }
            public string logistic_type { get; set; }
            public bool store_pick_up { get; set; }
        }

        public class City
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class State
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Country
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Neighborhood
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class City2
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class State2
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class SearchLocation
        {
            public Neighborhood neighborhood { get; set; }
            public City2 city { get; set; }
            public State2 state { get; set; }
        }

        public class SellerAddress
        {
            public string address_line { get; set; }
            public string zip_code { get; set; }
            public City city { get; set; }
            public State state { get; set; }
            public Country country { get; set; }
            public SearchLocation search_location { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string id { get; set; }
        }

        public class Location
        {
        }

        public class Geolocation
        {
            public string latitude { get; set; }
            public string longitude { get; set; }
        }

        public class Value2
        {
            public string id { get; set; }
            public string name { get; set; }
            public object @struct { get; set; }
        }

        public class Attribute
        {
            public string id { get; set; }
            public string name { get; set; }
            public string value_id { get; set; }
            public string value_name { get; set; }
            public object value_struct { get; set; }
            public List<Value2> values { get; set; }
            public string attribute_group_id { get; set; }
            public string attribute_group_name { get; set; }
        }

        public class Value3
        {
            public string id { get; set; }
            public string name { get; set; }
            public object @struct { get; set; }
        }

        public class AttributeCombination
        {
            public string id { get; set; }
            public string name { get; set; }
            public string value_id { get; set; }
            public string value_name { get; set; }
            public object value_struct { get; set; }
            public List<Value3> values { get; set; }
        }

        public class Value4
        {
            public object id { get; set; }
            public string name { get; set; }
            public object @struct { get; set; }
        }

        public class Attribute2
        {
            public string id { get; set; }
            public string name { get; set; }
            public object value_id { get; set; }
            public string value_name { get; set; }
            public object value_struct { get; set; }
            public List<Value4> values { get; set; }
        }

        public class Variation
        {
            public long id { get; set; }
            public string price { get; set; }
            public List<AttributeCombination> attribute_combinations { get; set; }
            public string available_quantity { get; set; }
            public string sold_quantity { get; set; }
            public List<object> sale_terms { get; set; }
            public List<string> picture_ids { get; set; }
            public object seller_custom_field { get; set; }
            public object catalog_product_id { get; set; }
            public List<Attribute2> attributes { get; set; }
            public object inventory_id { get; set; }
            public List<object> item_relations { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public string site_id { get; set; }
            public string title { get; set; }
            public object subtitle { get; set; }
            public string seller_id { get; set; }
            public string category_id { get; set; }
            public object official_store_id { get; set; }
            public string price { get; set; }
            public string base_price { get; set; }
            public object original_price { get; set; }
            public object inventory_id { get; set; }
            public string currency_id { get; set; }
            public string initial_quantity { get; set; }
            public string available_quantity { get; set; }
            public string sold_quantity { get; set; }
            public List<SaleTerm> sale_terms { get; set; }
            public string buying_mode { get; set; }
            public string listing_type_id { get; set; }
            public DateTime start_time { get; set; }
            public DateTime stop_time { get; set; }
            public DateTime end_time { get; set; }
            public DateTime expiration_time { get; set; }
            public string condition { get; set; }
            public string permalink { get; set; }
            public string thumbnail_id { get; set; }
            public string thumbnail { get; set; }
            public string secure_thumbnail { get; set; }
            public List<Picture> pictures { get; set; }
            public object video_id { get; set; }
            public List<object> descriptions { get; set; }
            public bool accepts_mercadopago { get; set; }
            public List<object> non_mercado_pago_payment_methods { get; set; }
            public Shipping shipping { get; set; }
            public string international_delivery_mode { get; set; }
            public SellerAddress seller_address { get; set; }
            public object seller_contact { get; set; }
            public Location location { get; set; }
            public Geolocation geolocation { get; set; }
            public List<object> coverage_areas { get; set; }
            public List<Attribute> attributes { get; set; }
            public List<object> warnings { get; set; }
            public string listing_source { get; set; }
            public List<Variation> variations { get; set; }
            public string status { get; set; }
            public List<object> sub_status { get; set; }
            public List<string> tags { get; set; }
            public string warranty { get; set; }
            public object catalog_product_id { get; set; }
            public string domain_id { get; set; }
            public object seller_custom_field { get; set; }
            public object parent_item_id { get; set; }
            public object differential_pricing { get; set; }
            public List<object> deal_ids { get; set; }
            public bool automatic_relist { get; set; }
            public DateTime date_created { get; set; }
            public DateTime last_updated { get; set; }
            public string health { get; set; }
            public bool catalog_listing { get; set; }
            public List<object> item_relations { get; set; }
        }


    }
}
