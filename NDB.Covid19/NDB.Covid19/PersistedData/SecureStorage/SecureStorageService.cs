using System;
using NDB.Covid19.Interfaces;
using Plugin.SecureStorage;
using ISecureStorage = Plugin.SecureStorage.Abstractions.ISecureStorage;

namespace NDB.Covid19.PersistedData.SecureStorage
{
    public class SecureStorageService : ISecureStorageService
    {
        private ISecureStorage _secureStorage;

        public ISecureStorage SecureStorage
        {
            get
            {
                if (_secureStorage == null)
                {
                    _secureStorage = CrossSecureStorage.Current;
                }

                return _secureStorage;
            }
        }

        public bool SaveValue(string key, string value)
        {
            return SecureStorage.SetValue(key, value);
        }

        public string GetValue(string key)
        {
            try
            {
                return SecureStorage.GetValue(key);
            }
            catch (Exception)
            {
                SecureStorage.DeleteKey(key);
                return null;
            }
        }

        public bool KeyExists(string key)
        {
            return SecureStorage.HasKey(key);
        }

        public void Delete(string key)
        {
            if (KeyExists(key))
            {
                SecureStorage.DeleteKey(key);
            }
        }

        /// <summary>
        ///     [Only use for unit testing purpose] Set Instance for SecureStorage
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public void SetSecureStorageInstance(ISecureStorage instance)
        {
            _secureStorage = instance;
        }
    }
}