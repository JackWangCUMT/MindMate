﻿/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Windows.Forms;
using MindMate.Plugins;
using MindMate.View.NoteEditing;
using MindMate.View;
using MindMate.View.EditorTabs;

namespace MindMate.WinXP
{
    public partial class MainForm : Form, View.IMainForm
    {
                        
        public MainForm()
        {
            InitializeComponent();
            toolStrip1.MainMenu = MainMenu;
            SetupSideBar();

            // moving splitter makes it the focused control, below event focuses the last control again
            splitContainer1.GotFocus += (a, b) => FocusLastControl();
            splitContainer1.MouseDown += SplitContainer1_MouseDown;

            // changing side bar tab gives focus away to tab control header, below event focuses relevant control again
            SideBarTabs.SelectedIndexChanged += SideBarTabs_SelectedIndexChanged;

            EditorTabs = new EditorTabs();
            splitContainer1.Panel1.Controls.Add(EditorTabs);
        }    

        #region Manage Focus

        private Control focusedControl;

        private void FocusLastControl()
        {
            if (focusedControl != null)
                focusedControl.Focus();
            else
                EditorTabs.SelectedTab.Control.Focus();
        }

        public void FocusMapView()
        {
            EditorTabs.Focus();
        }

        private void SplitContainer1_MouseDown(object sender, MouseEventArgs e)
        {
            focusedControl = NoteEditor.Focused ? NoteEditor : EditorTabs.SelectedTab.Control;
        }

        private void SideBarTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SideBarTabs.SelectedTab == SideBarTabs.NoteTab)
                SideBarTabs.SelectedTab.Controls[0].Focus();
            else
                FocusMapView();
        }

        #endregion

		public MainMenu MainMenu { get { return menuStrip; } }
        public MainMenuCtrl MainMenuCtrl { get; set; }        

        public EditorTabs EditorTabs { get; private set; }
        public SideTabControl SideBarTabs { get; private set; }
        public NoteEditor NoteEditor { get; private set; }

        public View.StatusBar StatusBar { get { return this.statusStrip1; } }

        public bool IsNoteEditorActive
        {
            get { return ActiveControl == splitContainer1 && splitContainer1.ActiveControl == NoteEditor; }
        }

        private void SetupSideBar()
        {
            SideBarTabs = new SideTabControl();
            NoteEditor = SideBarTabs.NoteEditor;
            
            this.splitContainer1.Panel2.Controls.Add(SideBarTabs);
        }

        public void InsertMenuItems(MainMenuItem[] menuItems)
        {
            MainMenuCtrl.InsertMenuItems(menuItems);
        }

        public void RefreshRecentFilesMenuItems()
        {
            MainMenuCtrl.RefreshRecentFilesMenuItems();
        }
    }
}
