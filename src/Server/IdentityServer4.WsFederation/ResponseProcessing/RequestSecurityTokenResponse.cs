// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Protocols.WsFederation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Xml;

namespace IdentityServer4.WsFederation
{
    internal class RequestSecurityTokenResponse
    {
        public DateTime CreatedAt { get;set; }
        public DateTime ExpiresAt { get;set; }
        public string AppliesTo { get; set; }
        public string Context { get; set; }
        public string ReplyTo { get; set; }
        public SecurityToken RequestedSecurityToken { get; set; }
        public SecurityTokenHandler SecurityTokenHandler { get; set; }

        public string Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, false))
                {
                    // <t:RequestSecurityTokenResponseCollection>
                    //TODO: check if collection is required
                    writer.WriteStartElement(WsTrustConstants_1_3.PreferredPrefix, WsFederationConstants.Elements.RequestSecurityTokenResponseCollection, WsTrustConstants.Namespaces.WsTrust1_3);
                    // <t:RequestSecurityTokenResponse>
                    writer.WriteStartElement(WsTrustConstants_1_3.PreferredPrefix, WsTrustConstants.Elements.RequestSecurityTokenResponse, WsTrustConstants.Namespaces.WsTrust1_3);
                    // @Context
                    writer.WriteAttributeString(WsFederationConstants.Attributes.Context, Context);

                    // <t:Lifetime>
                    writer.WriteStartElement(WsTrustConstants.Elements.Lifetime, WsTrustConstants.Namespaces.WsTrust1_3);

                    // <wsu:Created></wsu:Created>
                    writer.WriteElementString(WsUtility.PreferredPrefix, WsUtility.Elements.Created, WsFederationConstants.WsUtility.Namespace, CreatedAt.ToString(SamlConstants.GeneratedDateTimeFormat, DateTimeFormatInfo.InvariantInfo));
                    // <wsu:Expires></wsu:Expires>
                    writer.WriteElementString(WsUtility.PreferredPrefix, WsUtility.Elements.Expires, WsFederationConstants.WsUtility.Namespace, ExpiresAt.ToString(SamlConstants.GeneratedDateTimeFormat, DateTimeFormatInfo.InvariantInfo));

                    // </t:Lifetime>
                    writer.WriteEndElement();

                    // <wsp:AppliesTo>
                    writer.WriteStartElement(WsPolicy.PreferredPrefix, WsPolicy.Elements.AppliesTo, WsPolicy.Namespace);

                    // <wsa:EndpointReference>
                    writer.WriteStartElement(WsAddressing.PreferredPrefix, WsAddressing.Elements.EndpointReference, WsAddressing.Namespace);

                    // <wsa:Address></wsa:Address>
                    writer.WriteElementString(WsAddressing.PreferredPrefix, WsAddressing.Elements.Address, WsAddressing.Namespace, AppliesTo);

                    writer.WriteEndElement();
                    // </wsa:EndpointReference>

                    writer.WriteEndElement();
                    // </wsp:AppliesTo>

                    // <t:RequestedSecurityToken>
                    writer.WriteStartElement(WsTrustConstants_1_3.PreferredPrefix, WsTrustConstants.Elements.RequestedSecurityToken, WsTrustConstants.Namespaces.WsTrust1_3);

                    // write assertion
                    SecurityTokenHandler.WriteToken(writer, RequestedSecurityToken);

                    // </t:RequestedSecurityToken>
                    writer.WriteEndElement();

                    // </t:RequestSecurityTokenResponse>
                    writer.WriteEndElement();

                    // <t:RequestSecurityTokenResponseCollection>
                    writer.WriteEndElement();

                    writer.Flush();
                    var result = Encoding.UTF8.GetString(ms.ToArray());
                    return result;
                }
            }
        }
    }
}