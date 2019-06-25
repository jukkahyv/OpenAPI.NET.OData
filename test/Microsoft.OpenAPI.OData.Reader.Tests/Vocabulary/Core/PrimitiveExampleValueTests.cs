﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Vocabularies;
using Microsoft.OpenApi.OData.Edm;
using Microsoft.OpenApi.OData.Vocabulary.Core;
using Xunit;

namespace Microsoft.OpenApi.OData.Reader.Vocabulary.Core.Tests
{
    public class PrimitiveExampleValueTests
    {
        [Fact]
        public void DefaultPropertyAsNull()
        {
            // Arrange
            PrimitiveExampleValue value = new PrimitiveExampleValue();

            //  Act & Assert
            Assert.Null(value.Description);
            Assert.Null(value.Value);
        }

        [Fact]
        public void InitializeWithNullThrows()
        {
            // Arrange & Act
            PrimitiveExampleValue value = new PrimitiveExampleValue();

            // Assert
            Assert.Throws<ArgumentNullException>("record", () => value.Initialize(record: null));
        }

        [Fact]
        public void InitializeWithNullThrows1()
        {
            // Arrange & Act
            PrimitiveExampleValue value = new PrimitiveExampleValue();
            IEdmRecordExpression record = new EdmRecordExpression();

            // Assert
          //  Assert.Throws<ArgumentNullException>("record", () => value.Initialize(record: null));
        }

        [Theory]
        [InlineData(@"String""=""Hello World""")]
        public void TargetOnEntityTypeReturnsCorrectTopSupportedValue1(string data)
        {
            // Arrange
            string annotation = $@"<Annotation Term=""Org.OData.Core.V1.Example"">
                <Record Type=""Org.OData.Core.V1.PrimitiveExampleValue"">
                  <PropertyValue Property=""Description"" String=""Primitive example value"" />
                  <PropertyValue Property=""Value"" {data} />
                </Record>";

            IEdmModel model = GetEdmModel(annotation);
            Assert.NotNull(model); // guard

            IEdmEntityType customer = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Customer");
            Assert.NotNull(customer); // guard
            IEdmProperty dataProperty = customer.FindProperty("Data");
            Assert.NotNull(dataProperty);

            // Act
            PrimitiveExampleValue value = model.GetRecord<PrimitiveExampleValue>(dataProperty, "Org.OData.Core.V1.Example");

            // Assert
            Assert.NotNull(value);
            Assert.Equal("Primitive example value", value.Description);
            Assert.NotNull(value.Value);
        }

        private IEdmModel GetEdmModel(string annotation)
        {
            const string template = @"<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""NS"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityType Name=""Customer"">
        <Key>
          <PropertyRef Name=""ID"" />
        </Key>
        <Property Name=""ID"" Type=""Edm.Int32"" Nullable=""false"" />
        <Property Name=""Data"" Type=""Edm.PrimitiveType"" Nullable=""false"" >
          {0}
        </Property>
      </EntityType>
      <EntityContainer Name =""Default"">
         <Singleton Name=""Me"" Type=""NS.Customer"" />
      </EntityContainer>
      <Annotations Target=""NS.Customer/ID"">
        {0}
      </Annotations>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";
            string modelText = string.Format(template, annotation);

            IEdmModel model;

            bool result = CsdlReader.TryParse(XElement.Parse(modelText).CreateReader(), out model, out _);
            Assert.True(result);
            return model;
        }
    }
}
