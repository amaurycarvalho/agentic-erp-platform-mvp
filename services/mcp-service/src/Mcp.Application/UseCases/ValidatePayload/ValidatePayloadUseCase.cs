using System.Text.Json;
using Mcp.Application.UseCases.ExecuteTool;
using Mcp.Domain.Tools;

namespace Mcp.Application.UseCases.ValidatePayload;

public sealed class ValidatePayloadUseCase
{
    public void Execute(McpToolContract tool, JsonElement payload)
    {
        foreach (var field in tool.InputSchema)
        {
            if (!field.Required)
            {
                continue;
            }

            if (!payload.TryGetProperty(field.Name, out var value))
            {
                throw new ToolValidationException($"Field '{field.Name}' is required.");
            }

            ValidateFieldType(field, value);
            ValidateConstraints(field, value);
        }
    }

    private static void ValidateFieldType(McpToolFieldContract field, JsonElement value)
    {
        var validType = field.Type switch
        {
            "string" => value.ValueKind == JsonValueKind.String,
            "number" => value.ValueKind == JsonValueKind.Number,
            "boolean" => value.ValueKind is JsonValueKind.True or JsonValueKind.False,
            _ => true
        };

        if (!validType)
        {
            throw new ToolValidationException($"Field '{field.Name}' must be of type '{field.Type}'.");
        }
    }

    private static void ValidateConstraints(McpToolFieldContract field, JsonElement value)
    {
        if (string.IsNullOrWhiteSpace(field.Constraints))
        {
            return;
        }

        switch (field.Constraints)
        {
            case "must_not_be_empty":
                if (value.ValueKind == JsonValueKind.String && string.IsNullOrWhiteSpace(value.GetString()))
                {
                    throw new ToolValidationException($"Field '{field.Name}' must not be empty.");
                }
                break;

            case "greater_than_zero":
                if (value.ValueKind != JsonValueKind.Number || !value.TryGetDecimal(out var numericValue) || numericValue <= 0)
                {
                    throw new ToolValidationException($"Field '{field.Name}' must be greater than zero.");
                }
                break;
        }
    }
}
