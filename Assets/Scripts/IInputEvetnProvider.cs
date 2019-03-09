using UniRx;
using UnityEngine;

namespace Inputs
{
    public interface IInputEvetnProvider
    {
        IReadOnlyReactiveProperty<Vector3> MoveDirection{ get; }
        IReadOnlyReactiveProperty<bool> OnUseButtonPushed{ get; }
    }
}