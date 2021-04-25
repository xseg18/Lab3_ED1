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
        private readonly static Singleton _instance1 = new Singleton();
        public E_Arboles.PriorityQueue<int, string>[] devTable;
        private readonly static Singleton _instance2 = new Singleton();
        public ELineales.Lista<Assignment> proyectManager;

        private Singleton()
        {
            hashTable = new ELineales.Lista<Assignment>[20];
            devTable = new E_Arboles.PriorityQueue<int, string>[20];
            proyectManager = new ELineales.Lista<Assignment>();
        }

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }

        public static Singleton Instance1
        {
            get
            {
                return _instance1;
            }
        }

        public static Singleton Instance2
        {
            get
            {
                return _instance2;
            }
        }
    }
}
