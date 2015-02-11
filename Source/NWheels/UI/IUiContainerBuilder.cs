﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NWheels.UI.Behaviors;
using NWheels.UI.Widgets;
using NWheels.UI.Layouts;

namespace NWheels.UI
{
    public interface IUiContainerBuilder<TModel, TState> : IBoundUiElementBuilder<TModel, TState>
    {
        IUiWidgetSelector<TModel, TState> Add();
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    public interface IUiWidgetSelector<TModel, TState> : IBoundUiElementBuilder<TModel, TState>
    {
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    public static class UiWidgetChoices
    {
        public static ITextUiWidgetBuilder<TModel, TState> WidgetText<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector, string text)
        {
            return selector.CreateChildBuilder<ITextUiWidgetBuilder<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static ITextFieldUiWidgetBuilder<TModel, TState> WidgetTextField<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<ITextFieldUiWidgetBuilder<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static ICheckboxUiWidgetBuilder<TModel, TState> WidgetCheckbox<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<ICheckboxUiWidgetBuilder<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static ILookupFieldUiWidgetBuilder<TModel, TState, Unbound.Lookup> WidgetLookupField<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<ILookupFieldUiWidgetBuilder<TModel, TState, Unbound.Lookup>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static IButtonUiWidgetBuilder<TModel, TState> WidgetButton<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector, string text = null)
        {
            return selector.CreateChildBuilder<IButtonUiWidgetBuilder<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static IUiCommandBuilder<TModel, TState> WidgetButtonCommand<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            var command = selector.CreateChildBuilder<IUiCommandBuilder<TModel, TState>>();
            var button = selector.CreateChildBuilder<IButtonUiWidgetBuilder<TModel, TState>>();
            button.BindToCommand(command);
            return command;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static IDropDownMenuUiWidgetBuilder<TModel, TState> WidgetDropDownMenu<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<IDropDownMenuUiWidgetBuilder<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static ITabbedContentUiWidgetBuilder<TModel, TState> WidgetTabbedContent<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<ITabbedContentUiWidgetBuilder<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static IDataGridUiWidgetBuilder<TModel, TState, Unbound.Model, Unbound.State> WidgetDataGrid<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<IDataGridUiWidgetBuilder<TModel, TState, Unbound.Model, Unbound.State>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static IImageUiWidgetBuilder<TModel, TState> WidgetImage<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector, string imagePath)
        {
            return selector.CreateChildBuilder<IImageUiWidgetBuilder<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------
        public static ILogoUiWidgetBuilder WidgetLogo<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<ILogoUiWidgetBuilder>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static ILoggedInUserUiWidgetBuilder<TModel, TState> WidgetLoggedInUser<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<ILoggedInUserUiWidgetBuilder<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static IMapUiWidgetBuilder<TModel, TState> WidgetMap<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<IMapUiWidgetBuilder<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static IDockUiContainerLayout<TModel, TState> WidgetLayoutDock<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<IDockUiContainerLayout<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static IStackUiContainerLayout<TModel, TState> WidgetLayoutStack<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<IStackUiContainerLayout<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static IGridUiContainerLayout<TModel, TState> WidgetLayoutGrid<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<IGridUiContainerLayout<TModel, TState>>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static IFlowUiContainerLayout<TModel, TState> WidgetLayoutFlow<TModel, TState>(this IUiWidgetSelector<TModel, TState> selector)
        {
            return selector.CreateChildBuilder<IFlowUiContainerLayout<TModel, TState>>();
        }
    }
}
