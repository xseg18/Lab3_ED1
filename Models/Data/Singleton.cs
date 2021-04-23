using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3_ED1.Models
{
    public class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public ELineales.Lista<Assignment>[] hashTable;

        private Singleton()
        {
            hashTable = new ELineales.Lista<Assignment>[20];
        }

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
