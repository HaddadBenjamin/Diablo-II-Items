﻿using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace DiabloII.Items.Api.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static void AddAzureKeyVault(this IConfigurationBuilder configurationBuilder)
        {
            var vaultEndpoint = "https://benjamintestvault.vault.azure.net/";
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var defaultKeyVaultSecretManager = new DefaultKeyVaultSecretManager();
            var authentificationCallback = new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback);
            var keyVaultClient = new KeyVaultClient(authentificationCallback);

            configurationBuilder.AddAzureKeyVault(vaultEndpoint, keyVaultClient, defaultKeyVaultSecretManager);
        }
    }
}