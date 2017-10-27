using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace knowledgebase
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            ArrayList facts = new ArrayList();
            ArrayList rules = new ArrayList();
            ArrayList strings = new ArrayList();
            ArrayList newFacts = new ArrayList();
            int counter = 0;
            string line;
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify a filename as an argument " +
                    "for the program");
                return;
            }
            // Read the file and display it line by line.
            System.IO.StreamReader file =
                new System.IO.StreamReader(args[0]);
            while ((line = file.ReadLine()) != null)
            {
                strings.Add(line);
                counter++;
            }
            foreach (string l in strings)
            {
                parser.ParseLine(l, facts, rules);
            }
            Console.WriteLine("New Rule Facts\n-----------------");
            bool newFact = true;
            while (newFact)
            {
                newFact = false;
                foreach (Rule r in rules)
                {
                    if (r.DoesPass(facts))
                    {
                        newFact = true;
                        facts.Add(r.consequence);
                        Console.WriteLine(r.consequence);
                    }
                }
            }
            Console.WriteLine("Press 'ESC' to close the window.");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) return;
        }
    }
    class Rule
    {
        public ArrayList antecedents;
        public char consequence;
        public Rule()
        {
            this.antecedents = new ArrayList();
            this.consequence = '\0';
        }
        public Rule(ArrayList antecedents, char consequence)
        {
            this.antecedents = antecedents;
            this.consequence = consequence;
        }
        public bool DoesPass(ArrayList facts)
        {
            // check if we have the fact and return true if so
            if (facts.Contains(this.consequence)) return false;
            foreach (char c in this.antecedents)
            {
                if (!facts.Contains(c)) 
                    return false;
            }
            return true;
        }
        public override string ToString()
        {
            string antecedents = "";
            foreach (char s in this.antecedents)
            {
                antecedents += "(" + s.ToString() + ")";
            }
            return "Rule: [ Antecedents = " + antecedents + " Consequence = " + this.consequence + " ]";
        }
    }
    class Parser
    {
        public void ParseLine(string line, ArrayList facts, ArrayList rules)
        {
            if (!line.Contains(">"))
            {
                char i = GetFact(line, facts);
                if (i != '\0') facts.Add(i);
            } else {
                string[] split = line.Split(" > ");
                string[] ruleIndividuals = split[0].Split(", ");
                ArrayList antecedents = new ArrayList();
                foreach (string s in ruleIndividuals)
                {
                    antecedents.Add(GetFact(s));
                }
                rules.Add(new Rule(antecedents, GetFact(split[1])));
            }
            return;
        }
        public char GetFact(string line, ArrayList facts)
        {
            char f = line[0];
            if (facts.Contains(f)) return '\0';
            return f;
        }
        public char GetFact(string line)
        {
            return line[0];
        }
    }
}