﻿namespace Application.Cli
{
    using Framework;
    using Framework.Cli;
    using Microsoft.Extensions.CommandLineUtils;
    using System;

    public class AppCli : AppCliBase
    {
        protected override void RegisterCommand()
        {
            new MyCommand(this);

            base.RegisterCommand();
        }

        protected override void InitConfigCli(ConfigCli configCli)
        {
            configCli.WebsiteList.Add(new ConfigCliWebsite()
            {
                DomainName = "default",
                FolderNameNpmBuild = "Website/",
                FolderNameDist = "Website/dist/",
            });
        }
    }

    public class MyCommand : CommandBase
    {
        public MyCommand(AppCliBase appCli) 
            : base(appCli, "My", "My command")
        {

        }

        public CommandOption My;

        protected override void Register(CommandLineApplication configuration)
        {
            this.My = configuration.Option("-m", "My Option", CommandOptionType.NoValue);
        }

        protected override void Execute()
        {
            Console.WriteLine("My");
            if (My.Value() == "on")
            {
                Console.WriteLine("With option");
            }
        }
    }
}
