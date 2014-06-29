using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository
{
    public abstract class AzureStorageContext
    {
        private readonly string _connectionStringName;
        public AzureStorageContext() : this("AzureStorageConnectionString") { }

        public AzureStorageContext(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
            _storageContainers = this.GetType()
                .GetProperties()
                .Where(p => p.PropertyType.IsGenericType && (p.PropertyType.GetGenericTypeDefinition() == typeof(IStorageContainer<>) || p.PropertyType.GetGenericTypeDefinition() == typeof(StorageContainer<>)));

            SetupStorageContainers();
        }

        private void SetupStorageContainers()
        {
            foreach (var storageContainerProperty in _storageContainers)
            {
                storageContainerProperty.SetValue(this, Activator.CreateInstance(typeof(StorageContainer<>).MakeGenericType(storageContainerProperty.PropertyType.GetGenericArguments())));
            }
        }

        private IEnumerable<PropertyInfo> _storageContainers;

        public void SaveChanges()
        {
            foreach (var property in _storageContainers)
            {
                SaveChanges(property.Name, (dynamic)property.GetValue(this));
            }
        }

        private void SaveChanges(string containerName, dynamic storageContainer)
        {
            // Retrieve storage account from connection-string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings[_connectionStringName]);

            // Create the blob client 
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container 
            // Container name must use lower case
            CloudBlobContainer container = blobClient.GetContainerReference(containerName.ToLower());
            container.CreateIfNotExists();

			if (storageContainer.Public)
			{
				var permissions = container.GetPermissions();
				if (permissions.PublicAccess == BlobContainerPublicAccessType.Off)
				{
					permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
					container.SetPermissions(permissions);
				}
			}
			
            foreach (var item in storageContainer)
            {
                CloudBlockBlob blockBlob = GetBlock(item, container);

				WriteBlobFromEntity(blockBlob, item);
				UpdateEntityFromBlob(item,blockBlob);
            }
            foreach(var del in storageContainer.ToRemove)
            {
                var blockBlob = GetBlock(del, container);
                blockBlob.DeleteIfExists();
            }
        }

		private void WriteBlobFromEntity(CloudBlockBlob blob, object entity)
		{
			var entityType = entity.GetType();

			var entityProperties = entityType.GetProperties();

			var uriProperties = entityProperties.Where(_uriFilter);
			var nameProperties = entityProperties.Where(_nameFilter);
			var byteDataProperties = entityProperties.Where(t => t.PropertyType == typeof(byte[]));
			var streamDataProperties = entityProperties.Where(t => t.PropertyType == typeof(Stream));

			entityProperties = entityProperties
				.Except(uriProperties)
				.Except(nameProperties)
				.Except(byteDataProperties)
				.ToArray();
			
			foreach (var prop in entityProperties.Where(p => p.CanRead))
			{
				var propMatches = _propProperties.Where(p => p.Name == prop.Name && p.PropertyType == prop.PropertyType);
				if (propMatches.Any())
				{
					propMatches.First().SetValue(blob.Properties, prop.GetValue(entity));
				}
			}

			if (byteDataProperties.Any())
			{
				byte[] byteData = (byte[])byteDataProperties.First().GetValue(entity);
				if (byteData != null)
				{
					blob.UploadFromByteArray(byteData, 0, byteData.Length);
				}
			}
			else if (streamDataProperties.Any())
			{
				var streamData = (Stream)streamDataProperties.First().GetValue(entity);
				if (streamData != null)
				{
					blob.UploadFromStream(streamData);
				}
			}
			else
			{
				throw new ArgumentException("No data found on the model");
			}
		}

		private void UpdateEntityFromBlob(object entity,CloudBlockBlob blob)
		{
			var entityType = entity.GetType();

			var entityProperties = entityType.GetProperties();

			var uriProperties = entityProperties.Where(_uriFilter);
			var nameProperties = entityProperties.Where(_nameFilter);

			if (uriProperties.Any())
			{
				uriProperties.First().SetValue(entity, blob.Uri);
			}

			entityProperties = entityProperties
				.Except(uriProperties)
				.Except(nameProperties)
				.ToArray();

			foreach (var prop in entityProperties.Where(p => p.CanWrite))
			{
				var propMatches = _propProperties.Where(p => p.Name == prop.Name && p.PropertyType == prop.PropertyType);
				if (propMatches.Any())
				{
					prop.SetValue(entity, propMatches.First().GetValue(blob.Properties));
				}
			}
		}

		private PropertyInfo[] _propProperties = typeof(BlobProperties).GetProperties();

		private Func<PropertyInfo, bool> _uriFilter = t => t.PropertyType == typeof(Uri);
		private Func<PropertyInfo, bool> _nameFilter = t => t.PropertyType == typeof(string) && (t.Name.ToLowerInvariant() == "name" || t.Name.ToLowerInvariant() == "filename");

        private CloudBlockBlob GetBlock(Object item, CloudBlobContainer container)
        {
            var itemType = item.GetType();

            var itemProperties = itemType.GetProperties();

			var uriProperties = itemProperties.Where(_uriFilter);
            var nameProperties = itemProperties.Where(_nameFilter);

            string uriOrName = String.Empty;
			if (uriProperties.Any() && (Uri)uriProperties.First().GetValue(item) != null)
            {
                uriOrName = ((Uri)uriProperties.First().GetValue(item)).ToString();
            }
            else if (nameProperties.Any())
            {
                uriOrName = (string)nameProperties.First().GetValue(item);
            }

            if (string.IsNullOrWhiteSpace(uriOrName))
            {
                throw new ArgumentException("No URI or name found on the model");
            }

            return container.GetBlockBlobReference(uriOrName);
        }
    }

}
