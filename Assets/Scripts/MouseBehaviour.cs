using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

namespace AMARI.Assets.Scripts
{
    public class MouseBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            List<GameObject> cubeObjectList = new List<GameObject>();
            // マウスホールド時の挙動
            var mouseHold = this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0))
                .Select(_ => GetObjectByRayCastHit())
                .Where(cube => cubeObjectList.IsAddTriming(cube) & GetObjectByRayCastHit() != null)
                .Subscribe(_ => {
                    ExecuteEvents.Execute<IMessageProvider>(
                        target: gameObject,
                        eventData: null,
                        functor: (reciever, eventData) => {
                            reciever.OnRecievedOneShotMaterialChange(GetObjectByRayCastHit());
                            reciever.OnRecievedOneShotChangeNumbers(GetObjectByRayCastHit());
                        }
                    );
                });

            // マウスボタンリリース時の挙動
            var mouseRelease = this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Subscribe(_ => {
                    ExecuteEvents.Execute<IMessageProvider>(
                        target: gameObject,
                        eventData: null,
                        functor: (reciever, eventData) => reciever.OnRecievedMaterialAllChange()
                    );
                    cubeObjectList.Clear();
                });
        }
        private GameObject GetObjectByRayCastHit()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if(Physics.Raycast(ray.origin, ray.direction, out hit, 100.0f))
            {
                return hit.collider.gameObject;
            }
            return null;
        }   
    }
}