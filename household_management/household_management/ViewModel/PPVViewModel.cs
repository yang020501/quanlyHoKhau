﻿using household_management.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace household_management.ViewModel
{
    class PPVViewModel : BaseViewModel
    {
        DataTable dt ;

        //private  string _Name;
        //public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        //private String _Id;
        //public String Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        DataView dvPopulations;
        public DataView DvPopulations { get => dvPopulations; set { dvPopulations = value; OnPropertyChanged(); } }


        private ObservableCollection<Population> PopulationsList;
        //public ObservableCollection<Population> PopulationsList { get => _PopulationsList; set { _PopulationsList = value; OnPropertyChanged(); } }

        public PPVViewModel()
        {
            NewTablePopulations();
        }

        //private DataRowView _Selected;
        //public DataRowView Selected
        //{
        //    get => _Selected;
        //    set
        //    {
        //        _Selected = value;
        //        OnPropertyChanged();
        //        if (Selected != null)
        //        {
        //            Name = (string)Selected.Row["Name"];
        //            Id = (string)Selected.Row["Id"];
        //            this.OnPropertyChanged();
        //            MessageBox.Show(Name);


        //        }
        //    }
        //}
        public void NewTablePopulations()
        {
            PopulationsList = new ObservableCollection<Population>(DataProvider.Ins.DB.Populations);
            dt = new DataTable();
            dt.Columns.Add("OrdinalNumber");
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Gender");
            dt.Columns.Add("DateOfBirth");
            dt.Columns.Add("PlaceOfBirth");             
            dt.Columns.Add("Id_Household");
            dt.Columns.Add("Address");
            dt.Columns.Add("Relegion");
            dt.Columns.Add("Career");
            dt.Columns.Add("Photo");
            //fill datatablejk
            for (int i = 0; i < PopulationsList.Count; i++)
            {
                dt.Rows.Add
                    (
                         //PopulationsList[i].Stt.ToString(),
                         //PopulationsList[i].Id.ToString(),
                         //PopulationsList[i].Name.ToString(),
                         //PopulationsList[i].Sex.ToString(),
                         //PopulationsList[i].DateOfBirth.ToString(),
                         //PopulationsList[i].PlaceOfBirth.ToString(),                         
                         //PopulationsList[i].Id_Household.ToString(),
                         //PopulationsList[i].Address.ToString(),
                         //PopulationsList[i].Relegion.ToString(),
                         //PopulationsList[i].Career.ToString()
                         CheckData(PopulationsList[i])
                    );
            }
            dvPopulations = new DataView(dt);
        }
        // Check if any fields is null
        private string[] CheckData(Population item)
        {
            string[] list = new string[11];
            list[0] = check(item.Stt);
            list[1] = check(item.Id);
            list[2] = check(item.Name);
            list[3] = check(item.Sex);
            list[4] = check(item.DateOfBirth);
            list[5] = check(item.PlaceOfBirth);
            list[6] = check(item.Id_Household);
            list[7] = check(item.Address);
            list[8] = check(item.Relegion);
            list[9] = check(item.Career);
            list[10] = check(item.Photo);
            return list;
        }
        // Convert null, string or any type to Valid view data
        private string check(object txt)
        {
            DateTime dateTime = new DateTime();
            bool gender = new bool();
            if (txt == null)                
                return "";
            else if (txt.GetType() == dateTime.GetType())
            {
                dateTime = (DateTime)txt;
                return dateTime.ToString("dd/MM/yyyy");
            }
            else if (txt.GetType() == gender.GetType())
            {
                gender = (bool)txt;
                if (gender == true)
                    return "Male";
                else return "Female";
            }
            return txt.ToString();
        }
        
        public void doSearch(DataGrid dtg ,string find,string form)
        {
            form += " Like '%{0}%'";
            if (dvPopulations.Count < 0) // if nothing return 
                return;
            DvPopulations.RowFilter = string.Format(form, find);
            dtg.ItemsSource = DvPopulations;
            OnPropertyChanged("DvPopulations");

        }
    }
}