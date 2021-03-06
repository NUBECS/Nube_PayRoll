﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NUBE.PAYROLL.PL
{
    /// <summary>
    /// Interaction logic for frmHome.xaml
    /// </summary>
    public partial class frmHome : MetroWindow
    {
        public frmHome()
        {
            InitializeComponent();
            foreach (CMN.NavMenuItem m in lstMaster.Items)
            {
                if (m.MenuName == "NUBE Branch")
                {
                    if (Config.bIsNubeServer == false)
                    {
                        lstMaster.Items.Remove(m);
                        break;
                    }
                }
            }

            foreach (CMN.NavMenuItem m in lstTransaction.Items)
            {
                if (m.MenuName == "PCB")
                {
                    if (Config.bIsNubeServer == false)
                    {
                        lstTransaction.Items.Remove(m);
                        break;
                    }
                }
            }

            foreach (CMN.NavMenuItem m in lstTransaction.Items)
            {
                if (m.MenuName == "Monthly Deductions")
                {
                    if (Config.bIsNubeServer == true)
                    {
                        lstTransaction.Items.Remove(m);
                        break;
                    }
                }
            }

            foreach (CMN.NavMenuItem m in lstReports.Items)
            {
                if (m.MenuName == "Consolidate Salary Report (A3)")
                {
                    if (Config.bIsNubeServer == true)
                    {
                        lstReports.Items.Remove(m);
                        break;
                    }
                }
            }
        }

        public void ShowWelcome()
        {
            ccHomeContent.Content = new frmWelcome();
        }

        public void ShowForm(object o)
        {
            ccHomeContent.Content = o;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to exit?", "Exit Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void ListBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var dependencyObject = Mouse.Captured as DependencyObject;
                while (dependencyObject != null)
                {
                    if (dependencyObject is ScrollBar) return;
                    dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
                }
                ListBox lb = sender as ListBox;
                CMN.NavMenuItem mi = lb.SelectedItem as CMN.NavMenuItem;

                //if (!BLL.UserAccount.AllowFormShow(mi.FormName))
                //{

                //  //  MessageBox.Show(string.Format(Message.PL.DenyFormShow, mi.MenuName));
                //}
                //else
                //{
                if (mi.FormName == "Home")
                {
                    ccContent.Content = null;
                    ccHomeContent.Content = mi.Content;
                }
                else
                {
                    ccHomeContent.Content = null;
                    ccContent.Content = mi.Content;
                }

                //}
            }
            catch (Exception ex)
            {
                NUBE.PAYROLL.CMN.ExceptionLogging.SendErrorToText(ex);
            }
            MenuToggleButton.IsChecked = false;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
