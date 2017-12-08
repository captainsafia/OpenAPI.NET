﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Readers.ParseNodes;

namespace Microsoft.OpenApi.Readers.V2
{
    /// <summary>
    /// Class containing logic to deserialize Open API V2 document into
    /// runtime Open API object model.
    /// </summary>
    internal static partial class OpenApiV2Deserializer
    {
        private static void ParseMap<T>(
            MapNode mapNode,
            T domainObject,
            FixedFieldMap<T> fixedFieldMap,
            PatternFieldMap<T> patternFieldMap,
            List<string> requiredFields = null)
        {
            if (mapNode == null)
            {
                return;
            }

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(domainObject, fixedFieldMap, patternFieldMap);
                requiredFields?.Remove(propertyNode.Name);
            }
        }

        private static void ReportMissing(ParseNode node, IList<string> required)
        {
            foreach (var error in required.Select(
                    r => new OpenApiError(
                        node.Context.GetLocation(),
                        $"{r} is a required property"))
                .ToList())
            {
                node.Diagnostic.Errors.Add(error);
            }
        }

        private static string LoadString(ParseNode node)
        {
            return node.GetScalarValue();
        }
    }
}