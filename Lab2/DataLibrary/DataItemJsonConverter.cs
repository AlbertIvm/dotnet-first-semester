using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataLibrary
{
    public class DataItemJsonConverter : JsonConverter<DataItem>
    {
        public override void Write(Utf8JsonWriter writer, DataItem data, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", data.X);
            writer.WriteNumber("Y", data.Y);
            writer.WriteNumber("ValueReal", data.Value.Real);
            writer.WriteNumber("ValueImaginary", data.Value.Imaginary);
            writer.WriteEndObject();
        }

        public override DataItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            double x = 0;
            double y = 0;
            double real = 0;
            double imag = 0;

            string propertyName = "";
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new DataItem(x, y, new Complex(real, imag));
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
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
                    }
                }
            }
            throw new JsonException();
        }
    }
}