﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TRP.Models;
using TRP.Views;
using System.Linq;

namespace TRP.ViewModels
{
    public class CharactersViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static CharactersViewModel _instance;

        // Constructor: returns instance if instantiated, otherwise creates instance if it's null 
        public static CharactersViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CharactersViewModel();
                }
                return _instance;
            }
        }

        // Collection of Characters 
        public ObservableCollection<Character> Dataset { get; set; }

        // Command to load data
        public Command LoadDataCommand { get; set; }

        private bool _needsRefresh; // boolean for whether data is stale or not

        // Constructor: loads data and listens for broadcast from views
        public CharactersViewModel()
        {

            Title = "Character List";
            Dataset = new ObservableCollection<Character>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            // Update Database: Delete Character
            MessagingCenter.Subscribe<CharacterDeletePage, Character>(this, "DeleteData", async (obj, data) =>
            {
                await DeleteAsync(data);
            });

            // For adding Character
            MessagingCenter.Subscribe<CharacterNewPage, Character>(this, "AddData", async (obj, data) =>
            {
                await AddAsync(data);
            });

            // For modifying a Character
            MessagingCenter.Subscribe<CharacterEditPage, Character>(this, "EditData", async (obj, data) =>
            {
                await UpdateAsync(data);
                
            });

        }

        // Return whether a refresh is needed
        // It sets the refresh flag to false
        public bool NeedsRefresh()
        {
            if (_needsRefresh)
            {
                _needsRefresh = false;
                return true;
            }

            return false;
        }

        // Sets the need to refresh
        public void SetNeedsRefresh(bool value)
        {
            _needsRefresh = value;
        }


        public async void ReloadData()
        {
            await ExecuteLoadDataCommand();
        }

        // Command to load data into collection
        private async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Dataset.Clear();
                var dataset = await DataStore.GetAllAsync_Character(true);

                // Example of how to sort the database output using a linq query.
                //Sort the list
                dataset = dataset
                    .OrderBy(a => a.Name)
                    .ThenBy(a => a.Description)
                    .ToList();

                // Then load the data structure
                foreach (var data in dataset)
                {
                    Dataset.Add(data);
                }
                SetNeedsRefresh(false);
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            finally
            {
                IsBusy = false;
            }
        }

        // Refreshes data
        public void ForceDataRefresh()
        {
            var canExecute = LoadDataCommand.CanExecute(null);
            LoadDataCommand.Execute(null);
        }

        #region DataOperations
        // Add character to datastore
        public async Task<bool> AddAsync(Character data)
        {
            Dataset.Add(data);
            var ret = await DataStore.AddAsync_Character(data);
            return ret;
        }

        // Delete character in datastore
        public async Task<bool> DeleteAsync(Character data)
        {
            Dataset.Remove(data);
            var ret = await DataStore.DeleteAsync_Character(data);
            return ret;
        }

        // Update character in the datastore
        public async Task<bool> UpdateAsync(Character data)
        {
            var ret = await DataStore.UpdateAsync_Character(data);
            return ret;
        }

        // Call to database to ensure most recent
        public async Task<Character> GetAsync(string id)
        {
            var ret = await DataStore.GetAsync_Character(id);
            return ret;
        }

        #endregion DataOperations


        #region ItemConversion

        // Takes an item string ID and looks it up and returns the item
        // This is because the Items on a character are stores as strings of the GUID.  That way it can be saved to the DB.
        public Character GetCharacter(string charID)
        {
            if (string.IsNullOrEmpty(charID))
            {
                return null;
            }

            Character myData = DataStore.GetAsync_Character(charID).GetAwaiter().GetResult();
            if (myData == null)
            {
                return null;
            }

            return myData;
        }

        #endregion ItemConversion

        // Get random character
        public Character ChooseRandomCharacter()
        {
            if (Dataset.Count < 1)
            {
                return null;
            }

            // Get all the items for that location
            var randChar = Dataset.Where(c => c.PenguinType == PenguinTypeEnum.Adelie).FirstOrDefault();

            // If an attribute is selected...
            if (randChar != null)
            {
                return randChar;
            }

            return null;
        }
    }
}