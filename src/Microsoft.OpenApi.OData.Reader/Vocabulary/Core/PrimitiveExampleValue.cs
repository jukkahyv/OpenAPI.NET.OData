﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OpenApi.OData.Vocabulary.Core
{
    /// <summary>
    /// Complex type: Org.OData.Core.V1.PrimitiveExampleValue.
    /// </summary>
    internal class PrimitiveExampleValue : ExampleValue
    {
        /// <summary>
        /// Gets the Example value for the custom parameter
        /// </summary>
        public ODataPrimitiveValue Value { get; private set; }

        /// <summary>
        /// Init the <see cref="PrimitiveExampleValue"/>
        /// </summary>
        /// <param name="record">The input record.</param>
        public override void Initialize(IEdmRecordExpression record)
        {
            // Load ExampleValue
            base.Initialize(record);

            // Value of PrimitiveExampleValue
            IEdmPropertyConstructor property = record.FindProperty("Value");
            if (property != null)
            {
                Value = property.Value.Convert() as ODataPrimitiveValue;
            }
        }
    }
}
