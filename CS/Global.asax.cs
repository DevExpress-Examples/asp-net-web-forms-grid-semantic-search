using Azure.AI.OpenAI;
using DevExpress.AIIntegration;
using Microsoft.Extensions.AI;
using System;
using System.ClientModel;

namespace ASPxGridViewAIIntegration {
    public class Global_asax : System.Web.HttpApplication {
        void Application_Start(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxWebControl.CallbackError += new EventHandler(Application_Error);
            DevExpress.Security.Resources.AccessSettings.DataResources.SetRules(
                DevExpress.Security.Resources.DirectoryAccessRule.Allow(Server.MapPath("~/Content")),
                DevExpress.Security.Resources.UrlAccessRule.Allow()
            );

            // Replace with your endpoint, API key, and deployed AI model name.
            var azureOpenAIEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
            var azureOpenAIKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");

            var credentials = new ApiKeyCredential(azureOpenAIKey);
            var openAI = new AzureOpenAIClient(
                new Uri(azureOpenAIEndpoint),
                credentials
            );

            IChatClient chatClient = openAI
                .GetChatClient("gpt-4o-mini")
                .AsIChatClient();

            var embeddingGenerator = openAI
                .GetEmbeddingClient("text-embedding-3-small")
                .AsIEmbeddingGenerator();

            Application["EmbeddingGenerator"] = embeddingGenerator;
        }


        void Application_End(object sender, EventArgs e) {
            // Code that runs on application shutdown
        }
    
        void Application_Error(object sender, EventArgs e) {
            // Code that runs when an unhandled error occurs
        }
    
        void Session_Start(object sender, EventArgs e) {
            // Code that runs when a new session is started
        }
    
        void Session_End(object sender, EventArgs e) {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }
    }
}