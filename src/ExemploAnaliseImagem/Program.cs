using System;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace ExemploAnalise
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Entre com o caminho ou a URL da imagem.");
            string caminhoURL = Console.ReadLine();
            EfetuarTesteImagem(caminhoURL, Uri.IsWellFormedUriString(caminhoURL, UriKind.Absolute));
        }

        static void EfetuarTesteImagem(string caminho, bool eURL)
        {

            string chaveAPI = "0c91a890892d40a0ba9f9e22a5e358de";
            string chaveProjeto = "4a1a0670-7689-4d01-ac59-fc56227cc3ed";

            // Crio o client do Custom View Service.
            PredictionEndpoint endpoint = new PredictionEndpoint() { ApiKey = chaveAPI };
            ImagePrediction resultado = null;

            // Verifico se o projeto é uma URL ou um arquivo local para usar o método correto.
            if (eURL)
            {
                resultado = endpoint.PredictImageUrl(new Guid(chaveProjeto), new ImageUrl(caminho));
            }
            else
            {
                resultado = endpoint.PredictImage(new Guid(chaveProjeto), File.OpenRead(caminho));
            }

            // Percorro as analises.
            foreach (var possibilidade in resultado.Predictions)
            {
                Console.WriteLine($"Tag: {possibilidade.TagName}");
                Console.WriteLine($"Possibilidade: {possibilidade.Probability:P1}");
            }
        }
    }
}
