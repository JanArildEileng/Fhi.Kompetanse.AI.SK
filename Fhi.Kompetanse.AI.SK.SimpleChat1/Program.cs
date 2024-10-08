using Fhi.Kompetanse.AI.SK.ChatLoghelper.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Fhi.Kompetanse.AI.SK.SimpleChat1;

#pragma warning disable SKEXP0070 
#pragma warning disable SKEXP0010
public static class Program
{
    private static DumpLoggingProvider _loggingProvider = new DumpLoggingProvider();

    private static String OllamaEndpoint = @"http://127.0.0.1:11434";
    private static String OllamaModel = @"llama3";

    static async Task Main(string[] args)
    {
        await OllamaChatCompletionTest();
        Console.ReadLine();
    }

    public static async Task OllamaChatCompletionTest()
    {
        Kernel kernel = CreateBasicKernelBuilder().Build();
    
        var aiChatService = kernel.GetRequiredService<IChatCompletionService>();
        var chatHistory = new ChatHistory();

        while (true)
        {
            // Get user prompt and add to chat history
            Console.WriteLine("Your prompt:");
            var userPrompt = Console.ReadLine();
            chatHistory.Add(new ChatMessageContent(AuthorRole.User, userPrompt));

            // Stream the AI response and add to chat history
            Console.WriteLine("AI Response:");
            var response = "";
            await foreach (var item in
                aiChatService.GetStreamingChatMessageContentsAsync(chatHistory))
            {
                Console.Write(item.Content);
                response += item.Content;
            }
            chatHistory.Add(new ChatMessageContent(AuthorRole.Assistant, response));
            Console.WriteLine();


        }
    }


     private static IKernelBuilder CreateBasicKernelBuilder()
    {
        var kernelBuilder = Kernel.CreateBuilder();

        kernelBuilder.AddOllamaChatCompletion(modelId: OllamaModel, endpoint: new Uri(OllamaEndpoint));
        //bruke dette i AspNetCore
        //builder.Services.AddScoped<IChatCompletionService>((_)=>new OllamaChatCompletionService(endpoint: new Uri(OllamaEndpoint), modelId: OllamaModel));

        kernelBuilder.Services.AddLogging(l => l
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole()
            .AddDebug()
            .AddProvider(_loggingProvider)
        );

        kernelBuilder.Services.ConfigureHttpClientDefaults(c =>
        {
            c.AddLogger(s => _loggingProvider.CreateHttpRequestBodyLogger(s.GetRequiredService<ILogger<DumpLoggingProvider>>()));
            c.AddStandardResilienceHandler(options =>
            {
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(45);
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(15);
                options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(5);
            });
        });

        return kernelBuilder;
    }

}