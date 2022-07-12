using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Process_Generator
{
    class ProcessGenerator
    {
        private int TS; //timpul simularii
        private int q; //durata cuantei
        private int nrPrc = 0; //numarul de procese
        private int firstProcessCreationMoment;
        private int processorUsageTime = 0;


        Random rnd = new Random();
        List<Process> ListOfProcesses = new List<Process>();
        Queue<Process> ReadyProcesses = new Queue<Process>();
        Queue<Process> RunningProcesses = new Queue<Process>();

        //Se citesc datele din fisierul text "input.txt"
        private void generateValues()
        {
            string[] lines = File.ReadAllLines("Files/input.txt");
            Constants.T = Convert.ToInt32(lines[0]);
            Constants.Q = Convert.ToInt32(lines[1]);
            Constants.R_Min = Convert.ToInt32(lines[2]);
            Constants.R_Max = Convert.ToInt32(lines[3]);
            Constants.D_Min = Convert.ToInt32(lines[4]);
            Constants.D_Max = Convert.ToInt32(lines[5]);

            TS = Constants.T;
            q = Constants.Q;

        }

        //Se creaza un proces in cuanta in care se afla contorul 'i';
        private void createProcess(int i, int a, int b)
        {
            int R;
            int Life = rnd.Next(Constants.D_Min, Constants.D_Max);
            int T;

            if (nrPrc == 0)
            {
                T = a + b;  // 0 + firstProcessCreationMoment
                R = rnd.Next(Constants.R_Min, Constants.R_Max);
                
            }
            else
            {
                T = a + b; //ListOfProcesses[nrP - 1].t + ListOfProcesses[nrP - 1].r;
                R = rnd.Next(Constants.R_Min, Constants.R_Max);
            }

            Process p = new Process(nrPrc, R, T, Life);
            p.delta = p.life;
            ListOfProcesses.Add(p);
            ReadyProcesses.Enqueue(p);
            int d = i / q;
            Console.WriteLine("\n\tPROCESS NUMBER " + ListOfProcesses[nrPrc].num + " CREATED AT QUANTUM " + d);
            nrPrc++;

        }

        //Se scade valoarea din 'qnt' din viata procesului; qnt are ori valoarea cuantei, 
        //ori valoarea 1; qnt are valoarea 1 cand procesul are viata mai mica decat cuanta, si se scade numai cu 1
        //pentru a putea adauga alt proces in coada ready dupa ce se termina procesul
        private void useProcess(Process processRunning, int i, int qnt)
        {
            processRunning.life -= qnt;
            processRunning.TotalProcessingTime += qnt;
            foreach(Process p in ListOfProcesses)
            {
                if(p.num != processRunning.num && p.terminated == false)
                {
                    p.TotalWaitingTime += qnt;
                }
            }

            if (processRunning.life <= 0)
            {
                //Daca procesul se termina, se afiseaza ca s-a terminat, iar apoi se scoade din cozi
                double d = i / q;
                Console.WriteLine("\n\n--------PROCESS NUMBER " + processRunning.num + " TERMINATED AT QUANTUM " + Math.Floor(d) + "\n");
                processRunning.terminated = true;
                ReadyProcesses.Dequeue();
                RunningProcesses.Dequeue();

            }

        }

        //Se verifica daca se poate crea un proces; daca nu exista inca procese create, se verifica daca contorul 'i'este egal cu
        //pozitia 'firstProcessCreationMoment', valoare generata aleatoriu pentru alege momentul crearii primului proces
        private void checkForProcess(int i)
        {
            if (nrPrc == 0)
            {
                if (i == firstProcessCreationMoment)
                {
                    createProcess(i, 0, firstProcessCreationMoment);
                }
            }

            else
                if (i == (ListOfProcesses[nrPrc - 1].t + ListOfProcesses[nrPrc - 1].r))
            {
                createProcess(i, ListOfProcesses[nrPrc - 1].t, ListOfProcesses[nrPrc - 1].r);
            }



        }

        //Se verifica daca procesul se termina in cuanta curenta; daca nu, atunci se scade valoarea cuantei din viata acestuia
        //'ok' reprezinta o variabila care arata daca procesul se termina sau nu in cuanta curenta
        private void checkQuantum(Process processRunning, int i, ref int ok)
        {
            if (processRunning.life >= q)
            {
                useProcess(processRunning, i, q);
                ok = 1;
            }
            else
                ok = 0;

        }

        //Se extrag din coada de procese ready procesele care nu s-au terminat inainte de finalul simularii si se afiseaza
        private void processesLeft()
        {
            Console.WriteLine("\n");
            if (ReadyProcesses.Count != 0)
            {
                Console.WriteLine("PROCESSES UNTERMINATED: ");
                foreach (Process p in ListOfProcesses)
                {
                    if(p.terminated == false)
                    {
                        Console.WriteLine("PROCESS NUMBER " + p.num + ":  LIFE LEFT: " + p.life);
                        Constants.procesessLeft++;
                    }
                }

            }
            
        }
        private void displayProcesess()
        {
            //Se afiseaza in consola informatiile fiecarui proces
            for (int i = 0; i < nrPrc; i++)
            {
                Console.WriteLine("\nProcess number: " + i + "\n");
                Console.WriteLine("The moment the process has been created: " + ListOfProcesses[i].t + "\n");
                Console.WriteLine("Time untill the next process: " + ListOfProcesses[i].r + "\n");
                Console.WriteLine("It has a lifespan of : " + ListOfProcesses[i].delta + "\n");
                Console.WriteLine("Total processing time : " + ListOfProcesses[i].TotalProcessingTime + "\n");
                Console.WriteLine("Total waiting time : " + ListOfProcesses[i].TotalWaitingTime + "\n");
                Console.WriteLine("The moment the process gets the Processor : " + ListOfProcesses[i].processGetsProcesor + "\n");
                Console.WriteLine("Response time : " + ListOfProcesses[i].responseTime + "\n");
                Console.WriteLine("Terminated : " + ListOfProcesses[i].terminated + "\n");
                Console.WriteLine("\n\n");
            }


            //Se afiseaza in consola rezultatele finale ale simularii
            Console.WriteLine("THE SIMULATION LASTS " + TS + " UNITS OF TIME\n");
            Console.WriteLine("DURATION OF A QUANTUM: " + q + " UNITS OF TIME\n");
            Console.WriteLine("TOTAL NUMBER OF PROCESESS: " + nrPrc + "\n");
            if(Constants.procesessLeft > 0)
            {
                Console.WriteLine("UNTERMINATED PROCESESS: " + Constants.procesessLeft + "\n");
            }
            else
            {
                Console.WriteLine("ALL PROCESSES FINISHED\n");
            }
            
            Console.WriteLine("THE SIMULATION HAS " + TS / q + " QUANTA OF TIME\n");
            
            float averageProcessingTime = 0;
            float averageWaitingTime = 0;
            float averageResponseTime = 0;

            foreach(Process p in ListOfProcesses)
            {
                averageProcessingTime += p.TotalProcessingTime;
                averageWaitingTime += p.TotalWaitingTime;
                averageResponseTime += p.responseTime;

            }

            averageProcessingTime = averageProcessingTime / nrPrc;
            averageWaitingTime = averageWaitingTime / nrPrc;
            averageResponseTime = averageResponseTime / nrPrc;
            int percentageProcessorUsageTime = (processorUsageTime * 100) / TS;

            Console.WriteLine("TOTAL PROCESSOR USEAGE: " + percentageProcessorUsageTime + "%\n");
            Console.WriteLine("AVERAGE PROCESSING TIME: " + averageProcessingTime + "\n");
            Console.WriteLine("AVERAGE WAITING TIME: " + averageWaitingTime + "\n");
            Console.WriteLine("AVERAGE RESPONSE TIME: " + averageResponseTime + "\n");

            //Se afiseaza in fisierul text rezultatele finale ale simularii
            using (StreamWriter sw = new StreamWriter("Files/output.txt"))
            {
                sw.WriteLine("THE SIMULATION LASTS " + TS + " UNITS OF TIME\n");
                sw.WriteLine("DURATION OF A QUANTUM: " + q + " UNITS OF TIME\n");
                sw.WriteLine("TOTAL NUMBER OF PROCESESS: " + nrPrc + "\n");
                if (Constants.procesessLeft > 0)
                {
                    sw.WriteLine("UNTERMINATED PROCESESS: " + Constants.procesessLeft + "\n");
                }
                else
                {
                    sw.WriteLine("ALL PROCESSES FINISHED\n");
                }
                sw.WriteLine("THE SIMULATION HAS " + TS / q + " QUANTA OF TIME\n");
                sw.WriteLine("TOTAL PROCESSOR USEAGE: " + percentageProcessorUsageTime + "%\n");
                sw.WriteLine("AVERAGE PROCESSING TIME: " + averageProcessingTime + "\n");
                sw.WriteLine("AVERAGE WAITING TIME: " + averageWaitingTime + "\n");
                sw.WriteLine("AVERAGE RESPONSE TIME: " + averageResponseTime + "\n");
            }
        }

        private void generateProcesses()
        {
            generateValues();
            int i = 0;
            int ok = 0;
            firstProcessCreationMoment = rnd.Next(0, Constants.R_Max);  //Se genereaza momentul cand se genereaza primul proces

            //Se parcurge simularea iteratie cu iteratie
            for (i = 0; i < TS; i++)
            {
                checkForProcess(i); //Se verifica daca se poate crea un proces la iteratia respectiva
                if (ReadyProcesses.Count != 0)
                {
                    processorUsageTime += 1;

                    //Se verifica daca incepe o cuanta
                    if (i % q == 0)
                    {
                        //Daca mai exista un proces in coada running, acesta se scoate pentru a putea introduce un proces nou
                        if (RunningProcesses.Count != 0)
                        {
                            RunningProcesses.Dequeue();
                        }

                        //Se extrage un proces din coada de procese ready
                        RunningProcesses.Enqueue(ReadyProcesses.Peek());
                        Process processRunning = RunningProcesses.Peek();

                        //Se calculeaza momentul cand procesul primeste procesorul pentru prima data ('-1' reprezinta faptul ca procesul 
                        //nu a primit procesorul deloc pana acum
                        if (processRunning.processGetsProcesor == -1)
                        {
                            processRunning.processGetsProcesor = i;
                            processRunning.responseTime = processRunning.processGetsProcesor - processRunning.t;
                        }

                        //Se verifica daca procesul actual se termina in cuanta actuala
                        checkQuantum(processRunning, i, ref ok);
                            
                        //Daca se mai afla si alta procese in asteptare, procesul actual se adauga in spatele cozii de astaptare
                        if(ReadyProcesses.Count != 0)
                        {
                            ReadyProcesses.Dequeue();
                            ReadyProcesses.Enqueue(processRunning);
                        }


                    }
                    //Daca nu se incepe o cuanta noua (daca iteratia se afla deja in interiorul unei cuante)
                    else
                    {
                        // 'ok = 0' semnifica faptul ca procesul se termina in cuanta respectiva 
                        if (ok == 0)
                        {
                            //Daca mai exista un proces in coada running, acesta se scoate pentru a putea introduce un proces nou
                            if (RunningProcesses.Count != 0)
                            {
                                RunningProcesses.Dequeue();
                            }

                            //Se adauga in coada de procese running primul proces din coada de procese ready (acest proces nu se elimina din coada 
                            //de procese ready, deoarece la urmatoarea iteratie tot acest proces se va adauga in coada de procese running; procesul
                            //se elimina, sau se adauga in spatele cozii de asteptare numai daca procesul se termina, sau daca se termina cuanta
                            RunningProcesses.Enqueue(ReadyProcesses.Peek());
                            Process processRunning = RunningProcesses.Peek();

                            //Se calculeaza momentul cand procesul primeste procesorul pentru prima data ('-1' reprezinta faptul ca procesul 
                            //nu a primit procesorul deloc pana acum
                            if (processRunning.processGetsProcesor == -1)
                            {
                                processRunning.processGetsProcesor = i;
                                processRunning.responseTime = processRunning.processGetsProcesor - processRunning.t;
                            }

                            //Se scade valoarea 1 din viata procesului
                            useProcess(processRunning, i, 1);

                            if (processRunning.life <= 0 && ReadyProcesses.Count != 0)
                            {
                                //Daca procesul se termina in cuanta, se exstrage alt proces din coada de asteptare si se foloseste
                                //pana la finalul cuantei, sau pana cand se termina procesul
                                RunningProcesses.Enqueue(ReadyProcesses.Peek());
                                Process nextProcessRunning = RunningProcesses.Peek();
                                if (nextProcessRunning.processGetsProcesor == 0)
                                {
                                    nextProcessRunning.processGetsProcesor = i;
                                    nextProcessRunning.responseTime = nextProcessRunning.processGetsProcesor - nextProcessRunning.t;
                                }

                                

                            }


                        }
                    }

                }

            }
        }


        public void start()
        {
            generateProcesses();
            processesLeft();
            displayProcesess();
        }

    }
}
