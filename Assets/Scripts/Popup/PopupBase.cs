using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupBase<T, U> : MonoBehaviour where T : PopupBase<T, U>
{
    public static void Show(U param)
    {
        PopupManager.instance.Show<T, U>(typeof(T).Name, param);
    }
}
