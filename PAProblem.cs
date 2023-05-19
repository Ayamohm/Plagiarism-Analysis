using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using GraphGenerator;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "PlagiarismAnalysis"; } }

        public override void TryMyCode()
        {            

            //Case1
            string []vertices1 = { "1", "4", "5"};
            Tuple<string, string, float>[] edges1 = new Tuple<string,string,float>[2];
            edges1[0] = new Tuple<string, string, float>("1", "4", 10);
            edges1[1] = new Tuple<string, string, float>("4", "5", 15);

            float expected1 = 12.5f;
            float output1 = PlagiarismAnalysis.AnalyzeMatchingScore(vertices1, edges1, "4");
            PrintCase(vertices1, edges1, output1, expected1);

            //Case2
            string[] vertices2 = { "1", "2", "3", "4", "5" , "6", "7", "8"};
            Tuple<string, string, float>[] edges2 = new Tuple<string, string, float>[4];
            edges2[0] = new Tuple<string, string, float>("1", "2", 10);
            edges2[1] = new Tuple<string, string, float>("3", "4", 20);
            edges2[2] = new Tuple<string, string, float>("5", "6", 30);
            edges2[3] = new Tuple<string, string, float>("7", "8", 40);

            float expected2 = 30;
            float output2 = PlagiarismAnalysis.AnalyzeMatchingScore(vertices2, edges2, "6");
            PrintCase(vertices2, edges2, output2, expected2);

            //Case3
            string[] vertices3 = { "A1", "A2", "A3", "A4", "A5", "A6" };
            Tuple<string, string, float>[] edges3 = new Tuple<string, string, float>[6];
            edges3[0] = new Tuple<string, string, float>("A1", "A2", 1);
            edges3[1] = new Tuple<string, string, float>("A2", "A3",2);
            edges3[2] = new Tuple<string, string, float>("A5", "A4",3);
            edges3[3] = new Tuple<string, string, float>("A5", "A6",4);
            edges3[4] = new Tuple<string, string, float>("A3", "A5",5);
            edges3[5] = new Tuple<string, string, float>("A4", "A2",6);
            float expected3 = 3.5f;
            float output3 = PlagiarismAnalysis.AnalyzeMatchingScore(vertices3, edges3, "A6");
            PrintCase(vertices3, edges3, output3, expected3);

            //Case4
            string[] vertices4 = { "1", "2", "3", "4", "5", "6", "7", "8" };
            Tuple<string, string, float>[] edges4 = new Tuple<string, string, float>[11];
            edges4[0] = new Tuple<string, string, float>("1", "5",10);
            edges4[1] = new Tuple<string, string, float>("1", "4",10);
            edges4[2] = new Tuple<string, string, float>("1", "3",10);
            edges4[3] = new Tuple<string, string, float>("1", "2", 10);
            edges4[4] = new Tuple<string, string, float>("2", "3", 15);
            edges4[5] = new Tuple<string, string, float>("3", "4", 15);
            edges4[6] = new Tuple<string, string, float>("4", "5", 15);
            edges4[7] = new Tuple<string, string, float>("5", "2", 15);
            edges4[8] = new Tuple<string, string, float>("6", "7", 30);
            edges4[9] = new Tuple<string, string, float>("6", "8", 40);
            edges4[10] = new Tuple<string, string, float>("8", "7", 30);

            float expected41 = 33.3f;
            float output41 = PlagiarismAnalysis.AnalyzeMatchingScore(vertices4, edges4, "6");
            PrintCase(vertices4, edges4, output41, expected41);

            float expected42 = 12.5f;
            float output42 = PlagiarismAnalysis.AnalyzeMatchingScore(vertices4, edges4, "1");
            PrintCase(vertices4, edges4, output42, expected42);
        }

        

        Thread tstCaseThr;
        bool caseTimedOut ;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            int testCases;
            float actualResult = float.MinValue;
            float output = float.MinValue;

            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();
            testCases = int.Parse(line);
   
            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            int i = 1;
            while (testCases-- > 0)
            {
                string startVertex = sr.ReadLine();
                int v = int.Parse(sr.ReadLine());
                int e = int.Parse(sr.ReadLine());
                var vertices = new string[v];
                for (int j = 0; j < v; j++)
                {
                    vertices[j] = sr.ReadLine();
                }
                var edges = new Tuple<string, string, float>[e];
                for (int j = 0; j < e; j++)
                {
                    line = sr.ReadLine();
                    string[] lineParts = line.Split(',');
                    edges[j] = new Tuple<string, string, float>(lineParts[0], lineParts[1], float.Parse(lineParts[2]));
                }
                line = sr.ReadLine();
                actualResult = float.Parse(line);
                caseTimedOut = true;
                caseException = false;
                {
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            Stopwatch sw = Stopwatch.StartNew();
                            output = PlagiarismAnalysis.AnalyzeMatchingScore(vertices, edges, startVertex);
                            sw.Stop();
                            //PrintCase(vertices,edges, output, actualResult);
                            Console.WriteLine("|V| = {0}, |E| = {1}, time in ms = {2}", vertices.Length, edges.Length, sw.ElapsedMilliseconds);
                            Console.WriteLine("{0}", Math.Round(output,1));
                        }
                        catch
                        {
                            caseException = true;
                            output = float.MinValue;
                        }
                        caseTimedOut = false;
                    });

                    //StartTimer(timeOutInMillisec);
                    if (readTimeFromFile)
                    {
                        timeOutInMillisec = int.Parse(sr.ReadLine().Split(':')[1]);
                    }
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }

                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
					tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                else if (Math.Round(output, 1) == Math.Round(actualResult, 1))    //Passed
                {
                    Console.WriteLine("Test Case {0} Passed!", i);
                    correctCases++;
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    Console.WriteLine(" your answer = {0}, correct answer = {1}", Math.Round(output,1), Math.Round(actualResult,1));
                    wrongCases++;
                }

                i++;
            }
            file.Close();
            sr.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0)); 
        }

        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();

        }

        #endregion

        #region Helper Methods
        private static void PrintCase(string[] vertices, Tuple<string, string, float>[] edges, float output, float expected)
        {
            Console.WriteLine("Vertices: ");
            for (int i = 0; i < vertices.Length; i++)
            {
                Console.Write(vertices[i] + ", ");
            }
            Console.WriteLine("\nEdges: ");
            for (int i = 0; i < edges.Length; i++)
            {
                Console.WriteLine("{0}, {1} score = {2}", edges[i].Item1, edges[i].Item2, edges[i].Item3);
            }
            Console.WriteLine("Output: {0}", Math.Round(output,1));
            Console.WriteLine("Expected: {0}", Math.Round(expected,1));
            if (Math.Round(output,1) == Math.Round(expected,1))    //Passed
            {
                Console.WriteLine("CORRECT");
            }
            else                    //WrongAnswer
            {
                Console.WriteLine("WRONG");
            }
            Console.WriteLine();
        }
        
        #endregion
   
    }
}
