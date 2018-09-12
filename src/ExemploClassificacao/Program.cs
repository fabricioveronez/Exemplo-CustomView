using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace ExemploClassificacao
{
    class Program
    {
        static void Main(string[] args)
        {
            string chaveClassificacao = "<Chave de treinamento>";
            string idProjeto = "<ID Projeto>";

            // Crio o cliente para o Azure Custom View
            TrainingApi trainingApi = new TrainingApi() { ApiKey = chaveClassificacao };

            Console.WriteLine("Iniciando as tags.");

            // Crio a tag shitzu                        
            Tag shitzuTag = trainingApi.CreateTag(new Guid(idProjeto), "Shitzu");
            // Crio a tag poodle
            Tag poodleTag = trainingApi.CreateTag(new Guid(idProjeto), "Poodle");

            // Carrego as imagens de shitzu
            string[] imagensShitzu = Directory.GetFiles(Path.Combine("Amostras", "Shitzu")).ToArray();

            // Carrego as imagens de poodle
            string[] imagensPoodle = Directory.GetFiles(Path.Combine("Amostras", "Poodle")).ToArray();

            Console.WriteLine("Cadastrando as imagens de shitzu.");

            // Cadastro das imagens de shitzu
            foreach (var imagem in imagensShitzu)
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(imagem)))
                {
                    trainingApi.CreateImagesFromData(new Guid(idProjeto), stream, new List<string>() { shitzuTag.Id.ToString() });
                }
            }

            Console.WriteLine("Cadastrando as imagens de poodle.");

            // Cadastro das imagens de poodle
            foreach (var imagem in imagensPoodle)
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(imagem)))
                {
                    trainingApi.CreateImagesFromData(new Guid(idProjeto), stream, new List<string>() { poodleTag.Id.ToString() });
                }
            }

            // Inicio o treinamento com as imagens cadastradas.
            Iteration interacao = trainingApi.TrainProject(new Guid(idProjeto));

            Console.WriteLine("Treinando.");

            // Verifico periodicamente até concluir
            while (interacao.Status == "Training")
            {
                Thread.Sleep(1000);

                // Verifico novamente o status
                interacao = trainingApi.GetIteration(new Guid(idProjeto), interacao.Id);
            }

            // Agora que o treinamento está concluído, configuro como o endpoint padrão.
            interacao.IsDefault = true;
            trainingApi.UpdateIteration(new Guid(idProjeto), interacao.Id, interacao);
            Console.WriteLine("Finalizado.");
        }
    }
}
