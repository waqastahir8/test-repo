using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string dateString = reader.GetString()!;
            if (DateOnly.TryParse(dateString, out DateOnly date))
            {
                return date;
            }
            else
            {
                throw new JsonException($"Invalid date format: {dateString}");
            }
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            int year = 0, month = 0, day = 0;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break; 
                }
                else if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString()!;
                    reader.Read(); 
                    switch (propertyName)
                    {
                        case "year":
                            year = reader.GetInt32();
                            break;
                        case "month":
                            month = reader.GetInt32();
                            break;
                        case "day":
                            day = reader.GetInt32();
                            break;
                        default:
                            reader.Skip();
                            break;
                    }
                }
            }
            return new DateOnly(year, month, day);
        }
        else
        {
            throw new JsonException($"Unexpected token type: {reader.TokenType}");
        }
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
    }
}