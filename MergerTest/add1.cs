using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergerTest
{

    public class MyNesne
    {
        public string Name { get; set; }
        public string SurName { get; set; }
    }

    public class MyListe:IDictionary<string,string>
    {
        readonly List<MyNesne> liste = new List<MyNesne>();

        public MyListe()
        {

        }

        public MyListe(IDictionary<string, string> dd)
        {
            Add(dd);
        }

        public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ICollection<string> Keys => throw new NotImplementedException();

        public ICollection<string> Values => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(string name, string surname)
        {
            liste.Add(new MyNesne { Name = name, SurName = surname });
        }

        public void Add(IDictionary<string, string> dd)
        {

            foreach (var item in dd)
            {
                liste.Add(new MyNesne { Name = item.Key, SurName = item.Value });
            }

        }

        public void Add(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out string value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }


    public class TestMyList
    {

        public TestMyList()
        {

            SortedList my_slist = new SortedList() {
                             { "b.09", 234 },
                             { "b.11", 395 },
                             { "b.01", 405 },
                             { "b.67", 100 },
                             { "b.55", 500 }};


            MyListe ls1 = new MyListe()
            { { "Salim","Demiray" },
                { "Ege","Demiray"} };

            //MyListe ls = new MyListe();

            //ls.Add({ { "Salim","Demiray" } });
            //ls.Add({ "Salim","Demiray"});


            //SortedList SS = new SortedList();
            //SS.Add(


        }
    }


}
