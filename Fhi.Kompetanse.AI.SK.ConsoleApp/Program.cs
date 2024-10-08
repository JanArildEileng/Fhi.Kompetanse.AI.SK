using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Connectors.Ollama;
using OllamaSharp;

#pragma warning disable SKEXP0001 

String model = "llama3.1";

// Create a kernel with Azure OpenAI chat completion
IKernelBuilder builder = Kernel.CreateBuilder();//  .AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);

// Add enterprise components
builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

//OllamaApiClient ollamaApiClient = new OllamaApiClient("http://127.0.0.1:11434",model);

//builder.Services.AddScoped<IChatCompletionService>((_)=>new OllamaChatCompletionService(modelId: model,ollamaClient: ollamaApiClient,loggerFactory:null));
builder.Services.AddScoped<IChatCompletionService>((_)=>new OllamaChatCompletionService(endpoint: new Uri("http://127.0.0.1:11434"), modelId: model));



// Build the kernel
Kernel kernel = builder.Build();

// Add a plugin (the LightsPlugin class is defined below)
 kernel.Plugins.AddFromType<LightsPlugin>("Lights");

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

/*
  var chatCompletionService = new OllamaChatCompletionService(
            endpoint: new Uri("http://127.0.0.1:11434"),
            modelId: model);
*/

await kernel.InvokeAsync("Lights", "get_lights", null);


// Enable planning
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new() 
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
     ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    //Temperature = .1,
    //ChatSystemPrompt = @"Hello:"
};

// Create a history store the conversation
var history = new ChatHistory();

// Initiate a back-and-forth chat
string? userInput;
do
{
    // Collect user input
    Console.Write("User > ");
    userInput = Console.ReadLine();

    // Add user input
    history.AddUserMessage(userInput);

    // Get the response from the AI
    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    // Print the results
    Console.WriteLine("Assistant > " + result);

    // Add the message from the agent to the chat history
    history.AddMessage(result.Role, result.Content ?? string.Empty);


} while (userInput is not null);
