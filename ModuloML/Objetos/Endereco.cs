using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloML.Objetos
{
    public class Endereco
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class StatusHistory
        {
            public object date_cancelled { get; set; }
            public object date_delivered { get; set; }
            public object date_first_visit { get; set; }
            public object date_handling { get; set; }
            public object date_not_delivered { get; set; }
            public object date_ready_to_ship { get; set; }
            public object date_shipped { get; set; }
            public object date_returned { get; set; }
        }

        public class SubstatusHistory
        {
            public string status { get; set; }
            public string substatus { get; set; }
            public DateTime date { get; set; }
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
            public object id { get; set; }
            public string name { get; set; }
        }

        public class Municipality
        {
            public object id { get; set; }
            public object name { get; set; }
        }

        public class SenderAddress
        {
            public string id { get; set; }
            public string address_line { get; set; }
            public string street_name { get; set; }
            public string street_number { get; set; }
            public object comment { get; set; }
            public string zip_code { get; set; }
            public City city { get; set; }
            public State state { get; set; }
            public Country country { get; set; }
            public Neighborhood neighborhood { get; set; }
            public Municipality municipality { get; set; }
            public object agency { get; set; }
            public List<string> types { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string geolocation_type { get; set; }
            public DateTime geolocation_last_updated { get; set; }
            public string geolocation_source { get; set; }
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

        public class Country2
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Neighborhood2
        {
            public object id { get; set; }
            public string name { get; set; }
        }

        public class Municipality2
        {
            public object id { get; set; }
            public object name { get; set; }
        }

        public class ReceiverAddress
        {
            public string id { get; set; }
            public string address_line { get; set; }
            public string street_name { get; set; }
            public string street_number { get; set; }
            public string comment { get; set; }
            public string zip_code { get; set; }
            public City2 city { get; set; }
            public State2 state { get; set; }
            public Country2 country { get; set; }
            public Neighborhood2 neighborhood { get; set; }
            public Municipality2 municipality { get; set; }
            public object agency { get; set; }
            public List<string> types { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string geolocation_type { get; set; }
            public DateTime geolocation_last_updated { get; set; }
            public string geolocation_source { get; set; }
            public string delivery_preference { get; set; }
            public string receiver_name { get; set; }
            public string receiver_phone { get; set; }
        }

        public class DimensionsSource
        {
            public string id { get; set; }
            public string origin { get; set; }
        }

        public class ShippingItem
        {
            public string id { get; set; }
            public string description { get; set; }
            public string quantity { get; set; }
            public string dimensions { get; set; }
            public DimensionsSource dimensions_source { get; set; }
        }

        public class EstimatedScheduleLimit
        {
            public object date { get; set; }
        }

        public class Buffering
        {
            public object date { get; set; }
        }

        public class Offset
        {
            public DateTime date { get; set; }
            public string shipping { get; set; }
        }

        public class TimeFrame
        {
            public object from { get; set; }
            public object to { get; set; }
        }

        public class EstimatedDeliveryTime
        {
            public string type { get; set; }
            public DateTime date { get; set; }
            public string unit { get; set; }
            public Offset offset { get; set; }
            public TimeFrame time_frame { get; set; }
            public object pay_before { get; set; }
            public string shipping { get; set; }
            public string handling { get; set; }
            public object schedule { get; set; }
        }

        public class EstimatedDeliveryLimit
        {
            public DateTime date { get; set; }
            public string offset { get; set; }
        }

        public class EstimatedDeliveryFinal
        {
            public DateTime date { get; set; }
            public string offset { get; set; }
        }

        public class EstimatedDeliveryExtended
        {
            public DateTime date { get; set; }
            public string offset { get; set; }
        }

        public class EstimatedHandlingLimit
        {
            public DateTime date { get; set; }
        }

        public class ShippingOption
        {
            public string id { get; set; }
            public string shipping_method_id { get; set; }
            public string name { get; set; }
            public string currency_id { get; set; }
            public string list_cost { get; set; }
            public string cost { get; set; }
            public string delivery_type { get; set; }
            public EstimatedScheduleLimit estimated_schedule_limit { get; set; }
            public Buffering buffering { get; set; }
            public EstimatedDeliveryTime estimated_delivery_time { get; set; }
            public EstimatedDeliveryLimit estimated_delivery_limit { get; set; }
            public EstimatedDeliveryFinal estimated_delivery_final { get; set; }
            public EstimatedDeliveryExtended estimated_delivery_extended { get; set; }
            public EstimatedHandlingLimit estimated_handling_limit { get; set; }
        }

        public class CostComponents
        {
            public string special_discount { get; set; }
            public string loyal_discount { get; set; }
            public string compensation { get; set; }
            public string gap_discount { get; set; }
            public string ratio { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public string mode { get; set; }
            public string created_by { get; set; }
            public string order_id { get; set; }
            public string? order_cost { get; set; }
            public string base_cost { get; set; }
            public string site_id { get; set; }
            public string status { get; set; }
            public string substatus { get; set; }
            public StatusHistory status_history { get; set; }
            public List<SubstatusHistory> substatus_history { get; set; }
            public DateTime date_created { get; set; }
            public DateTime last_updated { get; set; }
            public object tracking_number { get; set; }
            public object tracking_method { get; set; }
            public object service_id { get; set; }
            public object carrier_info { get; set; }
            public string sender_id { get; set; }
            public SenderAddress sender_address { get; set; }
            public string receiver_id { get; set; }
            public ReceiverAddress receiver_address { get; set; }
            public List<ShippingItem> shipping_items { get; set; }
            public ShippingOption shipping_option { get; set; }
            public object comments { get; set; }
            public object date_first_printed { get; set; }
            public string market_place { get; set; }
            public object return_details { get; set; }
            public List<string> tags { get; set; }
            public string type { get; set; }
            public string logistic_type { get; set; }
            public object application_id { get; set; }
            public object return_tracking_number { get; set; }
            public CostComponents cost_components { get; set; }
        }


    }
}
