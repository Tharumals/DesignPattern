using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInversion
{

    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public class Person
    {
        public string Name;
    }

    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildren(string Name);
    }
    public class Relationships: IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> relation
            = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relation.Add((parent, Relationship.Parent, child));
            relation.Add((child, Relationship.Child, parent));
        }

        //public List<(Person, Relationship, Person)> Relation => relation;
        public IEnumerable<Person> FindAllChildren(string Name)
        {
            return relation.Where(x => x.Item1.Name == Name && x.Item2 == Relationship.Parent).Select(r => r.Item3);
        }
    }
    public class Research
    {
        public Research(IRelationshipBrowser browser)
        {
            foreach(var r in browser.FindAllChildren("John"))
            {
                Console.WriteLine($"John has a child call {r.Name}");
            }
        }
        static void Main(string[] args)
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Matt" };

            // low-level module
            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Research(relationships);
        }
    }
}
