﻿namespace NWheels.Microservices.Orchestration
{
    public interface IStateMachineCodeBehind<TState, TTrigger>
    {
        void BuildStateMachine(IStateMachineBuilder<TState, TTrigger> machine);
    }
}
