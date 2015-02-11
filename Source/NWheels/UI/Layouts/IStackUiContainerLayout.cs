﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NWheels.UI.Layouts
{
    public interface IStackUiContainerLayout<TModel, TState> : IUiContainerBuilder<TModel, TState>
    {
        IStackUiContainerLayout<TModel, TState> OrientationHorizontal();
        IStackUiContainerLayout<TModel, TState> OrientationVertical();
        IStackUiContainerLayout<TNewModel, TState> BindToModel<TNewModel>(Expression<Func<TModel, TNewModel>> path = null);
        IStackUiContainerLayout<TModel, TNewState> BindToUiState<TNewState>(Expression<Func<TState, TNewState>> path = null);
    }
}
