using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{

    //This class generally handle data on the ram
    abstract class GeneralHandler<T>
    {
        private ICollection<T> _ramData; // Store a coppy of the data in the ram for quick acces

        public GeneralHandler()
        {
            _ramData = new List<T>(); // Initating           
        }

        //Getter and setter to the data stored in the ram
        public ICollection<T> RamData
        {
            get { return _ramData; }
        }

        //Initiating the ram's saves from users stored in the disk
        public virtual void start()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            reciveData();
        }

        //Add an item to te ram ans update the DS
        public virtual void add(T item)
        {
            if (!RamData.Contains(item))
            {
                RamData.Add(item);
                AddToDS(item);
            }
        }

        //desierilse data 
        protected abstract void reciveData();

        //serialize data
        protected abstract bool AddToDS(T item);

        //Sort the data
        protected abstract ICollection<T> DefaultSort(ICollection<T> Data);

    }
}
