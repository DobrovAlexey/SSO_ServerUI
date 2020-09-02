// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace IdentityServer
{
    // todo: Реализовать загрузку скоупов и клиентов из базы.
    public static class Config
    {

        //  Представление утверждений о пользователе (id, имя, почта и т.д.)
        public static IEnumerable<IdentityResource> IdentityResources =>
            new []
            {
                // Обязательный ресурс.
                new IdentityResources.OpenId(), 

                // Profile содержит:
                // name, family_name, given_name, middle_name, nickname, preferred_username, profile,
                // picture, website, gender, birthdate, zoneinfo, locale, updated_at

                new IdentityResources.Profile(),
                //new IdentityResources.Email(),

                // Кастомное утверждение о пользователе.
                new IdentityResource(
                    name: "info_223",
                    displayName: "223 data",
                    userClaims: new[]
                    {
                        "organization_223",
                        "name_223"
                    })
            };

        // Ресурсы API: представляют функции, к которым клиент хочет получить доступ.
        public static IEnumerable<ApiScope> ApiScopes =>
            new []
            {
                new ApiScope("api"),

                new ApiScope(name: "read",   displayName: "Read your data."),
                new ApiScope(name: "write",  displayName: "Write your data."),
                new ApiScope(name: "delete", displayName: "Delete your data."),
            };

        // Добавление клиентов.
        public static IEnumerable<Client> Clients =>
            new []
            {
                #region clients

                // m2m client credentials flow client
                //new Client
                //{
                //    ClientId = "m2m.client",
                //    ClientName = "Client Credentials Client",

                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                //    AllowedScopes = { "scope1" }
                //},


                // interactive client using code flow + pkce
                //new Client
                //{
                //    ClientId = "interactive",
                //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,

                //    RedirectUris = { "https://localhost:44300/signin-oidc" },
                //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //    AllowOfflineAccess = true,
                //    AllowedScopes = { "openid", "profile", "scope2" }
                //},

                #endregion

                // machine to machine client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = { "api" }
                },

                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    //AllowOfflineAccess = true,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,

                        "api",
                        "read",
                        "info_223"
                    }
                }
            };
    }
}