﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Menus;
using Gemini.Framework.Services;
using Gemini.Modules.Inspector;
using Gemini.Modules.Output;
using NWheels.Tools.TestBoard.Modules.ApplicationExplorer;
using NWheels.Tools.TestBoard.Modules.Main;
using NWheels.Tools.TestBoard.Properties;

namespace NWheels.Tools.TestBoard.Modules.StartPage
{
    [Export(typeof(IModule))]
    public class StartPageModule : ModuleBase
    {
        public override void PostInitialize()
        {
            Shell.OpenDocument(IoC.Get<StartPageViewModel>());
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        [Export]
        public static MenuItemGroupDefinition StartPageViewMenuGroup =
            new MenuItemGroupDefinition(Gemini.Modules.MainMenu.MenuDefinitions.ViewMenu, 10);

        [Export]
        public static MenuItemDefinition ShowStartPageMenuItem =
            new CommandMenuItemDefinition<ShowStartPageCommandDefinition>(StartPageViewMenuGroup, 10);
    }
}
