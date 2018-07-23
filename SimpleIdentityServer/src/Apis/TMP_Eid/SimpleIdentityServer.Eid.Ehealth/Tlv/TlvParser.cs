﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleIdentityServer.Eid.Ehealth.Tlv
{
    public interface ITlvParser
    {
        T Parse<T>(byte[] file);
    }

    public sealed class TlvParser : ITlvParser
    {
        public T Parse<T>(byte[] file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var properties = typeof(T).GetProperties();
            var tlvFields = new Dictionary<int, PropertyInfo>();
            foreach(var property in properties)
            {
                var attrs = property.GetCustomAttributes(true).Where(a => a is TlvFieldAttribute);
                if (attrs != null && attrs.Any())
                {
                    var tlvField = attrs.First() as TlvFieldAttribute;
                    tlvFields.Add(tlvField.Value, property);
                }
            }

            var result = (T)Activator.CreateInstance(typeof(T));
            int i = 0;
            while (i < file.Length - 1)
            {
                var tag = file[i];
                i++;
                var lengthByte = file[i];
                int length = lengthByte & 0x7f;
                while ((lengthByte & 0x80) == 0x80)
                {
                    i++;
                    lengthByte = file[i];
                    length = (length << 7) + (lengthByte & 0x7f);
                }

                i++;
                if (0 == tag)
                {
                    i += length;
                    continue;
                }

                var field = tlvFields.FirstOrDefault(t => t.Key == tag);
                if (!field.Equals(default(KeyValuePair<int, PropertyInfo>)))
                {
                    var tlvValue = Copy(file, i, length);
                    var tlvField = field.Value;
                    var propertyType = tlvField.PropertyType;
                    object fieldValue = null;
                    if (propertyType == typeof(string))
                    {
                        fieldValue = Encoding.UTF8.GetString(tlvValue);
                    }

                    tlvField.SetValue(result, fieldValue);
                }

                i += length;
            }

            return result;
        }

        private static byte[] Copy(byte[] source, int idx, int count)
        {
            var result = new byte[count];
            Array.Copy(source, idx, result, 0, count);
            return result;
        }
    }
}
