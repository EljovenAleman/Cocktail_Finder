using Cocktail_Finder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Cocktail_Finder
{
    /// <summary>
    /// Lógica de interacción para SearchByIngredientWindow.xaml
    /// </summary>
    public partial class SearchByIngredientWindow : Window
    {
        DrinkList drinks = new DrinkList();
        DrinkDetailsList drinkDetailsList = new DrinkDetailsList();

        public SearchByIngredientWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchByIngredient(TextBox_Ingredients.Text);
        }

        private async Task SearchByIngredient(string ingredient)
        {
            try
            {
                
                string url = "https://www.thecocktaildb.com/api/json/v1/1/filter.php?i=" + ingredient;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    HttpResponseMessage response = await client.GetAsync(url);

                    string content = response.Content.ReadAsStringAsync().Result;
                    
                    drinks = GetCocktail_Info(content);
                }

                ShowCocktailsInMenu(drinks.drinks);
            }
            catch(Exception ex)
            {

            }
            
        }

        private void ShowCocktailsInMenu(List<Cocktail_Info> cocktails)
        {
            foreach(var cocktail in cocktails)
            {
                MenuItem menuItem = new MenuItem();

                menuItem.Header = cocktail.strDrink;

                menuItem.Click += new RoutedEventHandler(MenuItem_Click);

                Menu_Drinks.Items.Add(menuItem);
            }
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if(menuItem != null)
            {
                string imageURL = GetImageUrl(menuItem.Header.ToString());
                
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imageURL);
                bitmap.EndInit();

                Image_drinkImage.Source = bitmap;

                string drinkId = GetDrinkId(menuItem.Header.ToString());

                GetDrinkDetailsList(drinkId);

                ShowDrinkDetails(drinkDetailsList.drinks);
            }
        }

        private void ShowDrinkDetails(List<Drink_Details> details)
        {
            TextBox_Instruccions.Text = details[0].strInstructions;

            ListBox_Ingredients.Items.Clear();
            ListBox_Ingredients.Items.Add(details[0].strIngredient1);
            ListBox_Ingredients.Items.Add(details[0].strIngredient2);
            ListBox_Ingredients.Items.Add(details[0].strIngredient3);
            ListBox_Ingredients.Items.Add(details[0].strIngredient4);
            ListBox_Ingredients.Items.Add(details[0].strIngredient5);
            ListBox_Ingredients.Items.Add(details[0].strIngredient6);
            ListBox_Ingredients.Items.Add(details[0].strIngredient7);
            ListBox_Ingredients.Items.Add(details[0].strIngredient8);
            ListBox_Ingredients.Items.Add(details[0].strIngredient9);
            ListBox_Ingredients.Items.Add(details[0].strIngredient10);
            ListBox_Ingredients.Items.Add(details[0].strIngredient11);
            ListBox_Ingredients.Items.Add(details[0].strIngredient12);
            ListBox_Ingredients.Items.Add(details[0].strIngredient13);
            ListBox_Ingredients.Items.Add(details[0].strIngredient14);
            ListBox_Ingredients.Items.Add(details[0].strIngredient15);

        }

        private async Task GetDrinkDetailsList(string drinkId)
        {
            try
            {
                string url = "https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i=" + drinkId;
            
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    
                    HttpResponseMessage response = client.GetAsync(url).Result;                    

                    string content = response.Content.ReadAsStringAsync().Result;
                                        
                    drinkDetailsList = GetDrink_Details(content);
                }
                
            }
            catch(Exception ex)
            {
            
            }            
        }


        private string GetDrinkId(string header)
        {
            string drinkID = null;

            foreach (var drink in drinks.drinks)
            {
                if (header == drink.strDrink)
                {
                    drinkID = drink.idDrink;
                    break;
                }
            }
            
            return drinkID;
        }

        private string GetImageUrl(string header)
        {
            string imageUrl = null;

            foreach(var drink in drinks.drinks)
            {
                if(header == drink.strDrink)
                {
                    imageUrl = drink.strDrinkThumb;
                    break;
                }
            }

            return imageUrl;
        }
        private DrinkDetailsList GetDrink_Details(string content)
        {
            DrinkDetailsList drinkDetailsList = JsonConvert.DeserializeObject<DrinkDetailsList>(content);

            return drinkDetailsList;
        }

        private DrinkList GetCocktail_Info(string content)
        {            
            DrinkList drinkList = JsonConvert.DeserializeObject<DrinkList>(content);

            return drinkList;
        }
    }
}
