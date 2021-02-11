using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Selectel.Libs.APi.Responses
{
    public class ServersResponse
    {
        public class Dashboard
        {
            public class Maintenance
            {
                [JsonProperty("status")]
                public bool Status { get; set; }
            }
        }

        public class Service
        {
            public class Server
            {
                [JsonProperty("uuid")]
                public string UUID { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("tariff_line")]
                public string TarriffLine { get; set; }

                [JsonProperty("model")]
                public string Model { get; set; }

                [JsonProperty("tags")]
                public List<string> Tags { get; set; }

                [JsonProperty("tag_list")]
                public List<Tag> TagList { get; set; }

                [JsonProperty("state")]
                public string State { get; set; }

                [JsonProperty("is_order")]
                public bool IsOrder { get; set; }

                [JsonProperty("is_preorder")]
                public bool IsPreorder { get; set; }

                [JsonProperty("setup_fee_collection")]
                public Money SetupFee { get; set; }

                [JsonProperty("price_collection")]
                public Money Price { get; set; }

                [JsonProperty("service_tag")]
                public string ServiceTag { get; set; }

                [JsonProperty("is_primary")]
                public bool IsPrimary { get; set; }

                [JsonProperty("is_single_prolonged")]
                public bool IsSingleProlonged { get; set; }

                [JsonProperty("quantity")]
                public int Quantity { get; set; }

                [JsonProperty("is_qchange")]
                public bool IsQchange { get; set; }

                [JsonProperty("price_plan_available")]
                public List<string> PricePlansAvailable { get; set; }

                [JsonProperty("addition")]
                public List<Server> Addition { get; set; }

                [JsonProperty("primary")]
                public List<Server> Primary { get; set; }

                [JsonProperty("config_name")]
                public string ConfigName { get; set; }

                [JsonProperty("cpu")]
                public CpuInfo CPU { get; set; }

                [JsonProperty("ram")]
                public List<MemInfo> RAM { get; set; }

                [JsonProperty("disk")]
                public List<MemInfo> ROM { get; set; }

                [JsonProperty("gpu")]
                public GpuInfo GPU { get; set; }

                public class Tag
                {
                    [JsonProperty("uuid")]
                    public string UUID { get; set; }

                    [JsonProperty("text")]
                    public string Text { get; set; }

                    [JsonProperty("is_hide")]
                    public bool IsHide { get; set; }

                    [JsonProperty("is_filter")]
                    public bool IsFilter { get; set; }

                    [JsonProperty("sort_weight")]
                    public int SortWeight { get; set; }

                    [JsonProperty("style_key")]
                    public string StyleKey { get; set; }
                }

                public class Money
                {
                    [JsonProperty("RUB")]
                    public Timings RUB { get; set; }

                    [JsonProperty("EUR")]
                    public Timings EUR { get; set; }

                    [JsonProperty("USD")]
                    public Timings USD { get; set; }

                    public class Timings
                    {
                        [JsonProperty("year")]
                        public double? Year { get; set; }

                        [JsonProperty("day")]
                        public double? Day { get; set; }

                        [JsonProperty("month")]
                        public double? Month { get; set; }

                        [JsonProperty("hour")]
                        public double? Hour { get; set; }
                    }
                }

                public class CpuInfo
                {
                    [JsonProperty("name")]
                    public string Name { get; set; }

                    [JsonProperty("base_freq")]
                    public double BaseFreq { get; set; }

                    [JsonProperty("count")]
                    public int Count { get; set; }

                    [JsonProperty("cores_per_cpu")]
                    public int CoresPerCpu { get; set; }
                }

                public class MemInfo
                {
                    [JsonProperty("type")]
                    public string Type { get; set; }

                    [JsonProperty("size")]
                    public double Size { get; set; }

                    [JsonProperty("count")]
                    public int Count { get; set; }
                }

                public class GpuInfo
                {
                    [JsonProperty("name")]
                    public string Name { get; set; }

                    [JsonProperty("count")]
                    public int Count { get; set; }
                }
            }
        }

        public class Resource
        {
            public class ResourceItem
            {
                [JsonProperty("uuid")]
                public string UUID { get; set; }

                [JsonProperty("info")]
                public string Info { get; set; }

                [JsonProperty("created")]
                public int Created { get; set; }

                [JsonProperty("is_processing")]
                public bool? IsProcessing { get; set; }

                [JsonProperty("hw_uuid")]
                public string HardWareUUID { get; set; }

                [JsonProperty("owner_id")]
                public int OwnerId { get; set; }

                [JsonProperty("state")]
                public string State { get; set; }

                [JsonProperty("previous_state")]
                public string PreviousState { get; set; }

                [JsonProperty("location_uuid")]
                public string LocationUUID { get; set; }

                [JsonProperty("user_desc")]
                public string UserDescription { get; set; }

                [JsonProperty("pay_day")]
                public int PayDay { get; set; }

                [JsonProperty("primary_uuid")]
                public string PrimaryUUID { get; set; }

                [JsonProperty("is_primary")]
                public bool? IsPrimary { get; set; }

                [JsonProperty("is_single_prolonged")]
                public bool? IsSingleProlonged { get; set; }

                [JsonProperty("service_uuid")]
                public string ServiceUUID { get; set; }

                [JsonProperty("quantity")]
                public int Qunatity { get; set; }

                [JsonProperty("service_type")]
                public string ServiceType { get; set; }

                [JsonProperty("config_name")]
                public string ConfigName { get; set; }

                [JsonProperty("billing")]
                public BillingInfo Billing { get; set; }

                [JsonProperty("paid_till")]
                public int PaidTill { get; set; }

                [JsonProperty("actual_grace_date")]
                public int ActualGraceDate { get; set; }

                [JsonProperty("actual_dead_date")]
                public int ActualDeadDate { get; set; }

                [JsonProperty("tags")]
                public List<String> Tags { get; set; }

                [JsonProperty("project_uuid")]
                public string ProjectUUID { get; set; }

                public class BillingInfo
                {
                    [JsonProperty("currency")]
                    public string currency { get; set; }

                    [JsonProperty("current_price_plan")]
                    public CurrentPricePlaneInfo CurrentPricePlan { get; set; }

                    public class CurrentPricePlaneInfo
                    {
                        [JsonProperty("uuid")]
                        public string UUID { get; set; }

                        [JsonProperty("Name")]
                        public string Name { get; set; }

                        [JsonProperty("type")]
                        public string Type { get; set; }

                        [JsonProperty("period")]
                        public int Period { get; set; }
                    }

                    public class PriceInfo
                    {
                        [JsonProperty("due_date")]
                        public int DueDate { get; set; }

                        [JsonProperty("paid_till")]
                        public int PaidTill { get; set; }

                        [JsonProperty("amount_due")]
                        public double AmountDue { get; set; }

                        [JsonProperty("debt")]
                        public double Debt { get; set; }

                        [JsonProperty("discount")]
                        public double Discount { get; set; }

                        [JsonProperty("discount_detail")]
                        public DiscountInfo DiscountDetail { get; set; }

                        [JsonProperty("setup_fee")]
                        public double SetupFee { get; set; }

                        [JsonProperty("plan_price")]
                        public double PlanPrice { get; set; }

                        [JsonProperty("future_price")]
                        public double FuturePrice { get; set; }

                        public class DiscountInfo
                        {
                            [JsonProperty("user")]
                            public int User { get; set; }

                            [JsonProperty("resource")]
                            public int Resource { get; set; }

                            [JsonProperty("price_plan")]
                            public int PricePlan { get; set; }

                            [JsonProperty("campaign")]
                            public string Campaign { get; set; }
                        }
                    }
                }
            }
        }

        public class Power
        {
            public class PowerInfo
            {
                [JsonProperty("driver_status")]
                public DriverInfo DriverStatus { get; set; }

                public class DriverInfo
                {
                    [JsonProperty("maintenance")]
                    public bool? Maintenance { get; set; }

                    [JsonProperty("maintenance_reason")]
                    public string MaintenanceReason { get; set; }

                    [JsonProperty("power_state")]
                    public string PowerState { get; set; }

                    [JsonProperty("target_power_state")]
                    public string TargetPowerState { get; set; }

                    [JsonProperty("console_enabled")]
                    public bool? ConsoleEnabled { get; set; }
                }
            }
        }

        public class Location
        {
            public class LocationInfo
            {
                [JsonProperty("uuid")]
                public string UUID { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("location_id")]
                public int Id { get; set; }

                [JsonProperty("description")]
                public string Description { get; set; }

                [JsonProperty("dc_count")]
                public double? dc_count { get; set; }

                [JsonProperty("enable")]
                public bool Enable { get; set; }
            }
        }

        public class Network
        {
            public class IPinfo
            {
                [JsonProperty("uuid")]
                public string UUID { get; set; }

                [JsonProperty("created")]
                public int Created { get; set; }

                [JsonProperty("updated")]
                public int Updated { get; set; }

                [JsonProperty("resource_uuid")]
                public string ResourceUUID { get; set; }

                [JsonProperty("ip")]
                public string IP { get; set; }

                [JsonProperty("subnet_uuid")]
                public string SubnetUUID { get; set; }

                [JsonProperty("network_uuid")]
                public string NetworkUUID { get; set; }

                [JsonProperty("subnet")]
                public string Subnet { get; set; }

                [JsonProperty("netmask")]
                public string Netmask { get; set; }

                [JsonProperty("gateway")]
                public string GateWay { get; set; }

                [JsonProperty("broadcast")]
                public string BroadCast { get; set; }
            }
        }

        public class BootManager
        {
            public class OSDetails
            {
                [JsonProperty("login")]
                public string Login { get; set; }

                [JsonProperty("partitions")]
                public List<Partition> Partitions { get; set; }

                [JsonProperty("os_template")]
                public string OSTemplate { get; set; }

                [JsonProperty("password")]
                public string Password { get; set; }

                [JsonProperty("raid_type")]
                public string RaidType { get; set; }

                [JsonProperty("arch")]
                public string Arch { get; set; }

                [JsonProperty("version")]
                public string Version { get; set; }

                [JsonProperty("userhostname")]
                public string UserHostname { get; set; }

                [JsonProperty("ipv4_address")]
                public string IPv4 { get; set; }

                public class Partition
                {
                    [JsonProperty("mount")]
                    public string Mount { get; set; }

                    [JsonProperty("fstype")]
                    public string Filesystem { get; set; }

                    [JsonProperty("size")]
                    public double size { get; set; }

                    [JsonProperty("expand")]
                    public bool? Expandable { get; set; }

                    [JsonProperty("editable")]
                    public bool Editable { get; set; }
                }
            }
        }
    }
}