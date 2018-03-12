using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi
{
    public class StorageClient : IDisposable
    {
        #region Properties

        public string ConnectionString => "UseDevelopmentStorage=true";
        public string PrefixTable => "IDx";

        public CloudTable ClientStore { get { ThrowIfDisposed(); return _ClientStore; } set { _ClientStore = value; } }
        private CloudTable _ClientStore;

        public CloudTable ApiResourceTable { get { ThrowIfDisposed(); return _ApiResourceTable; } set { _ApiResourceTable = value; } }
        private CloudTable _ApiResourceTable;

        public CloudTable IdentityTable { get { ThrowIfDisposed(); return _IdentityTable; } set { _IdentityTable = value; } }
        private CloudTable _IdentityTable;

        private CloudTableClient CloudStorageClient = null;
        //private CloudQueueClient QueueClient = null;
        #endregion
        #region Constructor
        public StorageClient()
        {
            //string ConnectionString = Properties.Settings.Default.ApplicationStorageConnectionString;
            //string PrefixTable = Properties.Settings.Default.ApplicationStoragePrefix;

            CloudStorageClient = CloudStorageAccount.Parse(ConnectionString).CreateCloudTableClient();
            //QueueClient = CloudStorageAccount.Parse(ConnectionString).CreateCloudQueueClient();

            #region Table
            //Insert Table This!!!
            _ClientStore = CloudStorageClient.GetTableReference(PrefixTable + "ClientStore");
            _ApiResourceTable = CloudStorageClient.GetTableReference(PrefixTable + "ApiResource");
            _IdentityTable = CloudStorageClient.GetTableReference(PrefixTable + "Identity");

            //Create If Not Exist
            _ClientStore.CreateIfNotExistsAsync();
            _ApiResourceTable.CreateIfNotExistsAsync();
            _IdentityTable.CreateIfNotExistsAsync();
            #endregion
            #region Queue
            //Insert Queue

            //Create If Not Exist for Queue
            #endregion


        }
        #endregion
        #region Dispose
        private bool _disposed = false;
        ~StorageClient()
        {
            this.Dispose(false);
        }
        private void ThrowIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                CloudStorageClient = null;
                //QueueClient = null;
                //Insert Remove on this!!!!!
                _ClientStore = null;
                _ApiResourceTable = null;
                _IdentityTable = null;
                //Queue

                _disposed = true;
            }
        }
        #endregion
        #region Methods
        #endregion
    }
}
