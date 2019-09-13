using UniRx;

namespace AMARI.Assets.Scripts.Inputs
{
    interface IInputEvetnProvider
    {
        IReadOnlyReactiveProperty<bool> OnUseButtonPushed{ get; }
        IReadOnlyReactiveProperty<bool> OnUseButtonPushing{ get; }
        IReadOnlyReactiveProperty<bool> OnUseButtonUpped{ get; }
    }
}