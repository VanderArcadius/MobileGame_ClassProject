﻿using System;

using TRP.Models;
using TRP.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TRP.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MonsterEditPage : ContentPage
	{
	    private MonsterDetailViewModel _viewModel; // view model for this page

        public Monster Data { get; set; } // data for this page

        // Constructor: create instance of this page, and set the view model
        public MonsterEditPage(MonsterDetailViewModel viewModel)
        {
            // Save off the item
            Data = viewModel.Data;
            viewModel.Title = "Edit " + viewModel.Title;

            InitializeComponent();
            
            // Set the data binding for the page
            BindingContext = _viewModel = viewModel;

            // Set the picker to the preexisting monster type
            MonsterTypePicker.SelectedItem = Data.MonsterType.ToString();
        }

        // Returns the sum of all attributes
        public int AttributeSum()
        {
            return Data.Attribute.Attack + Data.Attribute.Defense + Data.Attribute.Speed;
        }

        // Returns the number of stat points available
        public int AdjustStatPointAvail(int statTotalPoints)
        {
            return statTotalPoints - (AttributeSum());
        }

        // The stepper function for Attack
        void Attack_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (AttributeSum() > GameGlobals.availStatPoints)
            {
                AttackValue.Text = String.Format("{0}", e.OldValue);
                attack.Value = e.OldValue;
            }
            else
            {
                AttackValue.Text = String.Format("{0}", e.NewValue);
                statPoints.Text = String.Format("{0}", AdjustStatPointAvail(GameGlobals.availStatPoints));
            }
        }

        // The stepper function for Defense
        void Defense_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (AttributeSum() > GameGlobals.availStatPoints)
            {
                DefenseValue.Text = String.Format("{0}", e.OldValue);
                defense.Value = e.OldValue;
            }
            else
            {
                DefenseValue.Text = String.Format("{0}", e.NewValue);
                statPoints.Text = String.Format("{0}", AdjustStatPointAvail(GameGlobals.availStatPoints));
            }
        }

        // The stepper function for Speed
        void Speed_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (AttributeSum() > GameGlobals.availStatPoints)
            {
                SpeedValue.Text = String.Format("{0}", e.OldValue);
                speed.Value = e.OldValue;
            }
            else
            {
                SpeedValue.Text = String.Format("{0}", e.NewValue);
                statPoints.Text = String.Format("{0}", AdjustStatPointAvail(GameGlobals.availStatPoints));
            }
        }

        // When save button is clicked, add attributes to this character, and broadcast edit
        private async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "EditData", Data);

            // removing the old ItemDetails page, 2 up counting this page
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            // Add a new items details page, with the new Item data on it
            await Navigation.PushAsync(new MonsterDetailPage(new MonsterDetailViewModel(Data)));

            // Last, remove this page
            Navigation.RemovePage(this);
        }

        // When cancel button is clicked, remove this page from stack
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Provides Monster information for picker
        private void MonsterTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var monsterType = MonsterTypePicker.SelectedItem.ToString();
            var monsterEnum = MonsterTypeList.ConvertStringToEnum(monsterType);
            pic.Source = Data.GetMonsterImage(monsterEnum);
        }
    }
}