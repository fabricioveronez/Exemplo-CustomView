using System;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace ExemploCustomView
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

            PredictionEndpoint endpoint = new PredictionEndpoint() { ApiKey = "API KEY" };
            ImagePrediction resultado = null;

            if (eURL)
            {
                resultado = endpoint.PredictImageUrl(new Guid("Id do Projeto"), new ImageUrl(caminho), new Guid("Id Interação"));
            }
            else
            {
                resultado = endpoint.PredictImage(Guid.NewGuid(), File.OpenRead(caminho), new Guid("36ca5bfe-03fd-44bd-aecd-fb6987504362"));
            }

            foreach (var possibilidade in resultado.Predictions)
            {
                Console.WriteLine($"Tag: {possibilidade.TagName}");
                Console.WriteLine($"Possibilidade: {possibilidade.Probability:P1}");                
            }
        }
    }
}
