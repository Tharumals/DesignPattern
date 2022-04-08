using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenClosePrinciple
{
    public enum Color
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Large, Huge
    }

    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product( string name, Color color, Size size)
        {
            if ( name == null)
            {
                throw new ArgumentNullException(paramName: nameof(name));
            }

            Name = name;
            Color = color;
            Size = size;
        }

    }

    public class ProductFilter
    {
        public  IEnumerable<Product> FilterBySize(IEnumerable<Product> product, Size size)
        {
            foreach(var p in product)
            {
                if(p.Size == size)
                {
                    yield return p;
                }
            }
        }

        public  IEnumerable<Product> FilterByColor(IEnumerable<Product> product, Color color)
        {
            foreach (var p in product)
            {
                if (p.Color == color)
                {
                    yield return p;
                }
            }
        }
    }

    public interface ISpecification<T>
    {
        bool IsSpecification(T t);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }
        public bool IsSpecification(Product t)
        {
            return t.Color == color;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }
        public bool IsSpecification(Product t)
        {
            return t.Size == size;
        }
    }

    // 
    public class AndSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first;
            this.second = second;
        }
        public bool IsSpecification(T t)
        {
            return first.IsSpecification(t) && second.IsSpecification(t);
        }
    }

    public class BetterFilter : IFilter<Product>
    {       
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach(var i in items)
            {
                if (spec.IsSpecification(i))
                {
                    yield return i;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree =new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            Product[] products = { apple, tree, house };

            var pf = new ProductFilter();
            Console.WriteLine("Old");
            foreach(var p in pf.FilterByColor(products, Color.Green))
            {
                Console.WriteLine($" - {p.Name} is Green");
            }

            var bf = new BetterFilter();

            Console.WriteLine("New");
            foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
            {
                Console.WriteLine($" - {p.Name} is Green");
            }

            foreach(var p in bf.Filter(products, new SizeSpecification(Size.Large)))
            {
                Console.WriteLine($" - {p.Name} is Large");
            }

            foreach(var p in bf.Filter(products,
                new AndSpecification<Product>(
                    new ColorSpecification(Color.Blue),
                    new SizeSpecification(Size.Large))))
            {
                Console.WriteLine($"- {p.Name} is big and blue");
            }
        }
    }
}
