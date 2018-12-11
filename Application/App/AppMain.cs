﻿namespace Application
{
    using Framework.Application;
    using Framework.Json;
    using Framework.Dal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Linq.Dynamic.Core;
    using Database.dbo;
    using Database.Person;
    using Framework.Dal.Memory;
    using Database.Memory;

    public class AppMain : AppJson
    {
        public AppMain() : this(null) { }

        public AppMain(ComponentJson owner)
            : base(owner)
        {

        }

        protected override async Task InitAsync()
        {
            await this.PageShowAsync<NavigationPage>();
        }
    }

    public class MessageBox : Page
    {
        public MessageBox() : this(null) { }

        public MessageBox(ComponentJson owner)
            : base(owner)
        {

        }

        protected override Task InitAsync()
        {
            new Html() { TextHtml = "Delete record?" };
            ButtonYes();
            ButtonNo();
            return base.InitAsync();
        }

        public Button ButtonYes()
        {
            return this.GetOrCreate<Button>("Yes", (button) => button.Text = "Yes");
        }

        public Button ButtonNo()
        {
            return this.GetOrCreate<Button>("No", (button) => button.Text = "No");
        }

        protected override Task ButtonClickAsync(Button button)
        {
            if (button == ButtonYes())
            {
                new Html(this).TextHtml = "<b>Ok</b>";
                this.IsYes = true;
            }
            if (button == ButtonNo())
            {
                this.IsYes = false;
            }
            return base.ButtonClickAsync(button);
        }

        public bool? IsYes;
    }

    public class NavigationPage : Page
    {
        public NavigationPage() { }

        public NavigationPage(ComponentJson owner)
            : base(owner)
        {

        }

        protected override async Task InitAsync()
        {
            BootstrapNavbar();
            await Grid().LoadAsync();
            Grid().IsHide = true;
            BootstrapNavbar().GridIndex = Grid().Index;
        }

        public BootstrapNavbar BootstrapNavbar()
        {
            return this.GetOrCreate<BootstrapNavbar>((bootstrapNavbar) => { bootstrapNavbar.BrandTextHtml = "<b>Hello</b>World"; });
        }

        protected override IQueryable GridQuery(Grid grid)
        {
            List<Database.Memory.Navigation> list = UtilDal.MemoryRowList<Database.Memory.Navigation>();
            if (list.Count == 0)
            {
                list.Add(new Database.Memory.Navigation() { Id = 1, Text = "<i class='fas fa-home'></i> Home" });
                list.Add(new Database.Memory.Navigation() { Id = 2, Text = "<i class='fas fa-user'></i> User" });
                list.Add(new Database.Memory.Navigation() { Id = 3, Text = "<i class='far fa-address-card'></i> Contact" });
                list.Add(new Database.Memory.Navigation() { Id = 4, Text = "<span class='flag-icon flag-icon-gb'></span> English" });
            }

            return UtilDal.Query<Database.Memory.Navigation>(ScopeEnum.MemorySingleton);
        }

        public Grid Grid()
        {
            return this.GetOrCreate<Grid>((grid) => { grid.CssClass = "container"; });
        }

        protected override async Task GridRowSelectedAsync(Grid grid)
        {
            Navigation navigation = grid.RowSelected() as Navigation;
            if (navigation != null && navigation.Text.Contains("Home"))
            {
                await this.PageShowAsync<HomePage>(ComponentJsonExtension.PageShowEnum.SiblingRemove);
            }
            if (navigation != null && navigation.Text.Contains("User"))
            {
                await this.PageShowAsync<LoginUserPage>(ComponentJsonExtension.PageShowEnum.SiblingRemove);
            }
            if (navigation != null && navigation.Text.Contains("Contact"))
            {
                await this.PageShowAsync<ContactPage>(ComponentJsonExtension.PageShowEnum.SiblingRemove);
            }
        }
    }

    public class LanguagePage : Page
    {
        public LanguagePage() { }

        public LanguagePage(ComponentJson owner)
            : base(owner)
        {

        }

        protected override async Task InitAsync()
        {
            await Grid().LoadAsync();
        }

        protected override IQueryable GridQuery(Grid grid)
        {
            List<Database.Memory.Language> list = UtilDal.MemoryRowList<Database.Memory.Language>();
            if (list.Count == 0)
            {
                list.Add(new Database.Memory.Language() { Id = 1, Text = "English", FlagIcon = "flag-icon-gb" });
                list.Add(new Database.Memory.Language() { Id = 2, Text = "German", FlagIcon = "flag-icon-de" });
                list.Add(new Database.Memory.Language() { Id = 3, Text = "French", FlagIcon = "flag-icon-fr" });
                list.Add(new Database.Memory.Language() { Id = 4, Text = "Italien", FlagIcon = "flag-icon-it" });
            }

            return UtilDal.Query<Database.Memory.Language>(ScopeEnum.MemorySingleton);
        }

        public Div Div()
        {
            return this.GetOrCreate<Div>((div) => { div.CssClass = "container"; });
        }

        public Grid Grid()
        {
            return Div().GetOrCreate<Grid>();
        }
    }

    public class LoginUserPage : Page
    {
        public LoginUserPage() { }

        public LoginUserPage(ComponentJson owner)
            : base(owner)
        {
            this.GetOrCreate<Grid>();
        }

        protected override async Task InitAsync()
        {
            await this.GetOrCreate<Grid>().LoadAsync();
        }

        protected override IQueryable GridQuery(Grid grid)
        {
            return UtilDal.Query<LoginUser>();
        }
    }

    public class HomePage : Page
    {
        public HomePage() { }

        public HomePage(ComponentJson owner)
            : base(owner)
        {
            new Html(this) { TextHtml = @"
            <section id='#'>
                <div class='container'>
                    <h1>Home</h1>
                    <p>Welcome to Hello World!</p>
                    <figure class='figure'>
                        <img src='logo.jpg' class='figure-img img-fluid rounded' alt='Company'>
                        <figcaption class='figure-caption'>Hello world application.</figcaption>
                    </figure>
                </div>
            </section>
            " };
        }
    }

    public class ContactPage : Page
    {
        public ContactPage() { }

        public ContactPage(ComponentJson owner)
            : base(owner)
        {

        }

        protected override async Task InitAsync()
        {
            Label().TextHtml = "MyLabel";
            await this.PageShowAsync<LanguagePage>();
            await this.PageShowAsync<MyPage>();

            new Html(this) { TextHtml = "Delete item: " };
            ButtonDelete();

            var grid = GridContact();
            GridPerson();

            await grid.LoadAsync();
        }

        public Html Label()
        {
            return this.GetOrCreate<Html>();
        }

        public Grid GridContact()
        {
            return this.GetOrCreate<Grid>("Contact", (grid) => { grid.CssClass = "container"; });
        }

        public Grid GridPerson()
        {
            return this.GetOrCreate<Grid>("Person", (grid) => { grid.CssClass = "container"; });
        }

        public Button ButtonDelete()
        {
            return this.GetOrCreate<Button>((button) => button.Text = "Delete");
        }

        protected override IQueryable GridLookupQuery(Grid grid, Row row, string fieldName, string text)
        {
            return UtilDal.Query<HelloWorld>().Where(item => item.Text.StartsWith(text));
        }

        protected override string GridLookupSelected(Grid grid, Row row, string fieldName, Row rowLookupSelected)
        {
            return ((HelloWorld)rowLookupSelected).Text;
        }

        protected override IQueryable GridQuery(Grid grid)
        {
            IQueryable result = null;
            if (grid == GridContact())
            {
                result = UtilDal.Query<vAdditionalContactInfo>();
            }
            if (grid == GridPerson())
            {
                var rowSelected = (vAdditionalContactInfo)GridContact().RowSelected();
                if (rowSelected != null) // Otherwise "new row" has been selected.
                {
                    string firstName = ((vAdditionalContactInfo)GridContact().RowSelected()).FirstName;
                    result = UtilDal.Query<Person>().Where(item => item.FirstName == firstName);
                }
                else
                {
                    return UtilDal.QueryEmpty<vAdditionalContactInfo>();
                }
            }
            return result;
        }

        protected override async Task GridRowSelectedAsync(Grid grid)
        {
            if (grid == GridContact())
            {
                await GridPerson().LoadAsync();
            }
        }

        protected override async Task ButtonClickAsync(Button button)
        {
            if (button == ButtonDelete())
            {
                await this.PageShowAsync<MessageBox>();
            }
        }

        protected override Task ProcessAsync()
        {
            MessageBox messageBox = this.Get<MessageBox>();
            if (messageBox?.IsYes != null)
            {
                if (messageBox.IsYes == true)
                {
                    new Html(this).TextHtml = "Deleted!";
                }
                messageBox.Remove();
            }

            Name = "HelloWorld " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fff");

            Language language = this.Get<LanguagePage>().Grid().RowSelected() as Language;

            Label().TextHtml = language?.Text;

            return base.ProcessAsync();
        }
    }

    public class MyPage : Page
    {
        public MyPage() { }

        public MyPage(ComponentJson owner)
            : base(owner)
        {

        }

        protected override async Task InitAsync()
        {
            await Grid().LoadAsync();
            ButtonDelete();
        }

        public Grid Grid()
        {
            return this.GetOrCreate<Grid>((grid) => { grid.CssClass = "container"; });
        }

        public Button ButtonDelete()
        {
            return this.GetOrCreate((Button button) => button.Text = "Delete");
        }

        protected override IQueryable GridQuery(Grid grid)
        {
            return UtilDal.Query<HelloWorld>();
        }

        protected override void GridQueryConfig(Grid grid, Config config)
        {
            config.ConfigGridQuery = new[] { new FrameworkConfigGridBuiltIn { RowCountMax = 3 } }.AsQueryable();
            config.ConfigFieldQuery = new[] {
                new FrameworkConfigFieldBuiltIn { FieldNameCSharp = "Text", Text = "My Text" },
            }.AsQueryable();
        }

        protected override Task ButtonClickAsync(Button button)
        {
            if (button == ButtonDelete())
            {
                this.PageShowAsync<MessageBox>();
            }
            return base.ButtonClickAsync(button);
        }

        protected override async Task ProcessAsync()
        {
            var messageBox = this.Get<MessageBox>();
            if (messageBox?.IsYes == true)
            {
                var row = Grid().RowSelected();
                if (row != null)
                {
                    await UtilDal.Delete(row);
                    await Grid().LoadAsync();
                }
            }
            if (messageBox?.IsYes != null)
            {
                this.Get<MessageBox>().Remove();
            }
        }
    }

    public class AppSelectorHelloWorld : AppSelector
    {
        protected override Type TypeAppJson()
        {
            return typeof(AppMain);
        }
    }
}
