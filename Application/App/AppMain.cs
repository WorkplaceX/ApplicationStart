﻿namespace Application
{
    using Framework.Application;
    using Framework.Json;
    using Framework.Dal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DatabaseCustom.Person;
    using Microsoft.EntityFrameworkCore;
    using Database.dbo;
    using System.Threading.Tasks;
    using System.Linq.Dynamic.Core;
    using Framework.Server;

    /// <summary>
    /// Main application.
    /// </summary>
    public class AppMain : App
    {
        protected override async Task InitAsync()
        {
            await AppJson.PageShowAsync<PageMain>();
            new Button(AppJson) { Text = "Click" };
            new Button(AppJson) { Text = "Click2" };
            MyButton().Text = "MyClick";

            var grid = GridContact();
            GridPerson();

            await grid.LoadAsync();
        }

        public Button MyButton()
        {
            return AppJson.CreateOrGet<Button>("MyButton");
        }

        public Grid GridContact()
        {
            return AppJson.CreateOrGet<Grid>("Contact");
        }

        public Grid GridPerson()
        {
            return AppJson.CreateOrGet<Grid>("Person");
        }

        /// <summary>
        /// Returns query to load data grid.
        /// </summary>
        protected override IQueryable GridLoadQuery(Grid grid)
        {
            if (grid == GridContact())
            {
                return UtilDal.Query<vAdditionalContactInfo>();
            }
            if (grid == GridPerson())
            {
                string firstName = ((vAdditionalContactInfo)GridContact().RowSelected()).FirstName;
                return UtilDal.Query<Person>().Where(item => item.FirstName == firstName);
            }
            return null;
        }

        /// <summary>
        /// Override this method to execute action after selected row changed. For example master, detail.
        /// </summary>
        protected override async Task GridRowSelectChangeAsync(Grid grid)
        {
            if (grid == GridContact())
            {
                await GridPerson().LoadAsync();
            }
        }

        protected override Task ButtonClickAsync(Button button)
        {
            if (button == MyButton())
            {

            }
            return base.ButtonClickAsync(button);
        }

        protected override Task ProcessAsync()
        {
            AppJson.Name = "HelloWorld " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
            return base.ProcessAsync();
        }
    }

    public class PageMain : Page
    {
        public PageMain() : this(null) { }

        public PageMain(ComponentJson owner)
            : base(owner)
        {

        }

        protected override Task InitAsync()
        {
            ButtonYes().Text = "Yes";
            ButtonNo().Text = "No";
            return base.InitAsync();
        }

        public Button ButtonYes()
        {
            return this.CreateOrGet<Button>("Yes");
        }

        public Button ButtonNo()
        {
            return this.CreateOrGet<Button>("No");
        }

        protected override Task ButtonClickAsync(Button button)
        {
            if (button == ButtonYes())
            {

            }
            return base.ButtonClickAsync(button);
        }
    }

    public class AppSelectorHelloWorld : AppSelector
    {
        protected override App CreateApp()
        {
            return new AppMain();
        }
    }
}
