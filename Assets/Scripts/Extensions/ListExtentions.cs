using System.Linq;
using System.Collections.Generic;

namespace AMARI.Assets.Scripts.Extensions
{
    public enum LRSwitch{
        LEFT,
        RIGHT
    };

    public static class StandardFunctionExtentions
    {
        public static string TwoStepConversion(this string extStr)
        {
            return "";
        }
    }
    public static class ListExtentions
    {
        /// <summary>
        /// List<T>にすでに追加されている要素を省いて追加
        /// </summary>
        /// <param name="addObj">追加したい要素</param>
        /// <typeparam name="T">任意のパラメーター</typeparam>
        /// <returns>要素を省いて追加してList<T>を返す</returns>
        public static void AddTriming<T>(this IList<T> extList, T addObj)
        {
            if(extList.Contains(addObj) == true)
            {
                // 何もしない
            }
            else
            {
                extList.Add(addObj);
            }
        }

        /// <summary>
        /// 制限以内のオブジェクト数をList<T>にすでに追加されている要素を省いて追加
        /// </summary>
        /// <param name="extList">追加したい要素</param>
        /// <param name="addObj">任意のパラメーター</param>
        /// <param name="limit">List<T>に追加するデータの制限</param>
        /// <typeparam name="T">任意のパラメータ</typeparam>
        public static void AddTrimingLimited<T>(this IList<T> extList, T addObj, int limit)
        {
            if(extList.Contains(addObj) == true)
            {
                // 何もしない
            }
            else if(extList.Count < limit)
            {
                extList.Add(addObj);
            }
        }

        /// <summary>
        /// List<T>にすでに追加されている要素を省いて追加しながら、追加できたかをbool値で返す
        /// </summary>
        /// <param name="extList"></param>
        /// <param name="addObj"></param>
        /// <typeparam name="T">任意のパラメータ</typeparam>
        /// <returns>真偽値(ture = 追加できた false = すでにある値)</returns>
        public static bool IsAddTriming<T>(this IList<T> extList, T addObj)
        {
            if(extList.Contains(addObj) == true)
            {
                return false;
            }
            else
            {
                extList.Add(addObj);
                return true;
            }
        }

        public static bool TupleContains<TObject, TComponent, TValue>(this IList<(TObject, TComponent)> extList, TValue value, LRSwitch lr)
        {
            if(lr == LRSwitch.LEFT)
            {
                return extList.Select(obj => obj.Item1).Any(item => item.Equals(value));
            }
            else if(lr == LRSwitch.RIGHT)
            {
                return extList.Select(obj => obj.Item2).Any(item => item.Equals(value));
            }
            else
            {
                return default;
            }
        }
        public static TComponent TupleContainsGetComponent<TObject, TComponent, TValue>(this IList<(TObject, TComponent)> extList, TValue value)
        {
            return extList.First(obj => obj.Item1.Equals(value)).Item2;
        }
    }
}