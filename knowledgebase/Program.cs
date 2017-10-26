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
            Console.WriteLine("Individuals\n----------");
            foreach (Individual i in individuals)
            {
                Console.WriteLine(i.ToString());
            }
            Console.WriteLine("Rules\n-----");
            foreach (Rule r in rules)
            {
                Console.WriteLine(r.ToString());
                if (r.CheckIfSatisfied(individuals))
                {
                    Console.WriteLine(r.consequence.ToString());
                }
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
        public Individual(string name, Property property)
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
            foreach (Property p in this.properties)
            {
                properties += p.ToString() + ", ";
            }
            return "Individual: name = " + this.name + " Properties = " + properties;
        }
    }
    class Property
    {
        public string type;
        public Property()
        {
            this.type = "";
        }
        public Property(string type)
        {
            this.type = type;
        }
        public override string ToString()
        {
            return this.type;
        }
    }
    class Rule
    {
        public ArrayList antecedents;
        public Individual consequence;
        public Rule()
        {
            this.antecedents = new ArrayList();
            this.consequence = new Individual();
        }
        public Rule(ArrayList antecedents, Individual individual)
        {
            this.antecedents = antecedents;
            this.consequence = individual;
        }
        public Rule(string[] antecedents, Individual individual)
        {

        }
        // TODO: Handle generics
        public bool CheckIfSatisfied(ArrayList individuals)
        {
            Regex rgx = new Regex("[A-Z]");
            bool flag = false;
            foreach (Individual i in this.antecedents) // TODO: Possible Generic
            {
                flag = false;
                foreach (Individual ii in individuals)
                {
                    // if (i.name == ii.name) flag = true;
                    if (rgx.IsMatch(i.name) && !CheckProperties(i, ii)) // Generic
                    {
                        return false;
                    }
                }
                if (!flag) return false;
            }
            return true;
        }
        private bool CheckProperties(Individual i, Individual ii)
        {
            foreach(Property p in i.properties)
            {
                foreach (Property pp in ii.properties)
                {
                    if (p.type == pp.type) return true;
                }
            }
            return false;
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
            if (!line.Contains(">")) // Def not a rule
            {
                Individual i = GetIndividual(line, individuals);
                if (i != null) individuals.Add(i);
            } else { // we have a rule
                // TODO: Check if generic and apply
                string[] split = line.Split(">");
                string[] ruleIndividuals = split[0].Split(",");
                ArrayList antecedents = new ArrayList();
                foreach (string s in ruleIndividuals)
                {
                    antecedents.Add(GetIndividual(s));
                }
                rules.Add(new Rule(antecedents, GetIndividual(split[1])));
            }
            return;
        }
        public Individual GetIndividual(string line)
        {
            string[] split = Regex.Split(line, @"[(|)]");
            ArrayList properties = new ArrayList();
            properties.Add(new Property(split[0]));
            return new Individual(split[1], properties);
        }
        // TODO: Check if generic
        public Individual GetIndividual(string line, ArrayList individuals)
        {
            string[] split = Regex.Split(line, @"[(|)]");
            foreach (Individual i in individuals)
            {
                if (i.name == split[1])
                {
                    i.properties.Add(new Property(split[0]));
                    return null;
                }
            }
            ArrayList properties = new ArrayList();
            properties.Add(new Property(split[0]));
            return new Individual(split[1], properties);
        }
    }
}