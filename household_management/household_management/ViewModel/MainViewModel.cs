﻿using household_management.Model;
using household_management.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace household_management.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }
        private string _Id;
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }
        private string _Role;
        public string Role { get => _Role; set { _Role = value; OnPropertyChanged(); } }

        private ImageSource _SPhoto;
        public ImageSource SPhoto { get => _SPhoto; set { _SPhoto = value; OnPropertyChanged(); } }

        private string _Photo;
        public string Photo { get => _Photo; set { _Photo = value; OnPropertyChanged(); } }

        public ICommand LoadWindowCommand{ get; set; }
        public ICommand LoadPopuationWindowCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand LoadManageButtonCommand { get; set; }
        public ICommand LoadAccount { get; set; }
        public bool isLoad = false;

        private Frame main = new Frame();
        public Frame Main { get => main; set { main = value; OnPropertyChanged(); } }

        private bool _addSelected;
        public bool AddSelected { get => _addSelected;set {  _addSelected = value;OnPropertyChanged();  openAddPage(); ; } }
        
        private bool _searchSelected;
        public bool SearchSelected { get => _searchSelected; set { _searchSelected = value; OnPropertyChanged(); openSearchPage(); } }

        private bool _manageSelected;
        public bool ManageSelected { get => _manageSelected;set { _manageSelected = value; OnPropertyChanged(); openManage(); } }

        public static MainViewModel data { get; set; }

        AddPage aView = new AddPage();

        private void openManage()
        {
            if(ManageSelected == true)
            {
                main.Refresh();
                main.Content = null;
                AccountPage page = new AccountPage();
                AccountManagerViewModel.Vm = data;              
                main.Content = page;

            }
        }
        
        private void openAddPage()
        {            
            if(AddSelected == true)
            {
                main.Refresh();
                main.Content = null;
                AddPageViewModel vm = new AddPageViewModel();
                AddPage addPage = new AddPage();
                addPage.DataContext = null;
                addPage.DataContext = vm;
                main.Content = addPage;
            }
          
        }
        private void openSearchPage()
        {
            if (SearchSelected == true)
            {
                main.Refresh();
                SearchViewModel vm = new SearchViewModel();
                Search wd = new Search();
                wd.DataContext = vm;
                wd.ShowDialog();
            }
        }
        public MainViewModel()
        {

          
            LoadManageButtonCommand = new RelayCommand<Button>((p) => { return true; }, (p) => 
            { 
                if(Role == "Manager")
                {
                    p.Visibility = Visibility.Visible;
                }
                else
                {
                    p.Visibility = Visibility.Hidden;
                }
            });

            LoadWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                isLoad = true;
                if (!LoginViewModel.isReLogin)
                {
                    p.Hide();
                    View.Login wd = new View.Login();
                    wd.ShowDialog();

                    if (wd.DataContext == null)
                        return;

                    if (LoginViewModel.isLogin)
                    {
                        
                        p.Show();
                    }
                    else
                    {
                        p.Close();
                    }
                }

                data = (MainViewModel)p.DataContext;
                Name = LoginViewModel.Name;
                Id = LoginViewModel.Id;
                Role = LoginViewModel.Role;
                loadPic(Id);



            });

            LoadPopuationWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                View.Populations wd = new View.Populations();
                wd.DataContext = new PopulationViewModel();
                wd.ShowDialog();              
            });

            LogoutCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {               
                View.Login wd = new View.Login();
                wd.Show();                             
                LoginViewModel.isLogin = false;
                LoginViewModel.isReLogin = true;
                p.Close();
            });
           
           
        }
       public void loadPic(string Id="0000")
        {
            if (Id == null)
                return;
            int tmp = int.Parse(Id);
            var link = DataProvider.Ins.DB.Users.Where(x => x.Id == tmp).SingleOrDefault();
            if (link != null)
            {
               
                    Photo = check(link.PhotoUser);
                    try
                    {
                        SPhoto = BitmapFromUri(new Uri(System.IO.Path.GetFullPath("../../userhinhthe/" + Photo))); // get picture
                    }
                    catch (Exception e)
                    {
                        SPhoto = BitmapFromUri(new Uri(System.IO.Path.GetFullPath("../../userhinhthe/" + Photo)));
                    }
                
               
            }

        }
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

        private ImageSource BitmapFromUri(Uri source)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmap.UriSource = source;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                return bitmap;
            }
            catch
            {
                source = new Uri(System.IO.Path.GetFullPath("../../Resources/account.jpg"));
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmap.UriSource = source;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                return bitmap;
            }
        }
    }
}
