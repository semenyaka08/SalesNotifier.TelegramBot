namespace SaleNotifier.TelegramBot.Communication;

public class Response
{
    public Format Format { get; set; }
    public string Message { get; set; } = string.Empty;

    public ResponseType ResponseType { get; set; }

    public List<ResponseButton> Buttons { get; set; } = [];
}

public enum ResponseType
{
    WorkflowType,
    CommandType
}

public enum Format
{
    PlainText,
    TextWithKeyboard,
    InlineKeyboard
}

