﻿using System;
using Newtonsoft.Json.Schema;
using System.Linq;
using System.Collections.Generic;

namespace Wham
{
    public static class JSchemaExtensions
    {
        public static readonly Dictionary<JSchemaType, string> AtomicTypeNames = new Dictionary<JSchemaType,string>()
        {
            { JSchemaType.Integer, "int" },
            { JSchemaType.Number, "double" },
            { JSchemaType.Boolean, "bool" },
            { JSchemaType.String, "string" },
            { JSchemaType.Array, "List<>" },
        };

        public static bool IsAtomicType(this JSchema schema)
        {
            return schema != null && schema.Type != null
            && (
                schema.Type == JSchemaType.Integer
                || schema.Type == JSchemaType.Number
                || schema.Type == JSchemaType.Boolean
                || schema.Type == JSchemaType.String
                || schema.Type == JSchemaType.Array
            );
        }

        public static string GetSchemaClrType(this JSchema schema)
        {
            if (schema.IsAtomicType())
            {
                if (schema.Type == JSchemaType.Array)
                {
                    return "List<object>";
                }
                else
                    return AtomicTypeNames[schema.Type.Value];
            }
            else
            {
//                if (schema.Type == JSchemaType.Object)
//                {
//                }
//                else
                return "object";
            }
        }

        public static JSchema GetBaseSchema(this JSchema schema)
        {
            if (schema != null)
            {
                if (schema.AllOf != null && schema.AllOf.Any())
                {
                    return schema.AllOf.FirstOrDefault(a => !a.IsAtomicType() && !string.IsNullOrEmpty(a.Title));
                } 
            }

            return null;
        }
    }
}

