using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi
{
    public class ResourceStore2 : IResourceStore
    {
        public ResourceStore2(StorageClient Storage)
        {
        }
        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            await Task.Delay(1);
            return new ApiResource("api1", "My Api 1");
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            await Task.Delay(1);
            return (new List<ApiResource>()
            {
                new ApiResource("api1", "My Api 1")
            }).AsEnumerable();
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            await Task.Delay(1);
            return (new List<IdentityResource>
            {
                //new IdentityResources.OpenId(),
                //new IdentityResources.Profile()
                new IdentityResource("api1", "My Api 1", new[] { "api1" })
            }).AsEnumerable();
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            await Task.Delay(1);
            Resources Resources = new Resources
                (
                    (new List<IdentityResource>
                        {
                            //new IdentityResources.OpenId(),
                            //new IdentityResources.Profile()
                            new IdentityResource("api1", "My Api 1", new[] { "api1" })
                        }).AsEnumerable(),
                    (new List<ApiResource>()
                        {
                            new ApiResource("api1", "My Api 1")
                        }).AsEnumerable()

                );
            return Resources;
        }
    }
}
