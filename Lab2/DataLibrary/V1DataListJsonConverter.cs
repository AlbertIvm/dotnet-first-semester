using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataLibrary
{
    public class V1DataListJsonConverter : JsonConverter<V1DataList>
    {
        public override void Write(Utf8JsonWriter writer, V1DataList data, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("ID", data.ID);
            writer.WriteString("Timestamp", data.Timestamp);
            writer.WriteStartArray("Data");
            foreach (var item in data)
            {
                writer.WriteRawValue(JsonSerializer.Serialize(item));
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public override V1DataList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            string id = "";
            DateTime timestamp = new();
            List<DataItem> itemsBuffer = new();

            double x = 0;
            double y = 0;
            double real = 0;
            double imag = 0;

            string propertyName = "";
            bool readingData = false;
            bool readingItem = false;
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        if (readingItem) throw new JsonException();
                        else
                        {
                            readingItem = true;
                            x = y = real = imag = 0;
                        }
                        break;
                    case JsonTokenType.StartArray:
                        if (readingData) throw new JsonException();
                        else readingData = true;
                        break;
                    case JsonTokenType.EndArray:
                        if (!readingData) throw new JsonException();
                        else readingData = false;
                        break;
                    case JsonTokenType.EndObject:
                        if (readingItem)
                        {
                            itemsBuffer.Add(new DataItem(x, y, new Complex(real, imag)));
                            readingItem = false;
                        }
                        else
                        {
                            return new V1DataList(id, timestamp, in itemsBuffer);
                        }
                        break;
                    case JsonTokenType.PropertyName:
                        propertyName = reader.GetString();
                        if (propertyName == "Data") continue;

                        reader.Read();
                        switch (propertyName)
                        {
                            case "X":
                                x = reader.GetDouble();
                                break;
                            case "Y":
                                y = reader.GetDouble();
                                break;
                            case "ValueReal":
                                real = reader.GetDouble();
                                break;
                            case "ValueImaginary":
                                imag = reader.GetDouble();
                                break;
                            case "ID":
                                id = reader.GetString();
                                break;
                            case "Timestamp":
                                timestamp = reader.GetDateTime();
                                break;
                        }
                        break;
                }
            }
            throw new JsonException();
        }
    }
}