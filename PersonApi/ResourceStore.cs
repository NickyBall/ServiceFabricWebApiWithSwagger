﻿using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.WindowsAzure.Storage.Table;
using PersonApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PersonApi
{
    public class ResourceStore : IResourceStore
    {
        private StorageClient Storage { get; set; }
        List<ApiResource> _ApiResources;
        List<ApiResource> ApiResourceList
        {
            get
            {
                if (_ApiResources == null) _ApiResources = new List<ApiResource>();
                return _ApiResources;
            }
            set => _ApiResources = value;
        }
        List<IdentityResource> _IdentityResources;
        List<IdentityResource> IdentityResourceList
        {
            get
            {
                if (_IdentityResources == null) _IdentityResources = new List<IdentityResource>();
                return _IdentityResources;
            }
            set => _IdentityResources = value;
        }
        public ResourceStore(StorageClient Storage)
        {
            this.Storage = Storage;
        }
        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            TableResult RetrieveResult = await Storage.ApiResourceTable.ExecuteAsync(TableOperation.Retrieve<ApiResourceEntity>("ResourceData", name));
            if (RetrieveResult.HttpStatusCode == HttpStatusCode.OK.GetHashCode())
            {
                ApiResourceEntity ResourceEntity = (ApiResourceEntity)RetrieveResult.Result;
                return new ApiResource(ResourceEntity.RowKey, ResourceEntity.ApiDescription);
            }
            return new ApiResource();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            List<ApiResource> Resources = new List<ApiResource>();
            foreach (var ScopeName in scopeNames)
            {
                TableResult RetrieveResult = await Storage.ApiResourceTable.ExecuteAsync(TableOperation.Retrieve<ApiResourceEntity>("ResourceData", ScopeName));
                if (RetrieveResult.HttpStatusCode == HttpStatusCode.OK.GetHashCode())
                {
                    ApiResourceEntity ResourceEntity = (ApiResourceEntity)RetrieveResult.Result;
                    ApiResource api = new ApiResource(ResourceEntity.RowKey, ResourceEntity.ApiDescription, new[] { "api1" });
                    //ApiResource api = new ApiResource()
                    //{
                    //    Name = ResourceEntity.RowKey,
                    //    DisplayName = ResourceEntity.ApiDescription,
                    //    Scopes =
                    //    {
                    //        new Scope()
                    //        {
                    //            Name = ResourceEntity.RowKey,
                    //            DisplayName = ResourceEntity.ApiDescription
                    //        },
                    //    }
                    //};
                    Resources.Add(api);
                }
            }
            return Resources.AsEnumerable();
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            List<IdentityResource> Resources = new List<IdentityResource>();
            //{
            //    new IdentityResources.OpenId(),
            //    new IdentityResources.Profile(),
            //    new IdentityResources.Email()
            //};
            //foreach (var ScopeName in scopeNames)
            //{
            //    TableResult RetrieveResult = await Storage.IdentityTable.ExecuteAsync(TableOperation.Retrieve<IdentityEntity>("IdentityData", ScopeName));
            //    if (RetrieveResult.HttpStatusCode == HttpStatusCode.OK.GetHashCode())
            //    {
            //        IdentityEntity ResourceEntity = (IdentityEntity)RetrieveResult.Result;

            //        Resources.Add(new IdentityResource(ResourceEntity.RowKey, new [] { "api1" }));
            //    }
            //}
            return Resources.AsEnumerable();
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            TableQuerySegment<ApiResourceEntity> ApiSegment = await Storage.ApiResourceTable.ExecuteQuerySegmentedAsync(new TableQuery<ApiResourceEntity>(), null);
            TableQuerySegment<IdentityEntity> IdentitySegment = await Storage.IdentityTable.ExecuteQuerySegmentedAsync(new TableQuery<IdentityEntity>(), null);
            IEnumerable<ApiResource> ApiResources = null;
            List<IdentityResource> IdentityResources = new List<IdentityResource>();
            if (ApiSegment.Count() > 0) ApiResources = ApiSegment.Select(r => new ApiResource(r.RowKey, r.ApiDescription));
            //if (IdentitySegment.Count() > 0) IdentityResources.AddRange(IdentitySegment.Select(r => new IdentityResource(r.RowKey, new [] { "api1" })));
            Resources Res = new Resources(IdentityResources.AsEnumerable(), ApiResources);
            return Res;
        }
    }
}
