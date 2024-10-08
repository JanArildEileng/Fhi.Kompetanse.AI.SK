// See https://github.com/openai/openai-dotnet

using OpenAI.Chat;
using OpenAI;
using static System.Net.WebRequestMethods;
using System.ClientModel;



OpenAIClientOptions openAIClientOptions = new OpenAIClientOptions() {
    Endpoint = new Uri("http://127.0.0.1:11434")
    
};

ApiKeyCredential credential = new ApiKeyCredential("ollama");


ChatClient client = new ChatClient(model: "llama3.2", credential,openAIClientOptions);


//ChatClient client = new OpenAI.OpenAIClient(OpenAIClientOptions(base_url = 'http://localhost:11434/v1',api_key='ollama') # required, but unused





ChatCompletion completion = client.CompleteChat("Say 'this is a test.'");

Console.WriteLine($"[ASSISTANT]: {completion.Content[0].Text}");
