// https://github.com/awaescher/OllamaSharp
using OllamaSharp;
using OllamaSharp.Models.Chat;
using static System.Net.Mime.MediaTypeNames;

var uri = new Uri("http://127.0.0.1:11434");
var ollama = new OllamaApiClient(uri);

// select a model which should be used for further operations
ollama.SelectedModel = "llama3.2";
//ollama.SelectedModel = "gemma2";

/* Test 1

await foreach (var stream in ollama.Generate("How are you today?"))
    Console.Write(stream.Response);

var chat = new Chat(ollama);
while (true)
{
    var message = Console.ReadLine();
    await foreach (var answerToken in chat.Send(message))
        Console.Write(answerToken);
    Console.WriteLine("?");
}
// messages including their roles and tool calls will automatically be tracked within the chat object
// and are accessible via the Messages property

Test1 */



var chatRequest = new ChatRequest
	{
		Model = "model",
		Messages = [
				new(ChatRole.User, "Why?"),
				new(ChatRole.Assistant, "Because!"),
				new(ChatRole.User, "And where?")]
	};

    ollama.Chat(chatRequest);



var chat = new Chat(ollama);
while (true)
{
    var message = Console.ReadLine();
    await foreach (var answerToken in chat.Send(message))
        Console.Write(answerToken);
    Console.WriteLine("?");
}
// messages including their roles and tool calls will automatically be tracked within the chat object
// and are accessible via the Messages property

