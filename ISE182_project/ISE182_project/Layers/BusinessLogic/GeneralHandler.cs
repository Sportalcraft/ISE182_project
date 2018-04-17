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

        //Getter and setter to the data stored in the ram
        protected ICollection<T> RamData
        {
            set
            {
                if (_ramData == null) //there is ni stored messages
                {
                    string error = "ram data was not initialized!";
                    Logger.Log.Error(Logger.Maintenance(error));

                    throw new InvalidOperationException(error);
                }

                MergeTwoCollections.mergeIntoFirst(_ramData, value); // Merging to avoid duplication
                _ramData = DefaultSort(_ramData);

                if (!serialize(_ramData)) // Serialize the new list
                {
                    string error = "faild to serialize data";
                    Logger.Log.Fatal(Logger.Maintenance(error));

                    throw new IOException(error);
                }
            }

            get
            {
                if (_ramData == null)
                {
                    ICollection<T> deserialized = deserialize(); // Deserialize ram data to the ram if not there alrady

                    if (deserialized == null) //disk file was empty
                    {
                        _ramData = new List<T>(); // Initating
                    }
                    else
                    {
                        _ramData = deserialized; // Saving data fron disk
                    }
                }

                return _ramData;
            }
        }

        //Initiating the ram's saves from users stored in the disk
        public void start()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            SetRAM();
        }

        //desierilse data 
        protected abstract ICollection<T> deserialize();

        //serialize data
        protected abstract bool serialize(ICollection<T> _ramData);

        //Sort the data
        protected abstract ICollection<T> DefaultSort(ICollection<T> Data);


        //-----------------------------------------------------------

        #region protected methods

        //Update the stored in disk data after changing ram
        protected void UpdateDisk()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            RamData = new List<T>(); //So the set atribulte will activate to ask to save to disk
        }

        //Setting the ram, if null
        protected void SetRAM()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            ICollection<T> temp = RamData; //So the get atribulte will activate to ask to draw from disk
        }

        #endregion
    }
}
