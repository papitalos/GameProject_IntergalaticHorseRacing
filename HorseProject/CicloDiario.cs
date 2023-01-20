﻿using HorseProject;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;


namespace HorseProject
{
    public static class CicloDiario
    {
        public static int currentAudio;
        public static Cavalo cavalo;
        public static int numVitoria = 1, escolhaCorrida = BootJogo.escolhaCorrida;
        public static bool pistaAberta = true,lojaAberta = true;
        public static string[] cicloRelogio = new string[4] { "Manhã    ", "Tarde    ", "Noite    ", "Madrugada" };
        public static string[] diasDaSemana = new string[7] { "Segunda-feira         ","Terça-feira           ","Quarta-feira          ","Quinta-feira          ","Sexta-feira           ","Sabado                ","Domingo               " };
        public static string horaDoDiaAtual, diaAtual;                                                                                                                                                                                                                                               
        public static int myDelay = 5000;
        public static int contadorDia = 1, diasSemComer = 0, i;
        public static Thread dia = new Thread(PassarTempo);//Criar uma nova thread para rodar o relogio do dia de maneira independente
        
        public static void Musica(int audio)
        {
            Thread musica = new Thread(new ThreadStart(() => TocarMusica(audio)));
            musica.Start();
            musica.Join();
        }

        public static void TocarMusica(int audio)
        {
            System.Media.SoundPlayer player;
            if (audio == 1)
            {
                player = new System.Media.SoundPlayer(@"C:\Users\italo\Source\Repos\papitalos\IntergalaticHorseRacing\HorseProject\Menu.wav");
                player.Play();
            }
            else if (audio == 2)
            {
                player = new System.Media.SoundPlayer(@"C:\Users\italo\Source\Repos\papitalos\IntergalaticHorseRacing\HorseProject\Som_de_trompetas.wav");
                player.Play();
            }
            else if (audio == 3)
            {
                player = new System.Media.SoundPlayer(@"C:\Users\italo\Source\Repos\papitalos\IntergalaticHorseRacing\HorseProjectCorrida.wav");
                player.Play();
            }
            currentAudio = audio;
        }

        public static void SaveGameData(string gameData, string filePath)
        {
            using (StreamWriter file = new StreamWriter(File.Open(filePath, FileMode.Create)))
            {
                file.Write(gameData);
            }
            Console.WriteLine("Informações do jogo salvas com sucesso em: " + filePath);
        }


        public static void ThreadTimerDiario()
        {

            while (BootJogo.estaRodando == true)
            {
              
                if (BootJogo.menuAtual == BootJogo.menu.menuEscolhaInicial || BootJogo.menuAtual == BootJogo.menu.menuInicial || BootJogo.menuAtual == BootJogo.menu.menuLoading)
                {
                    dia.Suspend();
                }
                else if(BootJogo.menuAtual == BootJogo.menu.menuSleep)
                {
                    dia.Abort();
                    i = 0;
                    dia.Start();
                }
                else
                {
                    dia.Resume();
                }
            }
            
           

        }




        public static void PassarTempo()
        {

            while (BootJogo.estaRodando == true)
            {
                for (contadorDia = 0; contadorDia < 7; contadorDia++)
                {
                    lojaAberta = true;
                    pistaAberta = true;
                    diaAtual = diasDaSemana[contadorDia];

                    for (i = 0; i < 4; i++)
                    {

                        horaDoDiaAtual = cicloRelogio[i];


                        //randomizar estado da pista
                        int randomizeEstados = Randomize.rdm.Next(1, 4);
                        Pista.condicoesPistaAtual = Pista.possiveisEstados[randomizeEstados];




                        //determinar se a pista esta aberta ou fechada dependendo do periodo do dia
                        if (i == 0 || i == 1)
                        {
                            pistaAberta = true;
                            lojaAberta = true;
                        }
                        if (i == 2)
                        {
                            pistaAberta = false;
                            lojaAberta = true;
                        }
                        if (i == 3)
                        {
                            pistaAberta = false;
                            lojaAberta = false;
                        }



                        if (BootJogo.menuAtual == BootJogo.menu.menuJogos)
                        {
                            switch (BootJogo.subMenuAtual)
                            {
                                case BootJogo.menu.subMenuCeleiro:
                                    Console.Clear();
                                    Graficos.SubMenuCeleiro();
                                    break;
                                case BootJogo.menu.subMenuCorridas:
                                    Console.Clear();
                                    Graficos.SubMenuCorrida(escolhaCorrida, numVitoria, cavalo);
                                    break;
                                case BootJogo.menu.subMenuLoja:
                                    break;
                            }
                        }


                        if (i != 3)
                        {
                            Thread.Sleep(myDelay);
                        }
                        if (i == 3)
                        {
                            Thread.Sleep(2000);
                            BootJogo.menuSleep();

                        }
                        while (BootJogo.menuAtual == BootJogo.menu.menuSleep) ;
                        
                    }
                    Thread.Sleep(2000);


                }

            }
            contadorDia = 0;
            diaAtual = diasDaSemana[0];
        }
    }
}






