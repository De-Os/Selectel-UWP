using RestSharp;
using Selectel.Libs.APi.Responses;
using System.Collections.Generic;

namespace Selectel.Libs.Api.Methods
{
    public class Servers
    {
        private SelectelApi _api;

        internal Servers(SelectelApi api) => this._api = api;

        public DashboardMethod Dashboard => new DashboardMethod(_api);
        public ServiceMethod Service => new ServiceMethod(_api);
        public ResourcesMethod Resources => new ResourcesMethod(_api);
        public PowerMethod Power => new PowerMethod(_api);
        public LocationMethod Location => new LocationMethod(_api);
        public ConsumptionMethod Consumption => new ConsumptionMethod(_api);
        public NetworkMethod Network => new NetworkMethod(_api);
        public BootMethod Boot => new BootMethod(_api);

        public class DashboardMethod
        {
            private SelectelApi _api;

            internal DashboardMethod(SelectelApi api) => this._api = api;

            public ServersResponse.Dashboard.Maintenance Maintenance()
            {
                var request = new RestRequest("servers/v2/dashboard/maintenance");
                return this._api.Call<ServersResponse.Dashboard.Maintenance>(request);
            }
        }

        public class ServiceMethod
        {
            private SelectelApi _api;

            internal ServiceMethod(SelectelApi api) => this._api = api;

            public List<ServersResponse.Service.Server> Servers()
            {
                var request = new RestRequest("servers/v2/service/server");
                return this._api.Call<List<ServersResponse.Service.Server>>(request);
            }

            public ServersResponse.Service.Server GetServer(string UUID)
            {
                var request = new RestRequest("servers/v2/service/server/" + UUID);
                return this._api.Call<ServersResponse.Service.Server>(request);
            }
        }

        public class ResourcesMethod
        {
            private SelectelApi _api;

            internal ResourcesMethod(SelectelApi api) => this._api = api;

            public List<ServersResponse.Resource.ResourceItem> Get()
            {
                var request = new RestRequest("servers/v2/resource");
                return this._api.Call<List<ServersResponse.Resource.ResourceItem>>(request);
            }
        }

        public class PowerMethod
        {
            private SelectelApi _api;

            internal PowerMethod(SelectelApi api) => this._api = api;

            public ServersResponse.Power.PowerInfo Get(string UUID)
            {
                var request = new RestRequest("servers/v2/power/" + UUID);
                return this._api.Call<ServersResponse.Power.PowerInfo>(request);
            }

            public void Reboot(string UUID)
            {
                var request = new RestRequest("servers/v2/power/" + UUID + "/reboot");
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(new { reboot = true });
                this._api.Call<object>(request, SelectelApi.RequestType.POST);
            }

            public void Set(string UUID, bool power)
            {
                var request = new RestRequest("servers/v2/power/" + UUID);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(new { power_state = power });
                this._api.Call<object>(request, SelectelApi.RequestType.PUT);
            }
        }

        public class LocationMethod
        {
            private SelectelApi _api;

            internal LocationMethod(SelectelApi api) => this._api = api;

            public ServersResponse.Location.LocationInfo Get(string UUID)
            {
                var request = new RestRequest("servers/v2/location/" + UUID);
                return this._api.Call<ServersResponse.Location.LocationInfo>(request);
            }
        }

        public class ConsumptionMethod
        {
            private SelectelApi _api;

            internal ConsumptionMethod(SelectelApi api) => this._api = api;

            public List<List<int>> Get(string UUID, int? till = null, int? from = null)
            {
                var request = new RestRequest("servers/v2/consumption/speed/resource/" + UUID);
                if (till != null) request.AddOrUpdateParameter("till", till);
                if (from != null) request.AddOrUpdateParameter("from", from);
                return this._api.Call<List<List<int>>>(request);
            }
        }

        public class NetworkMethod
        {
            private SelectelApi _api;

            internal NetworkMethod(SelectelApi api) => this._api = api;

            public List<ServersResponse.Network.IPinfo> GetIPs()
            {
                var request = new RestRequest("servers/v2/network/ipam/ip");
                return this._api.Call<List<ServersResponse.Network.IPinfo>>(request);
            }
        }

        public class BootMethod
        {
            private SelectelApi _api;

            internal BootMethod(SelectelApi api) => this._api = api;

            public ServersResponse.BootManager.OSDetails OSInfo(string UUID)
            {
                var request = new RestRequest("servers/v2/boot/os/" + UUID);
                return this._api.Call<ServersResponse.BootManager.OSDetails>(request);
            }
        }
    }
}