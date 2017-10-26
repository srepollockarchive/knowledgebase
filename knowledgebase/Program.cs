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
            ArrayList individuals = new ArrayList();
            ArrayList rules = new ArrayList();
            ArrayList strings = new ArrayList();
            int counter = 0;
            string line;
            if (args.Length == 0) {
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
                parser.ParseLine(l, individuals, rules);
            }
            Console.WriteLine("Applying Rules...");
            bool newFact = true;
            while (newFact)
            {
                newFact = false;
                foreach (Individual i in individuals)
                {
                    foreach (Rule r in rules)
                    {
                        if (r.DoesPass(i))
                        {
                            newFact = true;
                            i.properties.Add(r.consequence);
                        }
                    }
                }
            }
            // TODO: Done facts
            Console.WriteLine("Facts\n----------");
            foreach (Individual i in individuals)
            {
                Console.WriteLine(i.ToString());
            }
            Console.WriteLine("Press 'ESC' to close the window.");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) return;
        }
    }

    class Individual
    {
        public string name;
        public ArrayList properties;
        public Individual()
        {
            this.name = "";
            this.properties = new ArrayList();
        }
        public Individual(string name, string property)
        {
            this.name = name;
            this.properties = new ArrayList();
            this.properties.Add(property);
        }
        public Individual(string name, ArrayList properties)
        {
            this.name = name;
            this.properties = properties;
        }
        public override string ToString()
        {
            string properties = "";
            foreach (String p in this.properties)
            {
                properties += p.ToString() + ", ";
            }
            return "Individual: " + this.name + " Properties = " + properties;
        }
    }
    class Rule
    {
        public ArrayList antecedents;
        public string consequence;
        public Rule()
        {
            this.antecedents = new ArrayList();
            this.consequence = "";
        }
        public Rule(ArrayList antecedents, string individual)
        {
            this.antecedents = antecedents;
            this.consequence = individual;
        }
        public bool DoesPass(Individual i)
        {
            if (i.properties.Contains(this.consequence)) return false;
            foreach (string c in i.properties)
                if (!i.properties.Contains(c)) return false;
            return true;
        }
        public override string ToString()
        {
            string antecedents = "";
            foreach (Individual s in this.antecedents)
            {
                antecedents += "(" + s.ToString() + ")";
            }
            return "Rule: [ Antecedents = " + antecedents + " Consequence = " + this.consequence + " ]";
        }
    }
    class Parser
    {
        public void ParseLine(string line, ArrayList individuals, ArrayList rules)
        {
            if (!line.Contains(">"))
            {
                Individual i = GetIndividual(line, individuals);
                if (i != null) individuals.Add(i);
            } else {
                string[] split = line.Split(">");
                string[] ruleIndividuals = split[0].Split(",");
                ArrayList antecedents = new ArrayList();
                foreach (string s in ruleIndividuals)
                {
                    antecedents.Add(GetProperty(s));
                }
                rules.Add(new Rule(antecedents, GetProperty(split[1])));
            }
            return;
        }
        public Individual GetIndividual(string line, ArrayList individuals)
        {
            string[] split = Regex.Split(line, @"[(|)]");
            foreach (Individual i in individuals)
            {
                if (i.name == split[1])
                {
                    i.properties.Add(split[0]);
                    return null;
                }
            }
            ArrayList properties = new ArrayList();
            properties.Add(split[0]);
            return new Individual(split[1], properties);
        }
        public string GetProperty(string line)
        {
            string[] split = Regex.Split(line, @"[(|)]");
            return split[0];
        }
    }
}